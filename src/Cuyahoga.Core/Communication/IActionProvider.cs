using System;

namespace Cuyahoga.Core.Communication
{
	/// <summary>
	/// The IActionProvider interface contains methods that modules have to implement 
	/// when acting as a module that control other modules.
	/// </summary>
	public interface IActionProvider
	{
		ModuleActionCollection GetOutboundActions();
	}
}
