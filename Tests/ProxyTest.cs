using System;

using NHibernate;
using NUnit.Framework;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;


namespace Cuyahoga.Tests
{
	/// <summary>
	/// Summary description for ProxyTest.
	/// </summary>
	[TestFixture]
	public class ProxyTest
	{
		public ProxyTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[Test]
		public void ProxyTestTemplate()
		{
			CoreRepository cr = new CoreRepository(true);
			Template template = (Template)cr.GetObjectById(typeof(Template), 3);
			Assert.IsTrue(template != null);
			Assert.AreEqual("Cuyahoga", template.Name);
		}
	}
}
