using System;

namespace Cuyahoga.Core.Domain
{
    /// <summary>
    /// Association class between ContentItem and Role.
    /// </summary>
    public class ContentItemPermission : Permission
    {
        private ContentItem contentItem;

        /// <summary>
        /// Property ContentItem (ContentItem)
        /// </summary>
        public ContentItem ContentItem
        {
            get { return this.contentItem; }
            set { this.contentItem = value; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContentItemPermission()
            : base()
        {
        }
    }
}
