using Castle.MonoRail.Framework;
using Cuyahoga.Core.Domain;
using Cuyahoga.Web.Components;

namespace Cuyahoga.Web.Manager.ViewComponents
{
	/// <summary>
	/// Viewcomponent that allows to show or hide content based on a Right that the current 
	/// user has (via the Roles). Based on the Castle SecurityComponent.
	/// </summary>
	public class RightsComponent : ViewComponent
	{
		private bool _shouldRender;
		private ICuyahogaContext _cuyahogaContext;

		public RightsComponent(ICuyahogaContext cuyahogaContext)
		{
			this._cuyahogaContext = cuyahogaContext;
		}

		/// <summary>
		/// Called by the framework once the component instance
		/// is initialized
		/// </summary>
		public override void Initialize()
		{
			string right = (string)ComponentParams["right"];
			string rights = (string)ComponentParams["rights"];

			if (right == null && rights == null)
			{
				throw new RailsException("RightsComponent: you must supply a right (or rights) parameter");
			}

			_shouldRender = true;

			User currentUser = this._cuyahogaContext.CurrentUser;
			if (currentUser == null)
			{
				_shouldRender = false;
			}
			else
			{
				if (right != null)
				{
					_shouldRender = currentUser.HasRight(right);
				}
				else
				{
					foreach (string rightFromRights in rights.Split(','))
					{
						if (! currentUser.HasRight(rightFromRights.Trim()))
						{
							_shouldRender = false;
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Called by the framework so the component can
		/// render its content
		/// </summary>
		public override void Render()
		{
			if (_shouldRender)
			{
				Context.RenderBody();
			}
		}
	}
}
