using System;
using System.Collections;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Summary description for Site.
	/// </summary>
	public class Site
	{
		private int _id;
		private string _name;
		private string _siteUrl;
		private string _defaultCulture;
		private Template _defaultTemplate;
		private string _defaultPlaceholder;
		private string _webmasterEmail;
		private bool _useFriendlyUrls;
		private string _metaKeywords;
		private string _metaDescription;
		private Role _defaultRole;
		private IList _rootNodes;
		private DateTime _updateTimestamp;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int Id
		{
			get { return this._id; }
			set { this._id = value; }
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
		/// Property HomeUrl (string)
		/// </summary>
		public virtual string SiteUrl
		{
			get { return this._siteUrl; }
			set { this._siteUrl = value; }
		}

		/// <summary>
		/// Property DefaultCulture (string)
		/// </summary>
		public virtual string DefaultCulture
		{
			get { return this._defaultCulture; }
			set { this._defaultCulture = value; }
		}

		/// <summary>
		/// Property DefaultTemplate (Template)
		/// </summary>
		public virtual Template DefaultTemplate
		{
			get { return this._defaultTemplate; }
			set { this._defaultTemplate = value; }
		}

		/// <summary>
		/// The default role for registered users.
		/// </summary>
		public virtual Role DefaultRole
		{
			get { return this._defaultRole; }
			set { this._defaultRole = value; }
		}

		/// <summary>
		/// Property DefaultPlaceholder (string)
		/// </summary>
		public virtual string DefaultPlaceholder
		{
			get { return this._defaultPlaceholder; }
			set { this._defaultPlaceholder = value; }
		}

		/// <summary>
		/// Property WebmasterEmail (string)
		/// </summary>
		public virtual string WebmasterEmail
		{
			get { return this._webmasterEmail; }
			set { this._webmasterEmail = value; }
		}

		/// <summary>
		/// Indicates if the site uses friendly 'readable' urls by default.
		/// </summary>
		public virtual bool UseFriendlyUrls
		{
			get { return this._useFriendlyUrls; }
			set { this._useFriendlyUrls = value; }
		}

		/// <summary>
		/// List of global keywords for the site.
		/// </summary>
		public virtual string MetaKeywords
		{
			get { return this._metaKeywords; }
			set { this._metaKeywords = value; }
		}

		/// <summary>
		/// Global description for the site.
		/// </summary>
		public virtual string MetaDescription
		{
			get { return this._metaDescription; }
			set { this._metaDescription = value; }
		}

		/// <summary>
		/// Property RootNodes (IList)
		/// </summary>
		public virtual IList RootNodes
		{
			get { return this._rootNodes; }
			set { this._rootNodes = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public virtual DateTime UpdateTimestamp
		{
			get { return this._updateTimestamp; }
			set { this._updateTimestamp = value; }
		}

		#endregion

		public Site()
		{
			this._id = -1;
		}
	}
}
