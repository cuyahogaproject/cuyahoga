using System;
using System.Configuration;

using NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

using Castle.Facilities.NHibernateIntegration;
using Castle.MicroKernel;

using Castle.Model.Configuration;

namespace Cuyahoga.Core.Facilities
{
	/// <summary>
	/// The CuyahogaNHibernateFacility extends the Castle NHibernateFacility by registering a
	/// different ISessionManager that allows for dynamically rebuilding the SessionFactory.
	/// </summary>
	public class CuyahogaNHibernateFacility : NHibernateFacility
	{
		protected override void Init()
		{
			RegisterSessionStore();

			RegisterCuyahogaSessionManager();

			RegisterSessionFactoryResolver();

			RegisterTransactionManager();

			ConfigureCuyahogaFacility();
		}

		protected void RegisterCuyahogaSessionManager()
		{
			Kernel.AddComponent("nhfacility.sessionmanager", typeof(ISessionManager), typeof(CuyahogaSessionManager));
			
		}

		protected void RegisterSessionFactoryResolver()
		{
			Kernel.AddComponent("cuyahoga.nhfacility.sessionfactoryresolver", typeof(SessionFactoryResolver));
		}

		protected void ConfigureCuyahogaFacility()
		{
			ISessionManager sessionManager = (ISessionManager) Kernel[typeof(ISessionManager)];
			SessionFactoryResolver sessionFactoryResolver = (SessionFactoryResolver)Kernel[typeof(SessionFactoryResolver)];

			bool firstFactory = true;

			foreach(IConfiguration factoryConfig in FacilityConfig.Children)
			{
				if (!"factory".Equals(factoryConfig.Name))
				{
					throw new ConfigurationException("Unexpected node " + factoryConfig.Name);
				}

				ConfigureFactories(factoryConfig, sessionManager, firstFactory, sessionFactoryResolver);

				firstFactory = false;
			}
		}

		private void ConfigureFactories(IConfiguration config, 
			ISessionManager sessionManager, bool firstFactory, SessionFactoryResolver sessionFactoryResolver)
		{
			String id = config.Attributes["id"];

			if (id == null || String.Empty.Equals(id))
			{
				throw new ConfigurationException("You must provide a " + 
					"valid 'id' attribute for the 'factory' node. This id is used as key for " + 
					"the ISessionFactory component registered on the container");
			}

			String alias = config.Attributes["alias"];

			if (!firstFactory && (alias == null || alias.Length == 0))
			{
				throw new ConfigurationException("You must provide a " + 
					"valid 'alias' attribute for the 'factory' node. This id is used to obtain " + 
					"the ISession implementation from the SessionManager");
			}
			else if (alias == null || alias.Length == 0)
			{
				alias = Constants.DefaultAlias;
			}

			Configuration cfg = new Configuration();

			ApplyConfigurationSettings(cfg, config.Children["settings"]);
			RegisterAssemblies(cfg, config.Children["assemblies"]);
			RegisterResources(cfg, config.Children["resources"]);

			// Registers the Configuration object

			Kernel.AddComponentInstance( String.Format("{0}.cfg", id), cfg );


			// Registers the ISessionFactory as a component

			ISessionFactory sessionFactory = cfg.BuildSessionFactory();

			Kernel.AddComponentInstance( id, typeof(ISessionFactory), sessionFactory );

			// Registers the ISessionFactory within the ISessionManager

			sessionManager.RegisterSessionFactory(alias, sessionFactory);

			// Registers the mapping between the alias ant the component id
			sessionFactoryResolver.RegisterAliasComponentIdMapping(alias, id);
		}
	}
}
