using System;
using System.Collections;

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
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property BasePath (string)
		/// </summary>
		public virtual string BasePath
		{
			get { return this._basePath; }
			set { this._basePath = value; }
		}

		/// <summary>
		/// Property TemplateControl (string)
		/// </summary>
		public virtual string TemplateControl
		{
			get { return this._templateControl; }
			set { this._templateControl = value; }
		}

		/// <summary>
		/// The path to the Template control from the application root.
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
		public virtual string Css
		{
			get { return this._css; }
			set { this._css = value; }
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
		/// Default constructor.
		/// </summary>
		public Template()
		{
			this._id = -1;
			this._sections = new Hashtable();
		}
	}
}
