using System;
using System.IO;
using System.Collections;
using System.Security.Principal;

using Castle.Services.Transaction;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.DataAccess;

namespace Cuyahoga.Core.Service.Files
{
	/// <summary>
	/// FileResourceService. 
	/// TODO: merge with TransactionalFileService.
	/// </summary>
	[Transactional]
	public class FileResourceService : IFileResourceService
	{
		private IFileResourceDao FileResourceDao;
		private IFileService fileService;

		public FileResourceService(IFileResourceDao FileResourceDao, IFileService fileService)
		{
			this.FileResourceDao = FileResourceDao;
			this.fileService = fileService;
		}

		#region IFileResourceService Members

		public bool IsDownloadAllowed(Role roleToCheck, FileResource FileResource)
		{
			foreach (Role role in FileResource.DownloadRoles)
			{
				if (role.Id == roleToCheck.Id && role.Name == roleToCheck.Name)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsDownloadAllowed(IIdentity userToCheck, FileResource FileResource)
		{
			User cuyahogaUser = userToCheck as User;
			if (cuyahogaUser == null)
			{
				return this.IsAnonymousDownloadAllowed(FileResource);
			}
			else
			{
				foreach (Role role in cuyahogaUser.Roles)
				{
					if (IsDownloadAllowed(role, FileResource))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool IsAnonymousDownloadAllowed(FileResource FileResource)
		{
			foreach (Role role in FileResource.DownloadRoles)
			{
				foreach (AccessLevel accessLevel in role.Permissions)
				{
					if (accessLevel == AccessLevel.Anonymous)
					{
						return true;
					}
				}
			}
			return false;
		}

		public IList FindFileResourcesByName(string searchString)
		{
			return this.FileResourceDao.FindFileResourcesByName(searchString);
		}

		public IList FindFileResourcesByExtension(string extension)
		{
			return this.FileResourceDao.FindFileResourcesByExtension(extension);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void SaveFileResource(FileResource FileResource, System.IO.Stream fileContent, bool appendIdBeforeExtension)
		{
			// Rename file
			if(appendIdBeforeExtension)
			{
				// Save meta info to get id
				this.FileResourceDao.SaveOrUpdateFileResource(FileResource);
				string dirName = Path.GetDirectoryName(FileResource.PhysicalPath);
				// Filename w/o extension
				string tmpFileName = FileResource.Name.Substring(0, (FileResource.Name.Length - FileResource.TypeInfo.Length));
				tmpFileName = string.Concat(tmpFileName, ".", FileResource.Id.ToString(), FileResource.TypeInfo);
				FileResource.Name = tmpFileName;
				FileResource.PhysicalPath = Path.Combine(dirName, tmpFileName);
			}
			// Save physical file.
			this.fileService.WriteFile(FileResource.PhysicalPath, fileContent);
			// Save meta info
			this.FileResourceDao.SaveOrUpdateFileResource(FileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void UpdateFileResource(FileResource FileResource)
		{
			this.FileResourceDao.SaveOrUpdateFileResource(FileResource);
		}

		[Transaction(TransactionMode.Requires)]
		public virtual void DeleteFileResource(FileResource FileResource)
		{
			// Delete physical file.
			this.fileService.DeleteFile(FileResource.PhysicalPath);
			// Delete meta info.
			this.FileResourceDao.DeleteFileResource(FileResource);
		}

	

		#endregion
	}
}