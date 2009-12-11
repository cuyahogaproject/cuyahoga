using System.Web.Routing;
using Cuyahoga.Core;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Mvc.ViewModels
{
	public class ModuleAdminViewModel<T> where T: class
	{
		public Node Node { get; private set; }
		public Section Section { get; private set; }
		public ICuyahogaContext CuyahogaContext { get; private set; }
		public T ModuleData { get; private set; }

		public ModuleAdminViewModel(Node node, Section section, ICuyahogaContext cuyahogaContext, T moduleData)
		{
			this.Node = node;
			this.Section = section;
			this.CuyahogaContext = cuyahogaContext;
			this.ModuleData = moduleData;
		}

		public RouteValueDictionary GetNodeAndSectionParams()
		{
			RouteValueDictionary nodeAndSectionParams = new RouteValueDictionary();
			if (this.Node != null)
			{
				nodeAndSectionParams.Add("nodeid", this.Node.Id);
			}
			if (this.Section != null)
			{
				nodeAndSectionParams.Add("sectionid", this.Section.Id);
			}
			return nodeAndSectionParams;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class RouteValueDictionaryExtensions
	{
		/// <summary>
		/// Merge a given key value pair with the RouteValueDictionary. 
		/// </summary>
		/// <param name="routeValueDictionary"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static RouteValueDictionary Merge(this RouteValueDictionary routeValueDictionary, string key, object value)
		{
			if (! routeValueDictionary.ContainsKey(key))
			{
				routeValueDictionary.Add(key, value);
			}
			else
			{
				routeValueDictionary[key] = value;
			}
			return routeValueDictionary;
		}
	}
}
