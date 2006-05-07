using System;
using System.Collections;

using Castle.Facilities.NHibernateIntegration;

using NHibernate;
using NHibernate.Expression;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.DataAccess
{
	/// <summary>
	/// ContentItemDao.
	/// </summary>
	public class ContentItemDao: IContentItemDao
	{
		private ISessionManager sessionManager;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sessionManager"></param>
		public ContentItemDao(ISessionManager sessionManager)
		{
			this.sessionManager = sessionManager;
		}

		#region Methods

		/// <summary>
		/// Based on filter get sql (=, &lt;, &gt;)
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		protected virtual string DetermineSqlOperator(ContentItemDateFilter filter)
		{
			switch(filter)
			{
				case ContentItemDateFilter.Exact: return "=";
				case ContentItemDateFilter.Prior: return "<";
				case ContentItemDateFilter.After: return ">";
				default: return "=";
			}
		}

		protected virtual IList FindContentItemByDate(DateTime date, Cuyahoga.Core.DataAccess.ContentItemDateFilter filter, string propertyName)
		{
			string sqlOp = this.DetermineSqlOperator(filter);
		
			ISession session = this.sessionManager.OpenSession();

			string hql = string.Format("from IContentItem ci where ci.{0} {1} :date ", propertyName, sqlOp);
			IQuery query = session.CreateQuery(hql);
			query.SetDateTime("date", date);
			return query.List();
		}

		protected virtual IList FindContentItemByDateRange(DateTime fromDate, DateTime toDate, string propertyName)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = string.Format("from IContentItem ci where ci.{0} >= :fromDate and ci.{0} <= :toDate ", propertyName);
			IQuery query = session.CreateQuery(hql);
			query.SetDateTime("fromDate", fromDate);
			query.SetDateTime("toDate", toDate);
			return query.List();
		}

		protected virtual IList FindContentItemByUser(User user, string propertyName)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = string.Format("from IContentItem ci where ci.{0}.Id = :userId ", propertyName);
			IQuery query = session.CreateQuery(hql);
			query.SetInt32("userId", user.Id);
			return query.List();

		}

		#endregion


		#region IContentItemDao Members

		public IList FindContentItemsByTitle(string searchString)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = "from IContentItem ci where ci.Title like ? order by ci.Title";
			return session.Find(hql, searchString + "%", NHibernateUtil.String);
		}

		public IList FindContentItemsByDescription(string searchString)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = "from IContentItem ci where ci.Description like ? order by ci.Title";
			return session.Find(hql, searchString + "%", NHibernateUtil.String);
		}

		public IList FindContentItemsByCreator(User user)
		{
			return this.FindContentItemByUser(user, "CreatedBy");
		}

		public IList FindContentItemsByPublisher(User user)
		{
			return this.FindContentItemByUser(user, "PublishedBy");
		}

		public IList FindContentItemsByModifier(User user)
		{
			return this.FindContentItemByUser(user, "ModifiedBy");
		}

		public IList FindContentItemsBySection(Section section)
		{
			ISession session = this.sessionManager.OpenSession();

			string hql = "from IContentItem ci where ci.Section.Id = :sectionId";
			IQuery query = session.CreateQuery(hql);
			query.SetInt32("sectionId", section.Id);
			return query.List();
		}

		public IList FindContentItemsByCreationDate(DateTime date, ContentItemDateFilter filter)
		{
			return this.FindContentItemByDate(date, filter, "CreatedAt");
		}

		public IList FindContentItemsByCreationDate(DateTime fromDate, DateTime toDate)
		{
			return this.FindContentItemByDateRange(fromDate, toDate, "CreatedAt");
		}

		public IList FindContentItemsByPublicationDate(DateTime date, ContentItemDateFilter filter)
		{
			return this.FindContentItemByDate(date, filter, "PublishedAt");
		}

		public IList FindContentItemsByPublicationDate(DateTime fromDate, DateTime toDate)
		{
			return this.FindContentItemByDateRange(fromDate, toDate, "PublishedAt");
		}

		public IList FindContentItemsByModificationDate(DateTime date, Cuyahoga.Core.DataAccess.ContentItemDateFilter filter)
		{
			return this.FindContentItemByDate(date, filter, "ModifiedAt");
		}

		public IList FindContentItemsByModificationDate(DateTime fromDate, DateTime toDate)
		{
			return this.FindContentItemByDateRange(fromDate, toDate, "ModifiedAt");
		}

		#endregion
	}
}