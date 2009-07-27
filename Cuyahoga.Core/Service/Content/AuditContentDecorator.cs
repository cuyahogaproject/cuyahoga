using System;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// Decorator for IContentItemService that adds auditing (created and modified by etc.) to content items.
	/// </summary>
	/// <remarks>
	/// We might have to merge this one with the versioningdecorator.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class AuditContentDecorator<T> : AbstractContentItemServiceDecorator<T> where T : IContentItem
	{
		private readonly ICuyahogaContextProvider _cuyahogaContextProvider;

		public AuditContentDecorator(IContentItemService<T> contentItemService, ICuyahogaContextProvider cuyahogaContextProvider) : base(contentItemService)
		{
			this._cuyahogaContextProvider = cuyahogaContextProvider;
		}

		/// <summary>
		/// Add 'who did what and when' while saving the content item to the database.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public override T Save(T entity)
		{
			ICuyahogaContext cuyahogaContext = this._cuyahogaContextProvider.GetContext();
			if (entity.IsNew)
			{
				entity.CreatedAt = DateTime.Now;
				entity.CreatedBy = cuyahogaContext.CurrentUser;
			}
			entity.ModifiedAt = DateTime.Now;
			entity.ModifiedBy = cuyahogaContext.CurrentUser;
			return base.Save(entity);
		}
	}
}
