using System;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Communication;

namespace Cuyahoga.Modules.Search
{
	/// <summary>
	/// Summary description for SearchInputModule.
	/// </summary>
	public class SearchInputModule : ModuleBase, IActionProvider
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public SearchInputModule()
		{
		}

		#region IActionProvider Members

		public ActionCollection GetOutboundActions()
		{
			ActionCollection ac = new ActionCollection();
			ac.Add(new Action("Search", new string[0]));
			return ac;
		}

		#endregion
	}
}
