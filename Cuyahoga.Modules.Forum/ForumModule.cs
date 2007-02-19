using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Type;

using Castle.Services.Transaction;
using Castle.Facilities.NHibernateIntegration;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Communication;

using Cuyahoga.Core.Service;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Forum.Domain;

namespace Cuyahoga.Modules.Forum 
{
	/// <summary>
	/// Summary description for ForumModule.
	/// </summary>
    public class ForumModule : ModuleBase, INHibernateModule
	{

		private ForumModuleAction _currentAction;

		private CategorySortBy	_categorySortBy;
        private bool _categorySortASC;

		private ForumSortBy		_forumSortBy;
        private bool _forumSortASC;

		private ForumPostSortBy	_forumPostSortBy;
        private bool _forumPostSortASC;

		private int				_currentForumId;
		private int				_currentForumCategoryId;
		private int				_currentForumPostId;
		private int				_currentUserId;
		private int				_quotePost;
		private int				_origForumPostId;
		private int				_downloadId;

		private string			_forumthemepath;
        private ISessionManager _sessionManager;
		
		#region Properties
		public int CurrentUserId
		{
			get { return this._currentUserId; }
			set { this._currentUserId = value; }
		}

		public int CurrentForumId
		{
			get { return this._currentForumId; }
			set { this._currentForumId = value; }
		}

		public int CurrentForumCategoryId
		{
			get { return this._currentForumCategoryId; }
			set { this._currentForumCategoryId = value; }
		}

		public int CurrentForumPostId
		{
			get { return this._currentForumPostId; }
			set { this._currentForumPostId = value; }
		}

		public int QuotePost
		{
			get { return this._quotePost; }
			set { this._quotePost = value; }
		}

		public string ThemePath
		{
			get { return this._forumthemepath; }
			set { this._forumthemepath = value; }
		}

		public int OrigForumPostId
		{
			get { return this._origForumPostId; }
			set { this._origForumPostId = value; }
		}

		public int DownloadId
		{
			get { return this._downloadId; }
			set { this._downloadId = value; }
		}

		public ForumModuleAction ModuleAction
		{
			get { return this._currentAction; }
		}
		#endregion

        public ForumModule(ISessionManager sessionManager)
		{
            _sessionManager = sessionManager;

            this._currentAction = ForumModuleAction.ForumList;

			_categorySortBy			= CategorySortBy.Name;
			_forumSortBy			= ForumSortBy.Name;
			_forumPostSortBy		= ForumPostSortBy.DateCreated;
            _categorySortASC = true;
            _forumSortASC = true;
            _forumPostSortASC = true;

 
			this._forumthemepath = UrlHelper.GetApplicationPath() + "Modules/Forum/Images/Standard/";
		}

		/// <summary>
		/// Override ParsePathInfo to determine action and optional parameters.
		/// </summary>
		protected override void ParsePathInfo()
		{
			base.ParsePathInfo();
			if (base.ModuleParams.Length > 0)
			{
				// First pathinfo parameter is the module action.
				try
				{
					this._currentAction = (ForumModuleAction)Enum.Parse(typeof(ForumModuleAction), base.ModuleParams[0], true);

					switch(this._currentAction)
					{
						case ForumModuleAction.ForumCategoryList:
							this.CurrentForumCategoryId = Int32.Parse(base.ModuleParams[1]);
							break;

						case ForumModuleAction.ForumView:
							this.CurrentForumId = Int32.Parse(base.ModuleParams[1]);
							break;

						case ForumModuleAction.ForumNewPost:
							this.CurrentForumId = Int32.Parse(base.ModuleParams[1]);
							break;

						case ForumModuleAction.ForumViewPost:
							this.CurrentForumId		= Int32.Parse(base.ModuleParams[1]);
							this.CurrentForumPostId	= Int32.Parse(base.ModuleParams[3]);
							
							if(base.ModuleParams.Length > 5 && base.ModuleParams[4] == "Download")
							{
								this.DownloadId	= Int32.Parse(base.ModuleParams[5]);
							}
							
							break;

						case ForumModuleAction.ForumReplyPost:
							this.CurrentForumId		= Int32.Parse(base.ModuleParams[1]);
							this.CurrentForumPostId	= Int32.Parse(base.ModuleParams[3]);
							this.OrigForumPostId	= Int32.Parse(base.ModuleParams[3]);
							this.QuotePost = 0;
							break;

						case ForumModuleAction.ForumReplyPostQuote:
							this.CurrentForumId		= Int32.Parse(base.ModuleParams[1]);
							this.CurrentForumPostId	= Int32.Parse(base.ModuleParams[3]);
							this.OrigForumPostId	= Int32.Parse(base.ModuleParams[5]);
							this.QuotePost = 1;
							this._currentAction = ForumModuleAction.ForumReplyPost;
							break;

						case ForumModuleAction.ForumProfile:
						case ForumModuleAction.ForumViewProfile:
							this.CurrentUserId	= Int32.Parse(base.ModuleParams[1]);
							break;
					}
				}
				catch (ArgumentException ex)
				{
					throw new Exception("Error when parsing module action: " + base.ModuleParams[0], ex);
				}
				catch (Exception ex)
				{
					throw new Exception("Error when parsing module parameters: " + base.ModulePathInfo, ex);
				}
			}
		}

