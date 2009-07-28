namespace Cuyahoga.Core.Service.Content
{
	/// <summary>
	/// The property to sort content items by.
	/// </summary>
	public enum ContentItemSortBy
	{
		/// <summary>
		/// Sort by publishing date.
		/// </summary>
		PublishedAt,
		/// <summary>
		/// Sort by creation date.
		/// </summary>
		CreatedAt,
		/// <summary>
		/// Sort by modification date.
		/// </summary>
		ModifiedAt,
		/// <summary>
		/// Sort by title.
		/// </summary>
		Title,
		/// <summary>
		/// Sort by the author who created the content item.
		/// </summary>
		CreatedBy,
		/// <summary>
		/// Sort by the author who most recently modified the content item.
		/// </summary>
		ModifiedBy,
		/// <summary>
		/// Don't sort the articles.
		/// </summary>
		None
	}

	/// <summary>
	/// The sort direction of the content items.
	/// </summary>
	public enum ContentItemSortDirection
	{
		/// <summary>
		/// Sort descending.
		/// </summary>
		DESC,
		/// <summary>
		/// Sort ascending.
		/// </summary>
		ASC,
		/// <summary>
		/// Not applicable.
		/// </summary>
		None
	}
}
