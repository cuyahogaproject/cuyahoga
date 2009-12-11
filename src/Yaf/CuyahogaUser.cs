using System;
using System.Web;

using yaf;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.YetAnotherForum
{
	/// <summary>
	/// Stub class to translate Cuyahoga users into Yaf users. 
	/// </summary>
	public class CuyahogaUser : IForumUser
	{
		private int _userId; 
		private string _username;
		private string _email;
		private string _firstname;
		private string _lastname;
		private string _homepage;
		private bool _isAuthenticated;

		public CuyahogaUser()
		{
			try 
			{ 
				if (HttpContext.Current.User.Identity.IsAuthenticated) 
				{ 
					User user = (User)HttpContext.Current.User.Identity;
					this._userId = user.Id;
					this._username = user.UserName;
					this._email = user.Email;
					this._firstname = user.FirstName;
					this._lastname = user.LastName;
					this._homepage = user.Website;

					this._isAuthenticated = true;
				} 
				else
				{
					this._username = "";
					this._email = "";
					this._isAuthenticated = false;
				}
			} 
			catch (Exception ex) 
			{ 
				this._isAuthenticated = false; 
				this._username = "";
				throw new Exception("Failed to find user info from Cuyahoga.", ex); 
			}
		}

		#region IForumUser Members

		public string Email
		{
			get	{ return this._email; }
		}

		public bool IsAuthenticated
		{
			get	{ return this._isAuthenticated; }
		}

		public string Name 
		{	
			get { return this._username; }
		}

		public bool CanLogin
		{
			get { return false; }
		}

		public object Location
		{
			get { return null; }
		}

		public object HomePage
		{
			get	{ return this._homepage; }
		}

		public void UpdateUserInfo(int userID)
		{
			using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand()) 
			{ 
				cmd.CommandText = string.Format("update yaf_User set Email='{0}' where UserID={1}", this._email, userID);
				DB.ExecuteNonQuery(cmd); 
			} 
		}

		#endregion
	}
}