        public override void DeleteModuleContent()
		{
			// Delete all the files that users have uploaded
			
			string sUpDir = HttpContext.Current.Request.PhysicalApplicationPath + "Modules/Forum/Attach/";
			if(System.IO.Directory.Exists(sUpDir))
			{
				System.IO.Directory.Delete(sUpDir,true);
			}
		}

		/// <summary>
		/// The current view user control based on the action that was set while parsing the pathinfo.
		/// </summary>
		public override string CurrentViewControlPath
		{
			get
			{
				string basePath = "Modules/Forum/";
				return basePath + this._currentAction.ToString() + ".ascx";
			}
		}

		#region Forum
		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetForumsByCategoryId(int categoryid)
        {
            ISession session = this._sessionManager.OpenSession();

			try
			{
                ICriteria forum = session.CreateCriteria(typeof(ForumForum)).Add(NHibernate.Expression.Expression.Eq("CategoryId", categoryid));
                forum.AddOrder(new Order(this._forumSortBy.ToString(), this._forumSortASC));

                IList forumlist = forum.List();

                return forumlist;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum by category", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public ForumForum GetForumById(int forumId)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumForum)session.Load(typeof(ForumForum), forumId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum by id", ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
		public virtual void SaveForum(ForumForum forum)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(forum);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Forum ", ex);
			}
		}

		/// <summary>
		/// Delete the meta-information of a file
		/// </summary>
        [Transaction(TransactionMode.RequiresNew)]
        public virtual void DeleteForum(ForumForum forum)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(forum); 
                tx.Commit();
                session.Close(); 
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Forum", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetAllForums()
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                ICriteria forum = session.CreateCriteria(typeof(ForumForum)).AddOrder(new Order(this._forumSortBy.ToString(), this._forumSortASC));

                IList forumlist = forum.List();

