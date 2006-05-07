using System;
using System.Collections;

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
		long Id {get; set;}

		/// <summary>
		/// Global Identifier
		/// </summary>
		Guid GlobalId {get; set;}

		/// <summary>
		/// Title for display
		/// </summary>
		string Title {get; set;}

		/// <summary>
		/// Short description
		/// </summary>
		string Description {get; set;}

		/// <summary>
		/// Version information
		/// </summary>
		int Version {get; set;}

		/// <summary>
		/// For custom classes: fully qualified class name
		/// For files: extension of file
		/// </summary>
		string TypeInfo {get; set;}

		/*
		/// <summary>
		/// Generic classification for search/retrieval
		/// </summary>
		ContentItemClass Classification{get; set;}
		*/

		/// <summary>
		/// Date of creation
		/// </summary>
		DateTime CreatedAt {get; set;}

		/// <summary>
		/// Date of publication
		/// </summary>
		DateTime PublishedAt {get; set;}

		/// <summary>
		/// Date of last modification
		/// </summary>
		DateTime ModifiedAt {get; set;}

		/// <summary>
		/// Creator
		/// </summary>
		User CreatedBy {get; set;}

		/// <summary>
		/// Publisher
		/// </summary>
		User PublishedBy {get; set;}

		/// <summary>
		/// Last Modificator
		/// </summary>
		User ModifiedBy {get; set;}

		/// <summary>
		/// Parent section
		/// </summary>
		Section Section {get; set;}

		/// <summary>
		/// Assigned categories
		/// </summary>
		//IList Categories {get; set;}
	}


	/*
	/// <summary>
	/// Generic classification
	/// </summary>
	public enum ContentItemClass
	{
		CustomClass = 1,
		Document = 2,
		Image = 3,
		Video = 4,
		Audio = 5,
		RawBinary = 6
	}*/
}