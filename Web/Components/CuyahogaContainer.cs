using System;

using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Facilities.NHibernateIntegration;
using Castle.Facilities.AutomaticTransactionManagement;

using NHibernate;
using NHibernate.Cfg;

using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Service.SiteStructure;
using Cuyahoga.Core.Service.Content;
using Cuyahoga.Core.Service.Files;

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
			ConfigureLegacySessionFactory();
		}

		private void RegisterFacilities()
		{
			AddFacility("nhibernate", new Cuyahoga.Core.Facilities.CuyahogaNHibernateFacility());
			AddFacility("autotransaction", new TransactionFacility());
		}

		private void RegisterServices()
		{
			// The core services are registrated via services.config
			
			// Utility services
			AddComponent("web.moduleloader", typeof(ModuleLoader));
			AddComponent("core.sessionfactoryhelper", typeof(SessionFactoryHelper));

			// Legacy
			AddComponent("corerepositoryadapter", typeof(CoreRepositoryAdapter));
		}

		private void ConfigureLegacySessionFactory()
		{
			SessionFactoryHelper sessionFactoryHelper = this[typeof(SessionFactoryHelper)] as SessionFactoryHelper;
			sessionFactoryHelper.ConfigureLegacySessionFactory();
		}
	}
}
