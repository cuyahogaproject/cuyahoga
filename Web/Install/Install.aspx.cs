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
		}

		private void ShowMessage(string message)
		{
			this.lblMessage.Text = message;
			this.pnlMessage.Visible = true;
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
						this.pnlFinished.Visible = true;
					}
					catch (Exception ex)
					{
						ShowError("An error occured while creating the administrator: <br>" + ex.ToString());
					}					
				}
			}
		}
	}
}
