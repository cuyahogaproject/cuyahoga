using System.Web.Mvc;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Manager.Controllers;

namespace Cuyahoga.Web.Manager.Controllers
{
	[Authorize]
	public abstract class SecureController : BaseController
	{
		public SecureController()
		{}

		public SecureController(IModelValidator modelValidator) : base(modelValidator)
		{}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			// Add Cuyahoga user to the ViewData, so the views don't have to call dirty static 
			// things like CuyahogaContext.Current
			if (filterContext.HttpContext.User is User)
			{
				ViewData["CuyahogaUser"] = filterContext.HttpContext.User;
			}
			base.OnAuthorization(filterContext);
		}
	}
}