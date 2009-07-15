using System;
using Castle.Windsor;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Wrapper around the Windsor Container class for easy access to services (also outside the HttpContext).
	/// </summary>
	public static class IoC
	{
		private static IWindsorContainer container;

		/// <summary>
		/// The inner Windsor container.
		/// </summary>
		public static IWindsorContainer Container
		{
			get
			{
				if (container == null)
				{
					throw new InvalidOperationException("The container has not been initialized!");
				}
				return container;
			}
		}

		/// <summary>
		/// Indicates if the container is initialized properly.
		/// </summary>
		public static bool IsInitialized
		{
			get { return container != null; }
		}

		/// <summary>
		/// Initialize the IoC wrapper
		/// </summary>
		/// <param name="windsorContainer"></param>
		public static void Initialize(IWindsorContainer windsorContainer)
		{
			container = windsorContainer;
		}

		/// <summary>
		/// Resolve a service of the given type.
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public static object Resolve(Type serviceType)
		{
			return Container.Resolve(serviceType);
		}

		/// <summary>
		/// Resolve a service of the given type and with the given name.
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="serviceName"></param>
		/// <returns></returns>
		public static object Resolve(Type serviceType, string serviceName)
		{
			return Container.Resolve(serviceName, serviceType);
		}

		/// <summary>
		/// Resolve a service of the given type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Resolve<T>()
		{
			return Container.Resolve<T>();
		}

		/// <summary>
		/// Resolve a service of the given type parameter and with the given name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Resolve<T>(string name)
		{
			return Container.Resolve<T>(name);
		}

		/// <summary>
		/// Check if a component of type T is registered in the container.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static bool HasComponent<T>()
		{
			return Container.Kernel.HasComponent(typeof(T));
		}
	}
}
