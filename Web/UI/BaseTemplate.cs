using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// This is the base class for every template usercontrol.
	/// </summary>
	public abstract class BaseTemplate: System.Web.UI.UserControl
	{
		/// <summary>
		/// Template controls that inherit from BaseTemplate must have a Literal control with id="PageTitle"
		/// </summary>
		protected System.Web.UI.WebControls.Literal PageTitle;
		/// <summary>
		/// Template controls that inherit from BaseTemplate must have a HtmlGenericControl control id="CssStyleSheet"
		/// </summary>
		protected System.Web.UI.HtmlControls.HtmlGenericControl CssStyleSheet;
		
		/// <summary>
		/// The page title as shown in the title bar of the browser
		/// </summary>
		public string Title
		{
			get	{ return this.PageTitle.Text; }
			set { this.PageTitle.Text = value; }
		}

		/// <summary>
		/// Path to external stylesheet file, relative from the application root.
		/// </summary>
		public string Css
		{
			get { return this.CssStyleSheet.Attributes["href"];	}
			set { this.CssStyleSheet.Attributes.Add("href", this.Page.ResolveUrl(value)); }
		}

		/// <summary>
		/// The form of the template.
		/// </summary>
		public Control Form
		{
			get
			{
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is HtmlForm)
						return ctrl as HtmlForm;						
				}
				return null;
			}
		}

		/// <summary>
		/// All content containers.
		/// </summary>
		public Hashtable Containers
		{
			get 
			{
				Hashtable tbl = new Hashtable();
				foreach (Control ctrl in this.Form.Controls)
				{
					if (ctrl is PlaceHolder)
					{
						tbl.Add(ctrl.ID, ctrl);
					}
				}
				return tbl;
			}
		}

		/// <summary>
		/// Insert hyperlinks in the placeholders to enable placeholder selection (for administration only).
		/// </summary>
		public void InsertContainerButtons()
		{
			string placeholderChooseControl = Context.Request.QueryString["Control"] as string;
			if (placeholderChooseControl != null)
			{
				foreach (PlaceHolder plc in this.Containers.Values)
				{
					HtmlInputButton btn = new HtmlInputButton();
					btn.Value = plc.ID;
					btn.Attributes.Add("onClick", String.Format("window.opener.setPlaceholderValue('{0}','{1}');self.close()", placeholderChooseControl, plc.ID));
					plc.Controls.Add(btn);
				}
			}
		}
	}
}
