using System;
using System.Web;

using Cuyahoga.Core;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Base class for all module user controls
	/// </summary>
	public class BaseModuleControl : System.Web.UI.UserControl
	{
		private Module _module;

		/// <summary>
		/// The accompanying Module business object. Use this property  to access
		/// module properties, sections and nodes from the code-behind of the module user controls.
		/// </summary>
		public Module Module
		{
			get { return this._module; }
			set { this._module = value; }
		}

		public BaseModuleControl()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Wrap the module content in a visual block.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<div class=\"module\">");
			User cuyahogaUser = this.Page.User.Identity as User;
			if (this._module.EditPath.Length > 0 
				&& cuyahogaUser != null
				&& cuyahogaUser.CanEdit(this._module.Section))
			{
				writer.Write(String.Format("<a href=\"{0}?NodeId={1}&SectionId={2}\">Edit</a>", UrlHelper.GetApplicationPath() + Module.EditPath, Module.Section.Node.Id, Module.Section.Id));
			}
			if (this._module.Section != null && this._module.Section.ShowTitle)
			{
				writer.Write("<h3>" + this._module.Section.Title + "</h3>");
			}
			base.Render (writer);
			writer.Write("</div>");
		}

	}
}
