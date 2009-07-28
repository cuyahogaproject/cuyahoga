using System;
using System.Collections.Generic;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// IContentItem, common denominator
	/// for all CMS content
	/// </summary>
	public interface IContentItem
	{
		/// <summary>
		/// System-wide Identifier
		/// </summary>
        long Id { get; set;}

		/// <summary>
		/// Global Identifier
		/// </summary>
        Guid GlobalId { get; set;}

        /// <summary>
        /// Workflow information
        /// </summary>
        WorkflowStatus WorkflowStatus { get; set;}

		/// <summary>
		/// Title for display
		/// </summary>
        string Title { get; set;}

		/// <summary>
		/// Short description
		/// </summary>
        string Summary { get; set;}

        /// <summary>
        /// Defines the culture info (e.g. en-US)
        /// </summary>
        string Locale { get; set;}

		/// <summary>
		/// Indicates if the content item should be syndicated.
		/// </summary>
		bool Syndicate { get; set; }

		/// <summary>
		/// Version information
		/// </summary>
        int Version { get; set;}

		/// <summary>
		/// Date of creation
		/// </summary>
        DateTime CreatedAt { get; set;}

		/// <summary>
		/// Start of publication
		/// </summary>
        DateTime? PublishedAt { get; set;}

        /// <summary>
        /// End of publication 
        /// </summary>
        DateTime? PublishedUntil { get; set;}

		/// <summary>
		/// Date of last modification
		/// </summary>
        DateTime ModifiedAt { get; set;}

		/// <summary>
		/// Creator
		/// </summary>
        User CreatedBy { get; set;}

		/// <summary>
		/// Publisher
		/// </summary>
        User PublishedBy { get; set;}

		/// <summary>
		/// Last Modificator
		/// </summary>
        User ModifiedBy { get; set;}

		/// <summary>
		/// Parent section
		/// </summary>
        Section Section { get; set;}

		/// <summary>
		/// Assigned categories
		/// </summary>
		IList<Category> Categories {get; set;}

		/// <summary>
		/// Comments
		/// </summary>
		IList<Comment> Comments { get; set; }

        /// <summary>
        /// Defines view and edit roles
        /// </summary>
        IList<ContentItemPermission> ContentItemPermissions { get; set;}

		/// <summary>
		/// Indicates if the content item is new.
		/// </summary>
		bool IsNew { get; }

		/// <summary>
		/// Gets the url that corresponds to the content. Inheritors can override this for custom url formatting.
		/// </summary>
		/// <returns></returns>
		string GetContentUrl();
	}
}