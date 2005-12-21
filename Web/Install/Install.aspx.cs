using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.Install
{
	/// <summary>
	/// Summary description for Install.
	/// </summary>
	public class Install : System.Web.UI.Page
	{
		private CoreRepository _coreRepository;

		protected System.Web.UI.WebControls.Panel pnlErrors;
		protected System.Web.UI.WebControls.Panel pnlIntro;
		protected System.Web.UI.WebControls.Panel pnlAdmin;
		protected System.Web.UI.WebControls.HyperLink hplContinue;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnInstallDatabase;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.Button btnAdmin;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvPassword;
		protected System.Web.UI.WebControls.Label lblCoreAssembly;
		protected System.Web.UI.WebControls.Label lblModulesAssembly;
		protected System.Web.UI.WebControls.TextBox txtConfirmPassword;
		protected System.Web.UI.WebControls.CompareValidator cpvPassword;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvConfirmPassword;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.Panel pnlMessage;
		protected System.Web.UI.WebControls.Panel pnlCreateSite;
		protected System.Web.UI.WebControls.Button btnCreateSite;
		protected System.Web.UI.WebControls.Button btnSkipCreateSite;
		protected System.Web.UI.WebControls.Panel pnlFinished;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._coreRepository = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
			this.pnlErrors.Visible = false;

			if (! this.IsPostBack)
			{
				try
				{
					// Check installable state. Check both Cuyahoga.Core and Cuyahoga.Modules.
					bool canInstall = false;
					// Core
					DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Cuyahoga.Core"));
					if (dbInstaller.CanInstall)
					{
						lblCoreAssembly.Text = "Cuyahoga Core " + dbInstaller.NewAssemblyVersion.ToString(3);
						// Core modules
						DatabaseInstaller moduleDbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Modules"), Assembly.Load("Cuyahoga.Modules"));
						if (moduleDbInstaller.CanInstall)
						{
							canInstall = true;
							lblModulesAssembly.Text = "Cuyahoga Core Modules " + moduleDbInstaller.NewAssemblyVersion.ToString(3);
						}
					}
					if (canInstall)
					{
						this.pnlIntro.Visible = true;
					}
					else
					{
						// Check if we perhaps need to add an admin
						if (this._coreRepository.GetAll(typeof(Cuyahoga.Core.Domain.User)).Count == 0)
						{
							this.pnlAdmin.Visible = true;
						}
						else
						{
							ShowError("Can't install Cuyahoga. Check if the install.sql file exists in the /Install/Core|Modules/Database/DATABASE_TYPE/ directory and that there isn't already a version installed.");
						}
					}
				}
				catch (Exception ex)
				{
					ShowError("An error occured: <br><br>" + ex.ToString());
				}
			}
		}

		private void ShowError(string message)
		{
			this.lblError.Text = message;
			this.pnlErrors.Visible = true;
			this.pnlMessage.Visible = false;
		}

		private void ShowMessage(string message)
		{
			this.lblMessage.Text = message;
			this.pnlMessage.Visible = true;
			this.pnlErrors.Visible = false;
		}

		private void CreateSite()
		{
			Cuyahoga.Core.Domain.User adminUser = this._coreRepository.GetObjectById(typeof(Cuyahoga.Core.Domain.User), 1) as Cuyahoga.Core.Domain.User;
			Template defaultTemplate = this._coreRepository.GetObjectByDescription(typeof(Template), "Name", "Another Red") as Template;
			Role defaultAuthenticatedRole = this._coreRepository.GetObjectByDescription(typeof(Role), "Name", "Authenticated user") as Role;

			// Site
			Site site = new Site();
			site.Name = "Cuyahoga Sample Site";
			site.SiteUrl = UrlHelper.GetSiteUrl();
			site.WebmasterEmail = "webmaster@localhost";
			site.UseFriendlyUrls = true;
			site.DefaultCulture = "en-US";
			site.DefaultTemplate = defaultTemplate;
			site.DefaultPlaceholder = "maincontent";
			site.DefaultRole = defaultAuthenticatedRole;
			this._coreRepository.SaveObject(site);

			// Root node
			Node rootNode = new Node();
			rootNode.Culture = site.DefaultCulture;
			rootNode.Position = 0;
			rootNode.ShortDescription = "home";
			rootNode.ShowInNavigation = true;
			rootNode.Site = site;
			rootNode.Template = defaultTemplate;
			rootNode.Title = "Home";
			IList allRoles = this._coreRepository.GetAll(typeof(Role));
			foreach (Role role in allRoles)
			{
				NodePermission np = new NodePermission();
				np.Node = rootNode;
				np.Role = role;
				np.ViewAllowed = true;
				np.EditAllowed = role.HasPermission(AccessLevel.Administrator);
				rootNode.NodePermissions.Add(np);
			}
			this._coreRepository.SaveObject(rootNode);

			// Sections on root Node
			Section loginSection = new Section();
			loginSection.ModuleType = this._coreRepository.GetObjectByDescription(typeof(ModuleType), "Name", "User") as ModuleType;
			loginSection.Title = "Login";
			loginSection.CacheDuration = 0;
			loginSection.Node = rootNode;
			loginSection.PlaceholderId = "side1content";
			loginSection.Position = 0;
			loginSection.ShowTitle = true;
			loginSection.Settings.Add("SHOW_EDIT_PROFILE", "True");
			loginSection.Settings.Add("SHOW_RESET_PASSWORD", "True");
			loginSection.Settings.Add("SHOW_REGISTER", "True");
			loginSection.CopyRolesFromNode();
			rootNode.Sections.Add(loginSection);
			this._coreRepository.SaveObject(loginSection);
			Section introSection = new Section();
			introSection.ModuleType = this._coreRepository.GetObjectByDescription(typeof(ModuleType), "Name", "StaticHtml") as ModuleType;
			introSection.Title = "Welcome";
			introSection.CacheDuration = 0;
			introSection.Node = rootNode;
			introSection.PlaceholderId = "maincontent";
			introSection.Position = 0;
			introSection.ShowTitle = true;
			introSection.CopyRolesFromNode();
			rootNode.Sections.Add(introSection);
			this._coreRepository.SaveObject(introSection);
			
			// Pages
			Node page1 = new Node();
			page1.Culture = site.DefaultCulture;
			page1.Position = 0;
			page1.ShortDescription = "page1";
			page1.ShowInNavigation = true;
			page1.Site = site;
			page1.Template = defaultTemplate;
			page1.Title = "Articles";
			page1.ParentNode = rootNode;
			page1.CopyRolesFromParent();
			this._coreRepository.SaveObject(page1);
			Section articleSection = new Section();
			articleSection.ModuleType = this._coreRepository.GetObjectByDescription(typeof(ModuleType), "Name", "Articles") as ModuleType;
			articleSection.Title = "Articles";
			articleSection.CacheDuration = 0;
			articleSection.Node = page1;
			articleSection.PlaceholderId = "maincontent";
			articleSection.Position = 0;
			articleSection.ShowTitle = true;
			articleSection.Settings.Add("DISPLAY_TYPE", "FullContent");
			articleSection.Settings.Add("ALLOW_ANONYMOUS_COMMENTS", "True");
			articleSection.Settings.Add("ALLOW_COMMENTS", "True");
			articleSection.Settings.Add("SORT_BY", "DateOnline");
			articleSection.Settings.Add("SORT_DIRECTION", "DESC");
			articleSection.Settings.Add("ALLOW_SYNDICATION", "True");
			articleSection.Settings.Add("NUMBER_OF_ARTICLES_IN_LIST", "5");
			articleSection.CopyRolesFromNode();
			page1.Sections.Add(articleSection);
			this._coreRepository.SaveObject(articleSection);
			Node page2 = new Node();
			page2.Culture = site.DefaultCulture;
			page2.Position = 1;
			page2.ShortDescription = "page2";
			page2.ShowInNavigation = true;
			page2.Site = site;
			page2.Template = defaultTemplate;
			page2.Title = "Page 2";
			page2.ParentNode = rootNode;
			page2.CopyRolesFromParent();
			this._coreRepository.SaveObject(page2);
			Section page2Section = new Section();
			page2Section.ModuleType = this._coreRepository.GetObjectByDescription(typeof(ModuleType), "Name", "StaticHtml") as ModuleType;
			page2Section.Title = "Page 2";
			page2Section.CacheDuration = 0;
			page2Section.Node = page2;
			page2Section.PlaceholderId = "maincontent";
			page2Section.Position = 0;
			page2Section.ShowTitle = true;
			page2Section.CopyRolesFromNode();
			rootNode.Sections.Add(page2Section);
			this._coreRepository.SaveObject(page2Section);

			// User Profile node
			Node userProfileNode = new Node();
			userProfileNode.Culture = site.DefaultCulture;
			userProfileNode.Position = 2;
			userProfileNode.ShortDescription = "userprofile";
			userProfileNode.ShowInNavigation = false;
			userProfileNode.Site = site;
			userProfileNode.Template = defaultTemplate;
			userProfileNode.Title = "User Profile";
			userProfileNode.ParentNode = rootNode;
			userProfileNode.CopyRolesFromParent();
			this._coreRepository.SaveObject(userProfileNode);
			Section userProfileSection = new Section();
			userProfileSection.ModuleType = this._coreRepository.GetObjectByDescription(typeof(ModuleType), "Name", "UserProfile") as ModuleType;
			userProfileSection.Title = "User Profile";
			userProfileSection.CacheDuration = 0;
			userProfileSection.Node = userProfileNode;
			userProfileSection.PlaceholderId = "maincontent";
			userProfileSection.Position = 0;
			userProfileSection.ShowTitle = false;
			userProfileSection.CopyRolesFromNode();
			userProfileNode.Sections.Add(userProfileSection);
			this._coreRepository.SaveObject(userProfileSection);

			// Connections from Login to User Profile
			loginSection.Connections.Add("Register", userProfileSection);
			loginSection.Connections.Add("ResetPassword", userProfileSection);
			loginSection.Connections.Add("ViewProfile", userProfileSection);
			loginSection.Connections.Add("EditProfile", userProfileSection);
			this._coreRepository.UpdateObject(loginSection);

			// Connection from Articles to User Profile
			articleSection.Connections.Add("ViewProfile", userProfileSection);
			this._coreRepository.UpdateObject(articleSection);
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnInstallDatabase.Click += new System.EventHandler(this.btnInstallDatabase_Click);
			this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
			this.btnCreateSite.Click += new System.EventHandler(this.btnCreateSite_Click);
			this.btnSkipCreateSite.Click += new System.EventHandler(this.btnSkipCreateSite_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnInstallDatabase_Click(object sender, System.EventArgs e)
		{	
			DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Cuyahoga.Core"));
			DatabaseInstaller modulesDbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Modules"), Assembly.Load("Cuyahoga.Modules"));

			try
			{
				dbInstaller.Install();
				modulesDbInstaller.Install();
				this.pnlIntro.Visible = false;
				this.pnlAdmin.Visible = true;
				ShowMessage("Database tables successfully created.");
			}
			catch (Exception ex)
			{
				ShowError("An error occured while installing the database tables: <br>" + ex.ToString());
			}
		}

		private void btnAdmin_Click(object sender, System.EventArgs e)
		{
			if (this.IsValid)
			{
				// Only create an admin if there are really NO users.
				if (this._coreRepository.GetAll(typeof(Cuyahoga.Core.Domain.User)).Count > 0)
				{
					ShowError("There is already a user in the database. For security reasons Cuyahoga won't add a new user!");
				}
				else
				{
					Cuyahoga.Core.Domain.User newAdmin = new Cuyahoga.Core.Domain.User();
					newAdmin.UserName = "admin";
					newAdmin.Email = "webmaster@yourdomain.com";
					newAdmin.Password = Cuyahoga.Core.Domain.User.HashPassword(this.txtPassword.Text);
					newAdmin.IsActive = true;
					newAdmin.TimeZone = 0;

					try
					{
						Role adminRole = (Role)this._coreRepository.GetObjectById(typeof(Role), 1);	
						newAdmin.Roles.Add(adminRole);
						this._coreRepository.SaveObject(newAdmin);
						this.pnlAdmin.Visible = false;
						this.pnlCreateSite.Visible = true;
					}
					catch (Exception ex)
					{
						ShowError("An error occured while creating the administrator: <br>" + ex.ToString());
					}					
				}
			}
		}

		private void btnCreateSite_Click(object sender, System.EventArgs e)
		{
			try
			{
				CreateSite();
				this.pnlCreateSite.Visible = false;
				this.pnlFinished.Visible = true;
			}
			catch (Exception ex)
			{
				ShowError("An error occured while creating the site: <br>" + ex.ToString());
			}
		}

		private void btnSkipCreateSite_Click(object sender, System.EventArgs e)
		{
			this.pnlCreateSite.Visible = false;
			this.pnlFinished.Visible = true;
		}
	}
}
