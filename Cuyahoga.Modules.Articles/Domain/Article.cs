using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Search;

namespace Cuyahoga.Modules.Articles.Domain
{
	/// <summary>
	/// Summary description for Article.
	/// </summary>
	public class Article : ContentItem, ISearchableContent
	{
		private string _content;

		/// <summary>
		/// Property Content (string)
		/// </summary>
		public virtual string Content
		{
			get { return this._content; }
			set { this._content = value; }
		}

		/// <summary>
		/// Get the full contents of this ContentItem for indexing
		/// </summary>
		/// <returns></returns>
		public virtual string ToSearchContent()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get a list of <see cref="CustomSearchField"/>s, if any
		/// </summary>
		/// <returns></returns>
		public virtual IList<CustomSearchField> GetCustomSearchFields()
		{
			return new List<CustomSearchField>();
		}
	}
}
