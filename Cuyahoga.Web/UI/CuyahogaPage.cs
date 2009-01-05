using System;

using Cuyahoga.Core.Service;
using Castle.Windsor;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Base class for all aspx pages in Cuyahoga (public and admin).
	/// </summary>
	public class CuyahogaPage : System.Web.UI.Page, ICuyahogaPage
	{
		private IWindsorContainer _container;

		/// <summary>
		/// A reference to the Windsor container that can be used as a Service Locator for service classes.
		/// </summary>
		protected IWindsorContainer Container
		{
			get { return this._container; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public CuyahogaPage()
		{
			this._container = ContainerAccessorUtil.GetContainer();
		}
	}
}
