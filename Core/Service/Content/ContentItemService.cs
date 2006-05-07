using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// ContentItemService, service facade for content
	/// </summary>
	public class ContentItemService : IContentItemService
				
	{
		private IContentItemDao contentItemDao;
			
		public ContentItemService(IContentItemDao contentItemDao)
								
		{
			this.contentItemDao = contentItemDao;
		}
	 
		#region IContentItemService Member
	 
		public IList FindContentItemsByTitle(string searchString)
	 		
		{
			return this.contentItemDao.FindContentItemsByTitle(searchString);
		}
	 
		public IList FindContentItemsByDescription(string searchString)
	 		
		{
			return this.contentItemDao.FindContentItemsByDescription(searchString);
		}
	 
		public IList FindContentItemsByCreator(User user)
	 		
		{
			return this.contentItemDao.FindContentItemsByCreator(user);
		}
	 
		public IList FindContentItemsByPublisher(User user)
	 		
		{
			return this.contentItemDao.FindContentItemsByPublisher(user);
		}
	 
		public IList FindContentItemsByModifier(User user)
	 		
		{
			return this.contentItemDao.FindContentItemsByModifier(user);
		}
	 
		public IList FindContentItemsBySection(Section section)
	 		
		{
			return this.contentItemDao.FindContentItemsBySection(section);
		}
	 
		public IList FindContentItemsByCreationDate(DateTime date, ContentItemDateFilter filter)
	 		
		{
			return this.contentItemDao.FindContentItemsByCreationDate(date, filter);
		}
	 
		public IList FindContentItemsByCreationDate(DateTime fromDate, DateTime toDate)
	 		
		{
			return this.contentItemDao.FindContentItemsByCreationDate(fromDate, toDate);
		}
	 	
		public IList FindContentItemsByPublicationDate(DateTime date, ContentItemDateFilter filter)
	 		
		{
			return this.contentItemDao.FindContentItemsByPublicationDate(date, filter);
		}
	 
		public IList FindContentItemsByPublicationDate(DateTime fromDate, DateTime toDate)
	 		
		{
			return this.contentItemDao.FindContentItemsByPublicationDate(fromDate, toDate);
		}
	 
		public IList FindContentItemsByModificationDate(DateTime date, ContentItemDateFilter filter)
	 		
		{
			return this.contentItemDao.FindContentItemsByModificationDate(date, filter);
		}
	 
		public IList FindContentItemsByModificationDate(DateTime fromDate, DateTime toDate)
	 		
		{
			return this.contentItemDao.FindContentItemsByModificationDate(fromDate, toDate);
		}
	 
		#endregion
	}
}