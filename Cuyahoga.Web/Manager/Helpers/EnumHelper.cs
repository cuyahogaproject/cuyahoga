using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web.Mvc;
using Resources.Cuyahoga.Web.Manager;

namespace Cuyahoga.Web.Manager.Helpers
{
	/// <summary>
	/// Helper to prepare enumerations for display usage.
	/// </summary>
	public static class EnumHelper
	{
		public static ListItem[] GetListItems<T>(bool includeSelect)
		{
			IList<ListItem> items = GetValuesWithDescription<T>().Select(x => new ListItem { Text = x.Value, Value = x.Key }).ToList();
			if (includeSelect)
			{
				items.Insert(0, new ListItem { Value = String.Empty, Text = GlobalResources.SelectLabel });
			}
			return items.ToArray();
		}

		/// <summary>
		/// Gets a dictionary with description and values from the specified enumtype
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> GetValuesWithDescription<T>()
		{
			var descriptionandvalues = new Dictionary<string, string>();
			int i = 0;
			Type enumtype = typeof(T);
			foreach (string name in Enum.GetNames(enumtype))
			{
				Enum value = (Enum)Enum.GetValues(enumtype).GetValue(i);
				descriptionandvalues.Add(((int)Enum.GetValues(enumtype).GetValue(i)).ToString(), GetEnumDescription(value));
				i++;
			}
			return descriptionandvalues;
		}

		public static string GetEnumDescription<T>(T value)
		{
			ResourceManager resourceManager = Resources.Cuyahoga.Enums.ResourceManager;
			Type type = value.GetType();
			string resourceKey = type.Name + "_" + value;
			string description = resourceManager.GetString(resourceKey, Thread.CurrentThread.CurrentUICulture);
			if (string.IsNullOrEmpty(description))
			{
				// Just return the value of the enum when no text is found
				description = value.ToString();
			}
			return description;
		}
	}
}