                return forumlist;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum ", ex);
			}
		}

		/// <summary>
		/// The property to sort the links by.
		/// </summary>
		public enum ForumSortBy
		{
			/// <summary>
			/// Sort by Name.
			/// </summary>
			Name,
			/// <summary>
			/// Sort by the user who created the article.
			/// </summary>
			CreatedBy,
			/// <summary>
			/// Sort by the user who modified the article most recently.
			/// </summary>
			ModifiedBy,
			/// <summary>
			/// Don't sort the articles.
			/// </summary>
			None
		}

		#endregion

		#region Category

    	//
		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetAllCategories()
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                ICriteria category = session.CreateCriteria(typeof(ForumCategory)).AddOrder(new Order(this._categorySortBy.ToString(), this._categorySortASC));

                IList categorylist = category.List();

                return categorylist;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum categories ", ex);
			}
		}


		/// <summary>
		/// Get the meta-information of a single file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		public ForumCategory GetForumCategoryById(int id)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumCategory)session.Load(typeof(ForumCategory), id);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Forum category identifier: " + id.ToString(), ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveForumCategory(ForumCategory forumcategory)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(forumcategory);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Forum category", ex);
			}
		}

		/// <summary>
		/// Delete the meta-information of a file
		/// </summary>
        [Transaction(TransactionMode.RequiresNew)]
        public virtual void DeleteForumCategory(ForumCategory forumcategory)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(forumcategory);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to delete Forum category", ex);
			}
		}

		/// <summary>
		/// The property to sort the links by.
		/// </summary>
		public enum CategorySortBy
		{
			/// <summary>
			/// Sort by Name.
			/// </summary>
			Name,
			/// <summary>
			/// Sort by the user who created the article.
			/// </summary>
			CreatedBy,
			/// <summary>
			/// Sort by the user who modified the article most recently.
			/// </summary>
			ModifiedBy,
			/// <summary>
			/// Don't sort the articles.
			/// </summary>
			None
		}

		#endregion

		#region Posts
		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetAllForumPosts(int forumid)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                ICriteria forumpost = session.CreateCriteria(typeof(ForumPost)).Add(NHibernate.Expression.Expression.Eq("ForumId", forumid));
                forumpost.AddOrder(new Order(this._forumPostSortBy.ToString(), this._forumPostSortASC));
                forumpost.Add(NHibernate.Expression.Expression.Eq("ReplytoId", 0));

                IList forumpostlist = forumpost.List();

                return forumpostlist;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum posts", ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveForumPost(ForumPost forumpost)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(forumpost);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Forum post", ex);
			}
		}

		/// <summary>
		/// Get the meta-information of a single file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <returns></returns>
		public ForumPost GetForumPostById(int id)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumPost)session.Load(typeof(ForumPost), id);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Forum post identifier: " + id.ToString(), ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public IList GetAllForumPostReplies(int postid)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                ICriteria forumpost = session.CreateCriteria(typeof(ForumPost)).Add(NHibernate.Expression.Expression.Eq("ReplytoId", postid));
                forumpost.AddOrder(new Order(this._forumPostSortBy.ToString(), this._forumPostSortASC));

                IList forumpostlist = forumpost.List();

                return forumpostlist;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get forum post replies", ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
        public virtual void DeleteForumPost(ForumPost post)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(post);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to delete Forum post", ex);
			}
		}

		public IList SearchForumPosts(string searchString)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
				string hql;
				hql = "from ForumPost f where f.Message like :sv order by f.DateCreated ";
                IQuery q = session.CreateQuery(hql);
				q.SetString("sv", "%" + searchString + "%");
				return q.List();
			}
			catch(Exception ex)
			{
				throw new Exception("Unable to search!",ex);
			}
		}

		public enum ForumPostSortBy
		{
			Topic,
			DateCreated,
			DateModified,
			UserName,
			None
		}

		#endregion

		#region Emoticons
		public ForumEmoticon GetEmoticonById(int eId)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumEmoticon)session.Load(typeof(ForumEmoticon), eId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get emoticon by id", ex);
			}
		}

		public IList GetAllEmoticons()
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                IList emoticons = session.CreateCriteria(typeof(ForumEmoticon)).List();
                return emoticons;
            }
			catch (Exception ex)
			{
				throw new Exception("Unable to get emoticons ", ex);
			}
		}


        [Transaction(TransactionMode.RequiresNew)]
        public void SaveEmoticon(ForumEmoticon emoticon)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(emoticon);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Emoticon ", ex);
			}
		}

		/// <summary>
		/// Delete the meta-information of a file
		/// </summary>
        [Transaction(TransactionMode.RequiresNew)]
        public void DeleteEmoticon(ForumEmoticon emoticon)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(emoticon);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to delete Emoticon", ex);
			}
		}

		#endregion

		#region Tag
		public ForumTag GetTagById(int tId)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumTag)session.Load(typeof(ForumTag), tId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get tag by id", ex);
			}
		}

		public IList GetAllTags()
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                IList tags = session.CreateCriteria(typeof(ForumTag)).List();
                return tags;		
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get tags ", ex);
			}
		}


        [Transaction(TransactionMode.RequiresNew)]
        public void SaveTag(ForumTag tag)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(tag);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Tag ", ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
        public void DeleteTag(ForumTag tag)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(tag);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to delete Tag", ex);
			}
		}

		#endregion

		#region Forum user
		public ForumUser GetForumUserById(int tId)
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                return (ForumUser)session.Load(typeof(ForumUser), tId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get user by id", ex);
			}
		}

		public IList GetAllForumUsers()
		{
            ISession session = this._sessionManager.OpenSession();
            try
			{
                IList users = session.CreateCriteria(typeof(ForumUser)).List();
                return users;
            }
			catch (Exception ex)
			{
				throw new Exception("Unable to get users ", ex);
			}
		}


        [Transaction(TransactionMode.RequiresNew)]
        public void SaveForumUser(ForumUser user)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(user);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Tag ", ex);
			}
		}

        [Transaction(TransactionMode.RequiresNew)]
        public void DeleteForumUser(ForumUser user)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.Delete(user);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to delete user", ex);
			}
		}

		public ForumUser GetForumUserByUserId(int tId)
		{
            ISession session = this._sessionManager.OpenSession();
            IList l;
			ForumUser user = new ForumUser();
			try
			{
                l = session.Find("from ForumUser f where f.UserId=?", tId, NHibernateUtil.Int32);
				if(l.Count == 0)
				{
					return null;
				}
				else
				{
					user = (ForumUser)l[0];
				}
				
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get user by id", ex);
			}

			return user;
		}

		#endregion


		#region Forum files
        [Transaction(TransactionMode.RequiresNew)]
        public virtual void SaveForumFile(ForumFile forumfile)
		{
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(forumfile);
                tx.Commit();
                session.Close();
			}
			catch (Exception ex)
			{
                tx.Rollback();
				throw new Exception("Unable to save Forum file", ex);
			}
		}
        [Transaction(TransactionMode.RequiresNew)]
        public virtual void DeleteForumFile(ForumFile forumfile)
        {
            ISession session = this._sessionManager.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                System.IO.File.Delete(forumfile.ForumFileName);
                session.Delete(forumfile);
                tx.Commit();
                session.Close();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new Exception("Unable to save Forum file", ex);
            }
        }

		public ForumFile GetForumFileById(int id)
		{
            ISession session = this._sessionManager.OpenSession();

			try
			{
                return (ForumFile)session.Load(typeof(ForumFile), id);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get form file by id", ex);
			}
		}

		#endregion
		public enum SortDirection
		{
			DESC,
			ASC
		}
	}


	/// <summary>
	/// 
	/// </summary>
	public enum ForumModuleAction
	{
		/// <summary>
		/// 
		/// </summary>
		ForumList,
		/// <summary>
		/// 
		/// </summary>
		ForumCategoryList,
		/// <summary>
		/// 
		/// </summary>
		ForumView,
		/// <summary>
		/// 
		/// </summary>
		ForumNewPost,
		ForumViewPost,
		ForumReplyPost,
		ForumReplyPostQuote,
		ForumSearch,
		ForumProfile,
		ForumViewProfile
	}
}
