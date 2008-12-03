using System;
using System.Collections;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using NHibernate.Expression;

namespace Cuyahoga.Core.Service.SiteStructure
{
	/// <summary>
	/// Default implementation of ITemplateService.
	/// </summary>
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

		#endregion
	}
}
