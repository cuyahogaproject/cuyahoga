using System;
using System.Collections;

using Cuyahoga.Core;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// The ArticleModule provides a news system (articles, comments, content expiration, rss feed).
	/// </summary>
	public class ArticleModule : Module
	{
		private IList _articles;

		/// <summary>
		/// Property Articles
		/// </summary>
		public IList Articles
		{
			get { return this._articles; }
			set { this._articles = value; }
		}

		public ArticleModule()
		{
			base.Section = null;
			this._articles = null;
		}

		public override void LoadContent()
		{
		}

		public override void DeleteContent()
		{
			if (this._articles != null && this._articles.Count > 0)
			{
				throw new DeleteForbiddenException("You can't delete an article section when there is still content in it.");
			}
		}
	}
}
