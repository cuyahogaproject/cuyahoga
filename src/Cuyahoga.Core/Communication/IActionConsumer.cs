using System;

namespace Cuyahoga.Core.Communication
{
	/// <summary>
	/// The IActionConsumer interface contains methods that modules have to implement 
	/// when acting as a module that can be controlled from other modules.
	/// </summary>
	public interface IActionConsumer
	{
		ModuleActionCollection GetInboundActions();
	}
}
