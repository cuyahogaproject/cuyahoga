using System;
using System.Collections;
using System.ComponentModel;
// using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using FreeTextBoxControls;

using Cuyahoga.Web.Util;
using Cuyahoga.Web.UI;
using Cuyahoga.Modules.DAL;
using Cuyahoga.Modules.StaticHtml;

namespace Cuyahoga.Web.Modules.StaticHtml
{
	/// <summary>
	/// Summary description for EditHtml.
	/// </summary>
	public class EditHtml : ModuleAdminBasePage
	{
		private StaticHtmlModule _module;

		protected System.Web.UI.WebControls.Button btnSave;
		protected FreeTextBoxControls.FreeTextBox ftbStaticHtml;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Section.Module as StaticHtmlModule;

			if (! this.IsPostBack)
			{
				if (this._module.StaticHtmlContent != null)
				{
					this.ftbStaticHtml.Text = this._module.StaticHtmlContent.Content;
				}
				InsertFtbWordClean();
			}
		}

		private void InsertFtbWordClean()
		{
			// TODO: move this to a central place
			Toolbar toolbar1 = new Toolbar();
			ToolbarButton myButton = new ToolbarButton("Clean Word html", "FTB_WordClean", "wordclean");
			myButton.ScriptBlock = @"
				function FTB_WordClean(ftbName) {
				var wordData = editor.document.body.innerHTML; 
				if (wordData.indexOf('class=Mso')>=0){ 
					// make one line 
					wordData = wordData.replace(/\r\n/g, ''); 
					wordData = wordData.replace(/\n/g, ''); 
					wordData = wordData.replace(/\r/g, '');       
					wordData = wordData.replace(/\&nbsp\;/g,''); 
					// keep tags, strip attributes 
					wordData = wordData.replace(/ class=[^\s|>]*/gi,''); 
					//wordData = wordData.replace(/<p [^>]*TEXT-ALIGN: justify[^>]*>/gi,'<p align=""justify"">'); 
					wordData = wordData.replace(/ style=\""[^>]*\""/gi,''); 
					//clean up tags 
					wordData = wordData.replace(/<b [^>]*>/gi,'<b>'); 
					wordData = wordData.replace(/<i [^>]*>/gi,'<i>'); 
					wordData = wordData.replace(/<li [^>]*>/gi,'<li>'); 
					wordData = wordData.replace(/<ul [^>]*>/gi,'<ul>'); 
					// replace outdated tags 
					wordData = wordData.replace(/<b>/gi,'<strong>'); 
					wordData = wordData.replace(/<\/b>/gi,'</strong>'); 
					wordData = wordData.replace(/<i>/gi,'<em>'); 
					wordData = wordData.replace(/<\/i>/gi,'</em>'); 
					// kill unwanted tags 
					wordData = wordData.replace(/<\?xml\:[^>]*>/g, ''); // Word xml 
					wordData = wordData.replace(/<\/?st1\:[^>]*>/g,''); // Word SmartTags 
					wordData = wordData.replace(/<\/st1\:[^>]*>/g,''); // 
					wordData = wordData.replace(/<st1\:[^>]*>/g,''); // 
					wordData = wordData.replace(/<\/?[a-z]\:[^>]*>/g,''); // All other funny Word non-HTML stuff 
					wordData = wordData.replace(/<\/[a-z]\:[^>]*\/>/g,''); // 
					wordData = wordData.replace(/<\/?font[^>]*>/gi,''); // Disable if you want to keep font formatting 
					wordData = wordData.replace(/<\/?span[^>]*>/gi,''); 
					wordData = wordData.replace(/<span[^>]*>/gi,''); 
					wordData = wordData.replace(/<\/?div[^>]*>/gi,''); 
					//remove empty tags 
					wordData = wordData.replace(/<strong><\/strong>/gi,''); 
					wordData = wordData.replace(/<p[^>]*><\/P>/gi,''); 

					editor.document.body.innerHTML = wordData; 
				}
				}";

			toolbar1.Items.Add(myButton);

			ftbStaticHtml.Toolbars.Add(toolbar1);
		}

		private void SaveStaticHtml()
		{
			if (this._module.StaticHtmlContent == null)
				this._module.StaticHtmlContent = new StaticHtmlContent();
			// this._module.StaticHtmlContent.Title = ""
			this._module.StaticHtmlContent.Content = this.ftbStaticHtml.Text;
			IModulesDataProvider dp = ModulesDataFactory.GetInstance();
			if (this._module.StaticHtmlContent.Id == -1)
				dp.InsertStaticHtmlContent(this._module.Section.Id, Int32.Parse(Context.User.Identity.Name), this._module.StaticHtmlContent);
			else
				dp.UpdateStaticHtmlContent(this._module.Section.Id, Int32.Parse(Context.User.Identity.Name), this._module.StaticHtmlContent);
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

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveStaticHtml();
				ShowMessage("Content saved.");
			}
			catch (Exception ex)
			{
				ShowMessage(ex.Message);
			}
		}	
	}
}
