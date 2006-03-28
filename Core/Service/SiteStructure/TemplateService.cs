using System;
using System.Collections;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

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

		public Template GetTemplateById(int templateId)
		{
			return (Template)this._commonDao.GetObjectById(typeof(Template), templateId);
		}

		#endregion
	}
}
