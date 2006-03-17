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
			AddComponent("core.commondao", typeof(ICommonDao), typeof(CommonDao));
			AddComponent("core.sitestructuredao", typeof(ISiteStructureDao), typeof(SiteStructureDao));
			AddComponent("core.userdao", typeof(IUserDao), typeof(UserDao));
			AddComponent("core.siteservice", typeof(ISiteService), typeof(SiteService));
			AddComponent("core.nodeservice", typeof(INodeService), typeof(NodeService));
			AddComponent("core.sectionservice", typeof(ISectionService), typeof(SectionService));
			AddComponent("corerepositoryadapter", typeof(CoreRepositoryAdapter));
		}


		private void ConfigureLegacySessionFactory()
		{
			// TODO: get rid of this solution as soon as possible!
			AddComponent("core.legacysessionfactory", typeof(SessionFactory));
			SessionFactory cuyahogaSessionFactory = this[typeof(SessionFactory)] as SessionFactory;
			// We can't auto-wire the ISessionFactory via the constructor because it's
			// impossible to remove the old ISessonFactory from the container after a rebuild.
			cuyahogaSessionFactory.ExternalInitialize(this[typeof(ISessionFactory)] as ISessionFactory);
			cuyahogaSessionFactory.SessionFactoryRebuilt += new EventHandler(cuyahogaSessionFactory_SessionFactoryRebuilt);
		}

		private void cuyahogaSessionFactory_SessionFactoryRebuilt(object sender, EventArgs e)
		{
			ISessionFactory newNhSessionFactory = ((SessionFactory)this[typeof(SessionFactory)]).GetNHibernateFactory();
			this.Kernel.RemoveComponent("nhibernate.factory");
			this.Kernel.AddComponentInstance("nhibernate.factory", newNhSessionFactory);
		}
	}
}
