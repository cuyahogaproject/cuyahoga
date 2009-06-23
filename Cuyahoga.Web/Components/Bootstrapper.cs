using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Cuyahoga.Core;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Mvc.Localization;
using Cuyahoga.Web.Mvc.Sitemap;
using Cuyahoga.Web.Mvc.Validation;
using Cuyahoga.Web.Mvc.Windsor;
using log4net;

namespace Cuyahoga.Web.Components
{
	/// <summary>
	/// Responsible for initializing the Cuyahoga application.
	/// </summary>
	public static class Bootstrapper
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));

		/// <summary>
		/// Initialize Container
		/// </summary>
		/// 
		public static void InitializeContainer()
		{
			try
			{
				// Initialize Windsor
				IWindsorContainer container = new CuyahogaContainer();

				// Inititialize the static Windsor helper class. 
				IoC.Initialize(container);

				// Add ICuyahogaContext to the container.
				container.Register(Component.For<ICuyahogaContext>()
					.ImplementedBy<CuyahogaContext>()
					.Named("cuyahoga.context")
					.LifeStyle.PerWebRequest
				);

				// Windsor controller builder
				ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

				// Register ASP.NET MVC components
				RegisterMvcComponents(container);

				// Validators
				RegisterValidatorComponents(container);

				// Localizer
				container.Register(Component.For<ILocalizer>()
					.ImplementedBy<GlobalResourceLocalizer>()
				);
			}
			catch (Exception ex)
			{
				log.Error("Error initializing application.", ex);
				throw;
			}
		}

		/// <summary>
		/// Initialize Cuyahoga (check installer).
		/// </summary>
		/// <remarks>
		/// Because we might perform some redirects, it's not possible to call this method from Application_Start 
		/// in Global.asax.cs (on IIS 7).
		/// </remarks>
		public static void InitializeCuyahoga()
		{
			// Check for any new versions
			CheckInstaller();
		}

		private static void CheckInstaller()
		{
			if (!HttpContext.Current.Request.RawUrl.Contains("Install"))
			{
				// Check version and redirect to install pages if neccessary.
				DatabaseInstaller dbInstaller = new DatabaseInstaller(HttpContext.Current.Server.MapPath("~/Install/Core"),
				                                                      Assembly.Load("Cuyahoga.Core"));
				if (dbInstaller.TestDatabaseConnection())
				{
					if (dbInstaller.CanUpgrade)
					{
						HttpContext.Current.Application.Lock();
						HttpContext.Current.Application["IsUpgrading"] = true;
						HttpContext.Current.Application.UnLock();

						HttpContext.Current.Response.Redirect("~/Install/Upgrade.aspx");
					}
					else if (dbInstaller.CanInstall)
					{
						HttpContext.Current.Application.Lock();
						HttpContext.Current.Application["IsInstalling"] = true;
						HttpContext.Current.Application.UnLock();
						HttpContext.Current.Response.Redirect("~/Install/Install.aspx");
					}
				}
				else
				{
					throw new Exception("Cuyahoga can't connect to the database. Please check your application settings.");
				}
			}
		}

		private static void RegisterMvcComponents(IWindsorContainer container)
		{
			container.Register(AllTypes.Of<IController>()
				.FromAssembly(Assembly.GetExecutingAssembly())
				.Configure(c => c.LifeStyle.Transient)
			);
			// Register MVC sitemap provider
			container.Register(Component.For<IMvcSitemapProvider>()
				.ImplementedBy<MvcSitemapProvider>()
			);
		}

		private static void RegisterValidatorComponents(IWindsorContainer container)
		{
			container.Register(Component.For<IBrowserValidatorProvider>()
				.ImplementedBy<JQueryValidator>()
			);
			container.Register(Component.For<ILocalizedValidatorRegistry>()
				.ImplementedBy<CachedLocalizedValidatorRegistry>()
			);
			container.Register(Component.For(typeof(IModelValidator<>))
				.ImplementedBy(typeof(CastleModelValidator<>))
				.LifeStyle.Transient
			);
			container.Register(Component.For<UserModelValidator>().LifeStyle.Transient);
			container.Register(Component.For<RoleModelValidator>().LifeStyle.Transient);
			container.Register(Component.For<SectionModelValidator>().LifeStyle.Transient);
			container.Register(Component.For<BrowserValidationEngine>());
			container.Kernel.AddComponentInstance("validationresources", Resources.Cuyahoga.Web.Manager.ValidationMessages.ResourceManager);

			// Set validation engine to accessor.
			ValidationEngineAccessor.Current.SetValidationEngine(container.Resolve<BrowserValidationEngine>());
		}
	}
}
