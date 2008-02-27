using System;
using System.Globalization;
using System.Reflection;
using Castle.Core;
using Castle.MonoRail.Framework;
using Castle.MonoRail.Framework.Internal;
using log4net;

namespace Cuyahoga.Web.Components.MonoRail
{
	public class GlobalResourceFactory : IResourceFactory, IServiceEnabledComponent
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(GlobalResourceFactory));

		public IResource Create(ResourceDescriptor descriptor, Assembly appAssembly)
		{
			CultureInfo cultureInfo = this.ResolveCulture(descriptor.CultureName);
			return new GlobalResourceFacade(descriptor.ResourceName, cultureInfo);
		}

		public void Release(IResource resource)
		{
			resource.Dispose();
		}

		public void Service(IServiceProvider provider)
		{
		}

		private CultureInfo ResolveCulture(string name)
		{
			if (log.IsDebugEnabled)
				log.Debug(String.Format("Resolving culture {0}", name));

			if ("neutral".Equals(name))
				return CultureInfo.InvariantCulture;

			if (name != null)
				return CultureInfo.CreateSpecificCulture(name);

			return CultureInfo.CurrentCulture;
		}
	}
}
