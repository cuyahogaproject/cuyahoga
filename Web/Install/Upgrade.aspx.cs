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

using Cuyahoga.Core;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Install
{
	/// <summary>
	/// Summary description for Upgrade.
	/// </summary>
	public class Upgrade : System.Web.UI.Page
	{
		private CoreRepository _coreRepository;

		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Panel pnlErrors;
		protected System.Web.UI.WebControls.Label lblCoreAssemblyCurrent;
		protected System.Web.UI.WebControls.Label lblModulesAssemblyCurrent;
		protected System.Web.UI.WebControls.Label lblCoreAssemblyNew;
		protected System.Web.UI.WebControls.Label lblModulesAssemblyNew;
		protected System.Web.UI.WebControls.Button btnUpgradeDatabase;
		protected System.Web.UI.WebControls.HyperLink hplSite;
		protected System.Web.UI.WebControls.HyperLink hplAdmin;
		protected System.Web.UI.WebControls.Panel pnlFinished;
		protected System.Web.UI.WebControls.Panel pnlIntro;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._coreRepository = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
			this.pnlErrors.Visible = false;

			// Check security first
			Cuyahoga.Core.Domain.User cuyahogaUser = this.User.Identity as Cuyahoga.Core.Domain.User;
			if (! cuyahogaUser.HasPermission(AccessLevel.Administrator))
			{
				throw new AccessForbiddenException("Upgrade not allowed for the current user");
			}

			if (! this.IsPostBack)
			{
				try
				{
					// Check upgradable state. Check both Cuyahoga.Core and Cuyahoga.Modules.
					bool canUpgrade = false;

					// Core
					DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Cuyahoga.Core"));
					if (dbInstaller.CanUpgrade)
					{
						canUpgrade = true;
						lblCoreAssemblyCurrent.Text = "Cuyahoga Core " + dbInstaller.CurrentVersionInDatabase.ToString(3);
						lblCoreAssemblyNew.Text = "Cuyahoga Core " + dbInstaller.NewAssemblyVersion.ToString(3);
						// Core modules
						DatabaseInstaller moduleDbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Modules"), Assembly.Load("Cuyahoga.Modules"));
						lblModulesAssemblyCurrent.Text = "Cuyahoga Core Modules " + moduleDbInstaller.CurrentVersionInDatabase.ToString(3);
						if (moduleDbInstaller.CanUpgrade)
						{
							lblModulesAssemblyNew.Text = "Cuyahoga Core Modules " + moduleDbInstaller.NewAssemblyVersion.ToString(3);
						}
						else
						{
							lblModulesAssemblyNew.Text = "Cuyahoga Core Modules - no upgrade available";
						}
					}
					if (canUpgrade)
					{
						this.pnlIntro.Visible = true;
					}
					else
					{
						ShowError("Nothing to upgrade.");
					}
				}
				catch (Exception ex)
				{
					ShowError("An error occured: <br/><br/>" + ex.ToString());
				}
			}
		}

		private void ShowError(string message)
		{
			this.lblError.Text = message;
			this.pnlErrors.Visible = true;
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
			this.btnUpgradeDatabase.Click += new System.EventHandler(this.btnUpgradeDatabase_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnUpgradeDatabase_Click(object sender, System.EventArgs e)
		{
			DatabaseInstaller dbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Core"), Assembly.Load("Cuyahoga.Core"));
			DatabaseInstaller modulesDbInstaller = new DatabaseInstaller(Server.MapPath("~/Install/Modules"), Assembly.Load("Cuyahoga.Modules"));

			try
			{
				dbInstaller.Upgrade();
				if (modulesDbInstaller.CanUpgrade)
				{
					modulesDbInstaller.Upgrade();
				}
				this.pnlIntro.Visible = false;
				this.pnlFinished.Visible = true;

				// Remove the IsUpgrading flag, so users can view the pages again.
				HttpContext.Current.Application.Lock();
				HttpContext.Current.Application["IsUpgrading"] = false;
				HttpContext.Current.Application.UnLock();
			}
			catch (Exception ex)
			{
				ShowError("An error occured while installing the database tables: <br/>" + ex.ToString());
			}
		}
	}
}
