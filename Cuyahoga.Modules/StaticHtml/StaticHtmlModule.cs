using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Web.Mvc;
using Cuyahoga.Web.Mvc.Areas;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// The StaticHtmlModule provides the content of simple static page. It needs at least its 
	/// Section to be set to do something with the content (load, update, delete).
	/// </summary>
	public class StaticHtmlModule : ModuleBase, INHibernateModule, IMvcModule
	{
		private readonly IContentItemService<StaticHtmlContent> _contentItemService;

		public StaticHtmlModule(IContentItemService<StaticHtmlContent> contentItemService)
		{
			_contentItemService = contentItemService;
		}

		/// <summary>
		/// Get the content that belongs to the Section where the module is attached to.
		/// </summary>
		/// <returns></returns>
		public StaticHtmlContent GetContent()
		{
			if (base.Section != null)
			{
				return this._contentItemService.FindContentItemsBySection(base.Section).FirstOrDefault();
			}
			else
			{
				throw new Exception("Unable to get the StaticHtmlContent when no Section is available");
			}
		}

		/// <summary>
		/// Save the content.
		/// </summary>
		/// <param name="content"></param>
		public void SaveContent(StaticHtmlContent content)
		{
			this._contentItemService.Save(content);
		}

		/// <summary>
		/// Delete the content.
		/// </summary>
		/// <param name="content"></param>
		public void DeleteContent(StaticHtmlContent content)
		{
			this._contentItemService.Delete(content);
		}

		public override void DeleteModuleContent()
		{
			// Delete the associated StaticHtmlContent
			StaticHtmlContent content = this.GetContent();
			if (content != null)
			{
				DeleteContent(content);
			}
		}

		public void RegisterRoutes(RouteCollection routes)
		{
			routes.CreateArea("Modules/StaticHtml", "Cuyahoga.Modules.StaticHtml.Controllers",
				routes.MapRoute("StaticHtmlRoute", "Modules/StaticHtml/{controller}/{action}/{id}", new { action = "Edit", controller = "ManageContent", id = "" })
			);
		}
	}
}
