// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// Some imported utility methods.
	/// </summary>
	public static class CommonUtils
	{
		/// <summary>
		/// Obtains the entry and remove it if found.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>the entry value or the default value</returns>
		public static string ObtainEntryAndRemove(IDictionary attributes, string key, string defaultValue)
		{
			string value = ObtainEntryAndRemove(attributes, key);

			return value ?? defaultValue;
		}

		/// <summary>
		/// Obtains the entry and remove it if found.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		/// <param name="key">The key.</param>
		/// <returns>the entry value or null</returns>
		public static string ObtainEntryAndRemove(IDictionary attributes, string key)
		{
			string value = null;

			if (attributes != null && attributes.Contains(key))
			{
				value = (String)attributes[key];

				attributes.Remove(key);
			}

			return value;
		}

		/// <summary>
		/// Quotes the specified string with double quotes
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>A quoted string</returns>
		public static string SQuote(object content)
		{
			return "\'" + content + "\'";
		}
	}
}