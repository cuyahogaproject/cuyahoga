using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Base class for all modules. 
	/// </summary>
	public abstract class Module
	{
		private int _moduleId;
		private string _name;
		private string _assemblyName;
		private string _className;
		private string _path;
		private string _editPath;
		private string[] _moduleParams;
		private Section _section;

		#region properties
		
		/// <summary>
		/// Property ModuleId (int)
		/// </summary>
		public int ModuleId
		{
			get { return this._moduleId; }
			set { this._moduleId = value; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public virtual Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// Property AssemblyName (string)
		/// </summary>
		public string AssemblyName
		{
			get { return this._assemblyName; }
			set { this._assemblyName = value; }
		}

		/// <summary>
		/// Property ClassName (string)
		/// </summary>
		public string ClassName
		{
			get { return this._className; }
			set { this._className = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Path (string)
		/// </summary>
		public string Path
		{
			get { return this._path; }
			set { this._path = value; }
		}

		/// <summary>
		/// Property EditPath (string)
		/// </summary>
		public string EditPath
		{
			get { return this._editPath; }
			set { this._editPath = value; }
		}

		/// <summary>
		/// Property ModuleParams (string[])
		/// </summary>
		public string[] ModuleParams
		{
			get { return this._moduleParams; }
			set { this._moduleParams = value; }
		}

		#endregion

		/// <summary>
		/// Load the module content. The concrete module has to decide how to implement this
		/// (e.g. some modules would require that the Section is known).
		/// </summary>
		public abstract void LoadContent();

		/// <summary>
		/// Delete the module content. The concrete module has to decide how to delete the content 
		/// or whether deleting of content is allowed.
		/// </summary>
		public abstract void DeleteContent();
	}
}
