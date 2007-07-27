using System;
using System.Collections.Generic;
using System.Text;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
    /// Describes a ContentType <see>IContentItem</see> that 
    /// a module is able to manage
	/// </summary>
	public class ModuleContentType
	{
        private string contentType;
        private bool isVisible;
        private bool isTypeNameLocalized;
		private ModuleType moduleType;
		
		/// <summary>
		/// The full type name
		/// </summary>
		public string ContentType
		{
            get { return this.contentType; }
            set { this.contentType = value; }
		}
        /// <summary>
        /// Define if the type should be visible in
        /// automatic generated menus or lists
        /// </summary>
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
        }

        /// <summary>
        /// Set to true if the module's resource file
        /// provides a localized name (uses ContenType as key)
        /// </summary>
        public bool IsTypeNameLocalized
        {
            get { return this.isTypeNameLocalized; }
            set { this.isTypeNameLocalized = value; }
        }

		/// <summary>
		/// The ModuleType
		/// </summary>
		public ModuleType ModuleType
		{
			get { return this.moduleType; }
			set { this.moduleType = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ModuleContentType()
		{
		}
	}
}
