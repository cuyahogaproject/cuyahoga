using Castle.MonoRail.Framework;

namespace Cuyahoga.Web.Manager.ViewComponents
{
	public class HeaderComponent : ViewComponent
	{
		public override void Render()
		{
			RenderView("Header");
		}
	}
}
