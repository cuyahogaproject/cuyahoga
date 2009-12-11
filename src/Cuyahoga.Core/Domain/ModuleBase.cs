using System;
using System.Web;

using NHibernate;
using Castle.Core;
using Castle.Facilities.NHibernateIntegration;

using Cuyahoga.Core.Service;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// The base class for all Cuyahoga modules. 
	/// </summary>
	[Transient]
	public abstract class ModuleBase
	{
		private Section _section;
		private ISessionManager _sessionManager;
		private bool _sessionFactoryRebuilt = false;
		private string _modulePathInfo;
		private string[] _moduleParams;
		private string _sectionUrl;
		private string _displayTitle;

		/// <summary>
		/// The NHibernate session from the current ASP.NET context.
		/// </summary>
		protected ISession NHSession
		{
			get
			{
				if (this._sessionManager != null)
				{
					return this._sessionManager.OpenSession();
				}
				else
				{
					throw new NullReferenceException("Unable to obtain an NHibernate session because the session manager is null.");
				}
			}
		}

		/// <summary>
		/// The cache key used for output caching.
		/// </summary>
		public virtual string CacheKey
		{
			get
			{
				if (this._section != null)
				{
					string cacheKey = "M_" + this._section.Id.ToString();
					if (this._modulePathInfo != null)
					{
						cacheKey += "_" + this._modulePathInfo;
					}
					return cacheKey;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Flag that indicates if the SessionFactory is rebuilt. TODO: can't we handle this more elegantly?
		/// </summary>
		public bool SessionFactoryRebuilt
		{
			get { return this._sessionFactoryRebuilt; }
			set { this._sessionFactoryRebuilt = value; }
		}

		/// <summary>
		/// Property ModulePathInfo (string)
		/// </summary>
		public string ModulePathInfo
		{
			get { return this._modulePathInfo; }
			set
			{
				this._modulePathInfo = value;
				ParsePathInfo();
			}
		}

		/// <summary>
		/// Property ModuleParams (string[])
		/// </summary>
		public string[] ModuleParams
		{
			get { return this._moduleParams; }
		}

		/// <summary>
		/// Property Section (Section)
		/// </summary>
		public Section Section
		{
			get { return this._section; }
			set { this._section = value; }
		}

		/// <summary>
		/// The base url for this module. 
		/// </summary>
		public string SectionUrl
		{
			get { return this._sectionUrl; }
			set { this._sectionUrl = value; }
		}

		/// <summary>
		/// The path of default view user control from the application root.
		/// </summary>
		public string DefaultViewControlPath
		{
			get { return this._section.ModuleType.Path; }
		}

		/// <summary>
		/// Override this property when a different view should be active based on some action.
		/// </summary>
		public virtual string CurrentViewControlPath
		{
			get { return this.DefaultViewControlPath; }
		}

		/// <summary>
		/// Property DisplayTitle (string)
		/// </summary>
		public string DisplayTitle
		{
			get
			{
				if (this._displayTitle != null)
				{
					return this._displayTitle;
				}
				else if (this.Section != null)
				{
					return this.Section.Title;
				}
				else
				{
					return String.Empty;
				}
			}
			set
			{
				this._displayTitle = value;
			}
		}

		/// <summary>
		/// Sets the NHibernate session manager for the module (injected).
		/// </summary>
		public ISessionManager SessionManager
		{
			set { this._sessionManager = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ModuleBase()
		{
		}

		/// <summary>
		/// Override this method if you module needs module-specific pathinfo parsing.
		/// </summary>
		protected virtual void ParsePathInfo()
		{
			// Don't do anything special, just split the PathInfo params.
			if (this._modulePathInfo != null)
			{
				string pathInfoParamsAsString;
				if (this._modulePathInfo.StartsWith("/"))
				{
					pathInfoParamsAsString = this._modulePathInfo.Substring(1);
				}
				else
				{
					pathInfoParamsAsString = this._modulePathInfo;
				}

				this._moduleParams = pathInfoParamsAsString.Split(new char[] { '/' });
			}
		}

		/// <summary>
		/// Delete the ModuleContent in the module. Leave it up to the concrete Module how to do that.
		/// This method will likely be called before deleting a Section.
		/// </summary>
		public virtual void DeleteModuleContent()
		{
			// Do nothing here
			return;
		}

		/// <summary>
		/// Configure the module by reading the settings of the section.
		/// </summary>
		public virtual void ReadSectionSettings()
		{
			if (this._section == null)
			{
				throw new NullReferenceException("Can't access the section for settings.");
			}
		}

		/// <summary>
		/// Check if the module contains no invalid settings. Override this method in concrete module controllers.
		/// </summary>
		public virtual void ValidateSectionSettings()
		{
			// Do nothing here
			return;
		}
	}
}
