using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Services.Transaction;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using NHibernate.Expression;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Default implementation of ITemplateService.
	/// </summary>
	[Transactional]
	public class TemplateService : ITemplateService
	{
		private ICommonDao _commonDao;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="commonDao"></param>
		public TemplateService(ICommonDao commonDao)
		{
			this._commonDao = commonDao;
		}

		#region ITemplateService Members

		public IList GetAllTemplates()
		{
			return this._commonDao.GetAll(typeof(Template), "Name");
		}

		public IList<Template> GetAllSystemTemplates()
		{
			DetachedCriteria crit = DetachedCriteria.For(typeof(Template))
				.Add(Expression.IsNull("Site"));
			return this._commonDao.GetAllByCriteria<Template>(crit);
		}

		public Template GetTemplateById(int templateId)
		{
			return (Template)this._commonDao.GetObjectById(typeof(Template), templateId);
		}

		public IList<Template> GetAllTemplatesBySite(Site site)
		{
			DetachedCriteria crit = DetachedCriteria.For(typeof(Template))
				.Add(Expression.Eq("Site", site))
				.AddOrder(Order.Asc("Name"));
			return this._commonDao.GetAllByCriteria<Template>(crit);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void SaveTemplate(Template template)
		{
			this._commonDao.SaveOrUpdateObject(template);
		}

		[Transaction(TransactionMode.RequiresNew)]
		public void DeleteTemplate(Template template)
		{
			this._commonDao.DeleteObject(template);
		}

		#endregion
	}
}
