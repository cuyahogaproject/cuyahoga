using System;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

namespace Cuyahoga.Modules.LanguageSwitcher
{
	/// <summary>
	/// The LanguageSwitcher module enables switching to other languages on the current site.
	/// This basically means that users can select a different root Node with a different 
	/// culture setting.
	/// </summary>
	public class LanguageSwitcherModule : ModuleBase
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public LanguageSwitcherModule()
		{
		}
	}
}
