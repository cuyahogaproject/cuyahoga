using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.WebForms;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Controllers
{
	public class TemplatesController : SecureController
	{
		private readonly ITemplateService _templateService;

		public TemplatesController(ITemplateService templateService)
		{
			_templateService = templateService;
		}

		[PermissionFilter(RequiredRights = Rights.ManageTemplates)]
		public ActionResult Index()
		{
			ViewData["Title"] = GlobalResources.ManageTemplatesPageTitle;
			return View();
		}

		public ActionResult GetPlaceholdersByTemplateId(int templateId)
		{
			Template template = this._templateService.GetTemplateById(templateId);
			string virtualTemplatePath = VirtualPathUtility.Combine(CuyahogaContext.CurrentSite.SiteDataDirectory, template.Path);
			var placeholders = ViewUtil.GetPlaceholdersFromVirtualPath(virtualTemplatePath);
			var jsonValues = from key in placeholders.Keys
							 select new
							 {
								 Placeholder = key
							 };
			return Json(jsonValues);
		}
	}
}