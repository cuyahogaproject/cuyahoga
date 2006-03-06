using System;

using Castle.Model;

namespace Cuyahoga.Web.UI
{
	/// <summary>
	/// Base class for all aspx pages in Cuyahoga (public and admin).
	/// </summary>
	[Transient]
	public class CuyahogaPage : System.Web.UI.Page, ICuyahogaPage
	{
		public CuyahogaPage()
		{
		}
	}
}
