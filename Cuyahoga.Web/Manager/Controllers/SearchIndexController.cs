using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Mvc.Filters;

namespace Cuyahoga.Web.Manager.Controllers
{
	[PermissionFilter(RequiredRights = Rights.ManageSite)]
	public class SearchIndexController : ManagerController
	{
		private readonly ISearchService _searchService;
		private readonly ModuleLoader _moduleLoader;

		public SearchIndexController(ISearchService searchService, ModuleLoader moduleLoader)
		{
			_searchService = searchService;
			_moduleLoader = moduleLoader;
		}

		public ActionResult Index()
		{
			SearchIndexProperties indexProperties = new SearchIndexProperties { IndexDirectory = "Unknown" };
			try
			{
				indexProperties = this._searchService.GetIndexProperties();
			}
			catch (Exception ex)
			{
				Messages.AddException(ex);
			}
			return View(indexProperties);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult RebuildIndex()
		{
			try
			{
				IEnumerable<ISearchable> legacySearchableModules = GetLegacySearchModulesForCurrentSite();
				this._searchService.RebuildIndex(legacySearchableModules);
				Messages.AddFlashMessage("FullTextIndexRebuildMessage");
			}
			catch (Exception ex)
			{
				Messages.AddFlashException(ex);
			}
			return RedirectToAction("Index");
		}

		private IEnumerable<ISearchable> GetLegacySearchModulesForCurrentSite()
		{
			IList<ISearchable> searchableModules = new List<ISearchable>();
			foreach (Node rootNode in this.CuyahogaContext.CurrentSite.RootNodes)
			{
				AddSearchableModuleForNode(rootNode, searchableModules);
			}
			return searchableModules;
		}

		private void AddSearchableModuleForNode(Node node, IList<ISearchable> searchableModules)
		{
			foreach (Section section in node.Sections)
			{
				ISearchable module = this._moduleLoader.GetModuleFromSection(section) as ISearchable;

				if (module != null)
				{
					searchableModules.Add(module);
				}
			}
			foreach (Node childNode in node.ChildNodes)
			{
				AddSearchableModuleForNode(childNode, searchableModules);
			}
		}
	}
}
