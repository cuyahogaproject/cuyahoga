using System;

namespace Cuyahoga.Core.Domain
{
	/// <summary>
	/// Represents a comment.
	/// </summary>
	public class Comment
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		/// The Date and time when the comment was placed.
		/// </summary>
		public virtual DateTime CommentDateTime { get; set; }

		/// <summary>
		/// The name of the commenter.
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// The web site address of the commenter.
		/// </summary>
		public virtual string WebSite { get; set; }

		/// <summary>
		/// The comment.
		/// </summary>
		public virtual string CommentText { get; set; }

		/// <summary>
		/// IP address of the commenter.
		/// </summary>
		public virtual string UserIp { get; set; }

		/// <summary>
		/// The <see cref="ContentItem"></see> where this comment applies to.
		/// </summary>
		public virtual ContentItem ContentItem { get; set; }

		/// <summary>
		/// References the <see cref="User"></see> that has made the comment (optional).
		/// </summary>
		public virtual User User { get; set; }

		/// <summary>
		/// Gets the name of the person who commented (either from the associated user or the explicit name).
		/// </summary>
		public virtual string AuthorName
		{
			get
			{
				return this.User != null ? this.User.FullName : this.Name;
			}
		}

		/// <summary>
		/// Creates a new instance of the <see cref="Comment"></see> class.
		/// </summary>
		public Comment()
		{
			this.Id = -1;
		}
	}
}
