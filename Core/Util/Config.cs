using System;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Summary description for Config.
	/// </summary>
	public class Config
	{
		private Config()
		{
		}
        
		public static NameValueCollection GetConfiguration()
		{
			return (NameValueCollection)ConfigurationSettings.GetConfig("CuyahogaSettings");
		}
	}

	public class CuyahogaSectionHandler : NameValueSectionHandler
	{
		protected override string KeyAttributeName
		{
			get { return "setting";	}
		}

		protected override string ValueAttributeName
		{
			get { return base.ValueAttributeName; }
		}
	}
}
