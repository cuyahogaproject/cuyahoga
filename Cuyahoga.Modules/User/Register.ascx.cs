namespace Cuyahoga.Modules.User
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Domain;
	using Cuyahoga.Core.Service;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Core;

	/// <summary>
	///		Summary description for Register.
	/// </summary>
	public partial class Register : BaseModuleControl
	{
		private ProfileModule _module;


		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as ProfileModule;

			if (! this.IsPostBack)
			{
				// Databind is required to bind the localized resources.
				this.DataBind();
			}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void btnRegister_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid && this.IsPostBack)
			{
				// Check if username already exists.
				if (this._module.CheckIfUserExists(this.txtUsername.Text))
				{
					this.lblError.Text = String.Format(GetText("USEREXISTS"), this.txtUsername.Text);
					this.lblError.Visible = true;
				}
				else
				{
					Site site = base.PageEngine.ActiveNode.Site;
					// OK, create new user.
					try
					{
						this._module.RegisterUser(this.txtUsername.Text, this.txtEmail.Text);
						this.pnlConfirmation.Visible = true;
						this.lblConfirmation.Text = String.Format(GetText("REGISTERCONFIRMATION"), this.txtEmail.Text);
					}
					catch (EmailException)
					{
						this.lblError.Text = GetText("REGISTEREMAILERROR");
						this.lblError.Visible = true;
					}
					catch (Exception ex)
					{
						this.lblError.Text = ex.Message;
						this.lblError.Visible = true;
					}

					this.pnlRegister.Visible = false;
				}
			}
		}
	}
}
