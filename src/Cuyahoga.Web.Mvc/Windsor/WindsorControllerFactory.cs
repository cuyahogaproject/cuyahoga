using System;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;

namespace Cuyahoga.Web.Mvc.Windsor
{
	/// <summary>
	/// Controller Factory class for instantiating controllers using the Windsor IoC container.
	/// Copied from MvcContrib (http://mvccontrib.googlecode.com/svn/trunk). Not using MvcContrib anymore.
	/// </summary>
	public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IWindsorContainer _container;

		/// <summary>
		/// Creates a new instance of the <see cref="WindsorControllerFactory"/> class.
		/// </summary>
		/// <param name="container">The Windsor container instance to use when creating controllers.</param>
		public WindsorControllerFactory(IWindsorContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			_container = container;
		}

		protected override IController GetControllerInstance(Type controllerType)
		{
			if (controllerType == null)
			{
				throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", RequestContext.HttpContext.Request.Path));
			}

			return (IController)_container.Resolve(controllerType);
		}

		public override void ReleaseController(IController controller)
		{
			var disposable = controller as IDisposable;

			if (disposable != null)
			{
				disposable.Dispose();
			}

			_container.Release(controller);
		}
	}
}