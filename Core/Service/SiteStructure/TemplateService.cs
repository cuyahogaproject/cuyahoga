using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Castle.Services.Transaction;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service.Files;
using ICSharpCode.SharpZipLib.Zip;
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
		private IFileService _fileService;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="commonDao"></param>
		/// <param name="fileService"></param>
		public TemplateService(ICommonDao commonDao, IFileService fileService)
		{
			this._commonDao = commonDao;
			_fileService = fileService;
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

		public void ExtractTemplatePackage(string packageFilePath, Stream packageStream)
		{
			// TODO: much more checks of the archive

			// The template dir is the name of the zip package by convention.
			string templateDir = Path.GetFileNameWithoutExtension(packageFilePath);
			// We're assuming that the package will be saved in Templates directory of the site.
			string physicalTemplatesDirectory = Path.GetDirectoryName(packageFilePath);
			this._fileService.WriteFile(packageFilePath, packageStream);

			// Extract
			FastZip fastZipper = new FastZip();
			fastZipper.ExtractZip(packageFilePath, Path.Combine(physicalTemplatesDirectory, templateDir), String.Empty);

			// Delete package file.
			this._fileService.DeleteFile(packageFilePath);
		}

		#endregion
	}
}
