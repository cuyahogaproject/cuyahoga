
namespace Cuyahoga.Core
{
	/// <summary>
	/// Marker interface for modules that use NHibernate for data access. 
	/// When a module implements this interface, the module loader also 
	/// registers any mappings of module assembly in the configuration.
	/// TODO: move to a more appropriate namespace?
	/// </summary>
	public interface INHibernateModule
	{
	}
}
