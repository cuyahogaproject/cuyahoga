using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content 
{
	/// <summary>
	/// 
	/// </summary>
	public interface IContentItemService
				
	{
		/// <summary>
		/// Find ContentItems by title (begins with).
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		IList FindContentItemsByTitle(string searchString);
			
		/// <summary>
		/// Find ContentItems by description (begins with).
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		IList FindContentItemsByDescription(string searchString);
			
		/// <summary>
		/// Find ContentItems by the user who created them
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IList FindContentItemsByCreator(User user);
			
		/// <summary>
		/// Find ContentItems by the user who published them
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IList FindContentItemsByPublisher(User user);
			
		/// <summary>
		/// Find ContentItems by the user who last modified them 
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IList FindContentItemsByModifier(User user);
			
		/// <summary>
		/// Find ContentItems by the section they are contained in
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		IList FindContentItemsBySection(Section section);
			
			
		/// <summary>
		/// Find ContentItems by their creation date
		/// </summary>
		/// <param name="date"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		IList FindContentItemsByCreationDate(DateTime date, ContentItemDateFilter filter);
			
		/// <summary>
		/// Find ContentItems by a creation date range
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		IList FindContentItemsByCreationDate(DateTime fromDate, DateTime toDate);
			
		/// <summary>
		/// Find ContentItems by their publication date
		/// </summary>
		/// <param name="date"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		IList FindContentItemsByPublicationDate(DateTime date, ContentItemDateFilter filter);
			
		/// <summary>
		/// Find ContentItems by a publication date range
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		IList FindContentItemsByPublicationDate(DateTime fromDate, DateTime toDate);
			
		/// <summary>
		/// Find ContentItems by their modification date
		/// </summary>
		/// <param name="date"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		IList FindContentItemsByModificationDate(DateTime date, ContentItemDateFilter filter);
			
		/// <summary>
		/// Find ContentItems by a modification date range
		/// </summary>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		IList FindContentItemsByModificationDate(DateTime fromDate, DateTime toDate);
	}
}