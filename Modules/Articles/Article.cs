using System;

using Cuyahoga.Core;

namespace Cuyahoga.Modules.Articles
{
	/// <summary>
	/// Summary description for Article.
	/// </summary>
	public class Article
	{
		private int _id;
		private int _sectionId;
		private int _createdById;
		private int _modifiedById;
		private int _categoryId;
		private string _title;
		private string _summary;
		private string _content;
		private bool _syndicate;
		private DateTime _dateOnline;
		private DateTime _dateOffline;
		private DateTime _created;
		private DateTime _updated;
		private Section _section;
		private User _createdBy;
		private User _modifiedBy;
		private Category _category;
		private IList _comments;

		#region properties


		#endregion

		public Article()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
