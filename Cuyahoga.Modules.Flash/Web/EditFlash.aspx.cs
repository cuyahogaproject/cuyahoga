using System;
using System.Web.UI.WebControls;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.UI;
using FredCK.FCKeditorV2;
using Cuyahoga.Modules.Flash.Domain;

namespace Cuyahoga.Modules.Flash.Web
{
	/// <summary>
	/// Summary description for EditHtml.
	/// </summary>
	public class EditFlash : ModuleAdminBasePage
	{
		private FlashModule _module;
		protected FCKeditor fckEditor;
		protected Label lblMessage;
		protected Button btnSave;
	
		private void Page_Load(object sender, EventArgs e)
		{
			this.fckEditor.BasePath = this.Page.ResolveUrl("~/Support/FCKEditor/");
			this._module = base.Module as FlashModule;
			if(this.Section.Settings["ALTERNATEDIVID"].ToString() == string.Empty)
			{
				this.fckEditor.Visible = true;
				this.btnSave.Visible = true;
				lblMessage.Text = "";
				if (! this.IsPostBack)
				{
					AlternateContent shc = this._module.GetContent();
					if (shc != null)
					{
						this.fckEditor.Value = shc.Content;
					}
					else
					{
						this.fckEditor.Value = String.Empty;
					}
				}
			}
			else
			{
				this.fckEditor.Visible = false;
				this.btnSave.Visible = false;
				lblMessage.Text = "An alternate Div Id was provided for this section.";
			}
				
		}

		private void SaveStaticHtml()
		{
			User currentUser = (User)Context.User.Identity;
			AlternateContent content = this._module.GetContent();
			if (content == null)
			{
				// New
				content = new AlternateContent();
				content.Section = this._module.Section;
				content.CreatedBy = currentUser;
				content.ModifiedBy = currentUser;
			}
			else
			{
				// Exisiting
				content.ModifiedBy = currentUser;
			}
			content.Content = this.fckEditor.Value;
			this._module.SaveContent(content);	
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
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				SaveStaticHtml();
				ShowMessage("Content saved.");
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}	
	}
}
