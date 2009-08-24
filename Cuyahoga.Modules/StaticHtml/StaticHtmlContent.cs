using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Search;
using Cuyahoga.Core.Util;

namespace Cuyahoga.Modules.StaticHtml
{
	/// <summary>
	/// Summary description for StaticHtml.
	/// </summary>
	public class StaticHtmlContent : ContentItem, ISearchableContent
	{
		private string _content;

		/// <summary>
		/// The content.
		/// </summary>
		public virtual string Content
		{
			get { return this._content; }
			set { this._content = value; }
		}

		public override string GetContentUrl()
		{
			// return this url of the node itself when this object is connected to a single Node.
			if (this.Section.Node != null)
			{
				return UrlUtil.GetFriendlyUrlFromNode(this.Section.Node);
			}
			else
			{
				return base.GetContentUrl();
			}
		}

		public virtual string ToSearchContent(ITextExtractor textExtractor)
		{
			return this._content;
		}

		public virtual IList<CustomSearchField> GetCustomSearchFields()
		{
			return new List<CustomSearchField>();
		}
	}
}
