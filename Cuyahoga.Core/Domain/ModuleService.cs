namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Describes a service that belongs to a specific ModuleType. 
	/// For example, if a ModuleType uses its own specific DAO class, this
	/// class describes that class. The module loader then first registers
	/// the module services before trying to obtain a module instance.
	/// </summary>
	public class ModuleService
	{
		private string _serviceKey;
		private string _serviceType;
		private string _classType;
		private string _lifestyle;
		private ModuleType _moduleType;

		/// <summary>
		/// The key under which the service is registrated in the container.
		/// </summary>
		public string ServiceKey
		{
			get { return this._serviceKey; }
			set { this._serviceKey = value; }
		}

		/// <summary>
		/// The full type name of the service interface.
		/// </summary>
		public string ServiceType
		{
			get { return this._serviceType; }
			set { this._serviceType = value; }
		}

		/// <summary>
		/// The full type name of the service implementation class.
		/// </summary>
		public string ClassType
		{
			get { return this._classType; }
			set { this._classType = value; }
		}

		/// <summary>
		/// The (optional) Lifestyle of the service. Possibilities are Singleton (default) , Transient, 
		/// PerThread, Pooled.
		/// </summary>
		public string Lifestyle
		{
			get { return this._lifestyle; }
			set { this._lifestyle = value; }
		}

		/// <summary>
		/// The ModuleType where this service belongs to.
		/// </summary>
		public ModuleType ModuleType
		{
			get { return this._moduleType; }
			set { this._moduleType = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public ModuleService()
		{
		}
	}
}
