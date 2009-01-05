using System;
using System.Web;

using Castle.Windsor;

using Cuyahoga.Core.Service;
using Cuyahoga.Web.Components;
using Cuyahoga.Web.Util;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// Http module that manages the NHibernate sessions during an HTTP Request.
	/// </summary>
	public class CoreRepositoryModule : IHttpModule
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CoreRepositoryModule()
		{
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(Context_BeginRequest);
		}

		public void Dispose()
		{
			// Nothing here	
		}

		private void Context_BeginRequest(object sender, EventArgs e)
		{
			// Get the adapter for the 1.0 CoreRepository and store it in the HttpContext.Items collection.
			IWindsorContainer container = ContainerAccessorUtil.GetContainer();
			CoreRepository cr = (CoreRepository)container["corerepositoryadapter"];
			HttpContext.Current.Items.Add("CoreRepository", cr);
		}
	}
}
