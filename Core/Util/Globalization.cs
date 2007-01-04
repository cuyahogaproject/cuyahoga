using System;
using System.Collections;
using System.Globalization;

namespace Cuyahoga.Core.Util
{
	public class Globalization
	{
		public static SortedList GetOrderedCultures()
		{
			SortedList orderedCultures = new SortedList();
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
			{
				orderedCultures.Add(ci.DisplayName, ci.Name);
			}
			return (orderedCultures);
		}
	}
}
