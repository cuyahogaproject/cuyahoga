using System;

using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Facilities.NHibernateIntegration;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// The CuyahogaContainer serves as the IoC container for Cuyahoga.
	/// </summary>
	public class CuyahogaContainer : WindsorContainer
	{
		/// <summary>
		/// Constructor. The configuration is read from the web.config.
		/// </summary>
		public CuyahogaContainer() : base(new XmlInterpreter())
		{
			RegisterFacilities();
			RegisterServices();
		}

		private void RegisterFacilities()
		{
			AddFacility("nhibernate", new NHibernateFacility());
		}

		private void RegisterServices()
		{
			
		}
	}
}
