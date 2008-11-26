using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Core;
using Castle.Windsor;
using Cuyahoga.Core;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;
using Cuyahoga.Core.Validation.ModelValidators;
using Cuyahoga.Web.Mvc.Validation;
using log4net;
using MvcContrib.Castle;

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
				container.AddComponentWithLifestyle("cuyahoga.context", typeof(ICuyahogaContext), typeof(CuyahogaContext),
													LifestyleType.PerWebRequest);

				// Windsor controller builder
				ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

				// Register ASP.NET MVC components
				RegisterMvcComponents(container);

				// Validators
				RegisterValidatorComponents(container);
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
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (typeof(IController).IsAssignableFrom(type))
				{
					container.Kernel.AddComponent(type.Name.ToLower(), type, LifestyleType.Transient);
				}
			}
		}

		private static void RegisterValidatorComponents(IWindsorContainer container)
		{
			container.AddComponent("validatorprovider", typeof(IBrowserValidatorProvider), typeof(JQueryValidator));
			container.AddComponent("validatorregistry", typeof(ILocalizedValidatorRegistry), typeof(CachedLocalizedValidatorRegistry));
			container.AddComponentWithLifestyle("modelvalidator", typeof(IModelValidator<>), typeof(CastleModelValidator<>), LifestyleType.Transient);
			container.AddComponentWithLifestyle("usermodelvalidator", typeof(UserModelValidator), LifestyleType.Transient);
			container.AddComponentWithLifestyle("rolemodelvalidator", typeof(RoleModelValidator), LifestyleType.Transient);
			container.AddComponent("validationengine", typeof(BrowserValidationEngine));
			container.Kernel.AddComponentInstance("validationresources", Resources.Cuyahoga.Web.Manager.ValidationMessages.ResourceManager);

			// Set validation engine to accessor.
			ValidationEngineAccessor.Current.SetValidationEngine(container.Resolve<BrowserValidationEngine>());
		}
	}
}
