using System;
using System.Collections;

using Gentle.Framework;

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
			get 
			{ 
				if (this._articles == null && base.Section != null)
				{
					SqlBuilder sb = new SqlBuilder(StatementType.Select, typeof(Article));
					sb.AddConstraint(Operator.Equals, "sectionid", base.Section.Id);
					SqlResult res = Broker.Execute(sb.GetStatement());
					this._articles = ObjectFactory.GetCollection(typeof(Article), res);
				}
				return this._articles; 
			}
		}

		public ArticleModule()
		{
			base.Section = null;
			this._articles = null;
		}

		public IList GetAvailableCategories()
		{
			return Broker.RetrieveList(typeof(Category));
		}

		public override void LoadContent()
		{
		}

		public override void DeleteContent()
		{
			if (this.Articles != null && this.Articles.Count > 0)
			{
				throw new DeleteForbiddenException("You can't delete an article section when there is still content in it.");
			}
		}
	}
}
