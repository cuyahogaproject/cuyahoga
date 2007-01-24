using System;
using System.Collections;
using Cuyahoga.Core.Domain;
using Cuyahoga.Modules.Articles.Domain;
using ArticleCategory = Cuyahoga.Modules.Articles.Domain.Category;

namespace Cuyahoga.Modules.Articles.DataAccess
{
	/// <summary>
	/// Specific Data Access functionality for Article module.
	/// </summary>
	public interface IArticleDao
	{
		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		IList GetAvailableCategoriesBySite(Site site);

		/// <summary>
		/// Find a category with a given title and site.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="site"></param>
		/// <returns></returns>
		ArticleCategory FindCategoryByTitleAndSite(string title, Site site);

		/// <summary>
		/// Get all articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		IList GetAllArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection);

		/// <summary>
		/// Get all archived articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <returns></returns>
		IList GetArchivedArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection);

		/// <summary>
		/// Get all online articles for a given category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		IList GetDisplayArticlesByCategory(ArticleCategory category, SortBy sortBy, SortDirection sortDirection);

		/// <summary>
		/// Get all online articles for a given section.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		IList GetDisplayArticlesBySection(Section section, SortBy sortBy, SortDirection sortDirection);

		/// <summary>
		/// Get all online articles of a given section for syndication. Limit the number to the given amount.
		/// </summary>
		/// <param name="section"></param>
		/// <param name="maxNumberOfArticles"></param>
		/// <param name="sortBy"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		IList GetRssArticles(Section section, int maxNumberOfArticles, SortBy sortBy, SortDirection sortDirection);

		/// <summary>
		/// Get all online articles of a given category for syndication.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		IList GetRssArticlesByCategory(ArticleCategory category, SortBy sortBy, SortDirection sortDirection);
	}
}
