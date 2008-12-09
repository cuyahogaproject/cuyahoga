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

		[Transaction(TransactionMode.RequiresNew)]
		public void ExtractTemplatePackage(string packageFilePath, Stream packageStream)
		{
			ICollection<string> allowedExtensions = new[] { ".ascx", ".css", ".gif", ".png", ".jpg", ".js", ".swf"};
			
			// The template dir is the name of the zip package by convention.
			string templateDir = Path.GetFileNameWithoutExtension(packageFilePath);
			string physicalTemplatesDirectory = Path.GetDirectoryName(packageFilePath);
			string physicalTargetTemplateDir = Path.Combine(physicalTemplatesDirectory, templateDir);

			if (! Directory.Exists(physicalTargetTemplateDir))
			{
				this._fileService.CreateDirectory(physicalTargetTemplateDir);
			}
			// Extract
			ZipFile templatesArchive = new ZipFile(packageStream);
			foreach (ZipEntry zipEntry in templatesArchive)
			{
				if (zipEntry.IsDirectory)
				{
					// only 'Css' and 'Images' are allowed as directory names
					if (zipEntry.Name.ToLower() != "css/" && zipEntry.Name.ToLower() != "images/")
					{
						throw new InvalidPackageException("InvalidDirectoryInPackageFoundException");
					}
					this._fileService.CreateDirectory(Path.Combine(physicalTargetTemplateDir, zipEntry.Name.Replace("/", String.Empty)));
				}
				if (zipEntry.IsFile)
				{
					string targetFilePath = Path.Combine(physicalTargetTemplateDir, zipEntry.Name);
					string extension = Path.GetExtension(targetFilePath);
					// Check allowed extensions.
					if (! allowedExtensions.Contains(extension))
					{
						throw new InvalidPackageException("InvalidExtensionFoundException");
					}
					// ascx controls should be in the root of the template dir
					if (extension == ".ascx" && ! (Path.GetDirectoryName(targetFilePath).EndsWith(templateDir)))
					{
						throw new InvalidPackageException("InvalidAscxLocationException");
					}
					// css files should be in the css subdirectory
					if (extension == ".css" && !(Path.GetDirectoryName(targetFilePath).ToLower().EndsWith(@"\css")))
					{
						throw new InvalidPackageException("InvalidCssLocationException");
					}
					this._fileService.WriteFile(targetFilePath, templatesArchive.GetInputStream(zipEntry));
				}
			}
		}

		#endregion
	}
}
