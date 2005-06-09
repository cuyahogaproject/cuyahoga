using System;
using System.Web;

using Cuyahoga.Core.Service;

namespace Cuyahoga.Web.HttpModules
{
	/// <summary>
	/// Http module that manages the NHibernate sessions during an HTTP Request.
	/// </summary>
	public class NHSessionModule : IHttpModule
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHSessionModule()
		{
		}

		public void Init(HttpApplication context)
		{
			context.BeginRequest += new EventHandler(Context_BeginRequest);
			context.EndRequest += new EventHandler(Context_EndRequest);
		}

		public void Dispose()
		{
			// Nothing here	
		}

		private void Context_BeginRequest(object sender, EventArgs e)
		{
			// Create the repository for Core objects and add it to the current HttpContext.
			CoreRepository cr = new CoreRepository(true);
			HttpContext.Current.Items.Add("CoreRepository", cr);
		}

		private void Context_EndRequest(object sender, EventArgs e)
		{
			// Close the NHibernate session.
			if (HttpContext.Current.Items["CoreRepository"] != null)
			{
				CoreRepository cr = (CoreRepository)HttpContext.Current.Items["CoreRepository"];
				cr.CloseSession();
			}
		}
	}
}
