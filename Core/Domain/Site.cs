using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a Site instance.
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
		private IList<Role> _roles;
		private IList<Template> _templates;
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
		[ValidateNonEmpty("SiteNameValidatorNonEmpty")]
		[ValidateLength(1, 100, "SiteNameValidatorLength")]
		public virtual string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property SiteUrl (string)
		/// </summary>
		[ValidateNonEmpty("SiteUrlValidatorNonEmpty")]
		[ValidateLength(12, 100, "SiteUrlValidatorLength")]
		[ValidateRegExp(@"^http(s)*://(\w[.\w]*)(:\d+)*(/\w[.\w]*)*", "SiteUrlValidatorPattern")]
		public virtual string SiteUrl
		{
			get { return this._siteUrl; }
			set { this._siteUrl = value; }
		}

		/// <summary>
		/// Property DefaultCulture (string)
		/// </summary>
		[ValidateNonEmpty("DefaultCultureValidatorNonEmpty")]
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
		[ValidateNonEmpty("DefaultRoleValidatorNonEmpty")]
		public virtual Role DefaultRole
		{
			get { return this._defaultRole; }
			set { this._defaultRole = value; }
		}

		/// <summary>
		/// Property DefaultPlaceholder (string)
		/// </summary>
		[ValidateLength(0, 100, "PlaceholderValidatorLength")]
		public virtual string DefaultPlaceholder
		{
			get { return this._defaultPlaceholder; }
			set { this._defaultPlaceholder = value; }
		}

		/// <summary>
		/// Property WebmasterEmail (string)
		/// </summary>
		[ValidateNonEmpty("EmailValidatorNonEmpty")]
		[ValidateLength(1, 100, "EmailValidatorLength")]
		[ValidateEmail("EmailValidatorEmail")]
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
		[ValidateLength(0, 500, "MetaKeywordsValidatorLength")]
		public virtual string MetaKeywords
		{
			get { return this._metaKeywords; }
			set { this._metaKeywords = value; }
		}

		/// <summary>
		/// Global description for the site.
		/// </summary>
		[ValidateLength(0, 500, "MetaDescriptionValidatorLength")]
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
		/// The roles that are associated with this site.
		/// </summary>
		public virtual IList<Role> Roles
		{
			get { return _roles; }
			set { _roles = value; }
		}

		/// <summary>
		/// The templates that are related to the site.
		/// </summary>
		public virtual IList<Template> Templates
		{
			get { return _templates; }
			set { _templates = value; }
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
		/// The virtual path of the site data directory (starting with ~/ and ending with /).
		/// </summary>
		public virtual string SiteDataDirectory
		{
			get
			{
				if (this._id <= 0)
				{
					throw new InvalidOperationException("Unable to get the site data directory when the site isn't saved yet.");
				}
				return string.Format("~/SiteData/{0}/ ", this._id);
			}
		}

		#endregion

		/// <summary>
		/// Creates a new instance of the <see cref="Site"></see> class.
		/// </summary>
		public Site()
		{
			this._id = -1;
			this._useFriendlyUrls = true;
			this._rootNodes = new ArrayList();
			this._roles = new List<Role>();
			this._templates = new List<Template>();
		}
	}
}
