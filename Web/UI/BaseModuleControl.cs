using System;
using System.Web;
using System.IO;
using System.Globalization;
using System.Resources;
using System.Reflection;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Base class for all module user controls.
	/// Credits to the DotNetNuke team (http://www.dotnetnuke.com) for the output caching solution.
	/// </summary>
	public class BaseModuleControl : System.Web.UI.UserControl
	{
		private ModuleBase _module;
		private string _cachedOutput;
		private ResourceManager _resMan;

		/// <summary>
		/// Indicator if there is cached content. The derived ModuleControls should determine whether to
		/// load content or not.
		/// </summary>
		protected bool HasCachedOutput
		{
			get { return this._cachedOutput != null; }
		}

		/// <summary>
		/// The resource manager for the module.
		/// </summary>
		protected ResourceManager ResMan
		{
			get { return this._resMan; }
		}

		/// <summary>
		/// The accompanying ModuleBase business object. Use this property  to access
		/// module properties, sections and nodes from the code-behind of the module user controls.
		/// </summary>
		public ModuleBase Module
		{
			get { return this._module; }
			set { this._module = value; }
		}

		public BaseModuleControl()
		{
			// Base name of the resources consists of Namespace.Resources.Strings
			string baseName = this.GetType().BaseType.Namespace + ".Resources.Strings";
			this._resMan = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.Module.Section.CacheDuration > 0 && this.Module.CacheKey != null)
			{
				if (HttpContext.Current.Cache[this.Module.CacheKey] != null)
				{
					// Found cached content.
					this._cachedOutput = HttpContext.Current.Cache[this.Module.CacheKey].ToString();
				}
			}
			base.OnInit(e);
		}


		/// <summary>
		/// Wrap the section content in a visual block.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			writer.Write("<div class=\"section\">");
			// Section title
			if (this._module.Section != null && this._module.Section.ShowTitle)
			{
				writer.Write("<h3>" + this._module.Section.Title + "</h3>");
			}
			// Edit button
			User cuyahogaUser = this.Page.User.Identity as User;
			if (this._module.Section.ModuleType.EditPath != null
				&& cuyahogaUser != null
				&& cuyahogaUser.CanEdit(this._module.Section))
			{
				writer.Write(String.Format("<a href=\"{0}?NodeId={1}&SectionId={2}\">Edit</a>", UrlHelper.GetApplicationPath() + this._module.Section.ModuleType.EditPath, this._module.Section.Node.Id, this._module.Section.Id));
			}
			// Write module content and handle caching when neccesary.
			if (this._module.Section.CacheDuration > 0 && this.Module.CacheKey != null)
			{
				if (this._cachedOutput == null)
				{
					StringWriter tempWriter = new StringWriter();
					base.Render(new System.Web.UI.HtmlTextWriter(tempWriter));
					this._cachedOutput = tempWriter.ToString();
					HttpContext.Current.Cache.Insert(this.Module.CacheKey, this._cachedOutput, null
						, DateTime.Now.AddSeconds(this._module.Section.CacheDuration), TimeSpan.Zero);
				}
				// Output the user control's content.
                writer.Write(_cachedOutput);
			}
			else
			{
				base.Render (writer);
			}
			writer.Write("</div>");
		}

	}
}
