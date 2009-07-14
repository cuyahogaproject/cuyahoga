using System;
using System.Collections;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a Cuyahoga template. This is not restricted to one physical file.
	/// It's possible to create multiple template objects based on the same template
	/// UserControl and stylesheet.
	/// </summary>
	public class Template
	{
		private int _id;
		private DateTime _updateTimestamp;
		private string _name;
		private string _basePath;
		private string _templateControl;
		private string _css;
		private Site _site;
		private IDictionary _sections;

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		[ValidateNonEmpty("TemplateNameValidatorNonEmpty")]
		[ValidateLength(1, 100, "TemplateNameValidatorLength")]
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property BasePath (string)
		/// </summary>
		[ValidateNonEmpty]
		[ValidateLength(1, 100)]
		public virtual string BasePath
		{
			get { return this._basePath; }
			set { this._basePath = value; }
		}

		/// <summary>
		/// Property TemplateControl (string)
		/// </summary>
		[ValidateNonEmpty]
		[ValidateLength(1, 50)]
		public virtual string TemplateControl
		{
			get { return this._templateControl; }
			set { this._templateControl = value; }
		}

		/// <summary>
		/// BasePath and Template control combined.
		/// </summary>
		/// <remarks>
		/// This is a combination of BasePath and TemplateControl. This is the same as pre-0.7 Path property.
		/// </remarks>
		public virtual string Path
		{
			get { return this.BasePath + "/" + this.TemplateControl; }
		}


		/// <summary>
		/// The filename of the stylesheet file to be used with this Template.
		/// </summary>
		/// <remarks>
		/// The stylesheet file has to be in the [BasePath]/Css directory.
		/// </remarks>
		[ValidateNonEmpty]
		[ValidateLength(1, 100)]
		public virtual string Css
		{
			get { return this._css; }
			set { this._css = value; }
		}

		/// <summary>
		/// The site where this template belongs to.
		/// </summary>
		public virtual Site Site
		{
			get { return _site; }
			set { _site = value; }
		}

		/// <summary>
		/// The sections that are directly related to the template. The key represents the placeholder
		/// where the section belongs.
		/// </summary>
		public virtual IDictionary Sections
		{
			get { return this._sections; }
			set { this._sections = value; }
		}

		/// <summary>
		/// Gets the full css path from the sitedata root for the css styles that are shown in the html editor.
		/// By convention, this file is in the /Css/ subdirectory with the name '
		/// </summary>
		public virtual string EditorCss
		{
			get { return this.BasePath + "/Css/" + "editor.css"; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Template()
		{
			this._id = -1;
			this._sections = new Hashtable();
		}

		/// <summary>
		/// Get a clean copy of this template.
		/// </summary>
		/// <returns></returns>
		public virtual Template GetCopy()
		{
			Template newTemplate = new Template();
			newTemplate.Name = this.Name;
			newTemplate.BasePath = this.BasePath;
			newTemplate.TemplateControl = this.TemplateControl;
			newTemplate.Css = this.Css;

			return newTemplate;
		}
	}
}
