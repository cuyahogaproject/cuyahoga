#region Copyright and License
/*
Copyright 2006 Dominique Paris, xp-rience.net
Design work copyright Dominique Paris (http://www.xp-rience.net/)

You can use this Software for any commercial or noncommercial purpose, 
including distributing derivative works.

In return, we simply require that you agree:

1. Not to remove any copyright notices from the Software. 
2. That if you distribute the Software in source code form you do so only 
   under this License (i.e. you must include a complete copy of this License 
   with your distribution), and if you distribute the Software solely in 
   object form you only do so under a license that complies with this License. 
3. That the Software comes "as is", with no warranties. None whatsoever. This 
   means no express, implied or statutory warranty, including without 
   limitation, warranties of merchantability or fitness for a particular 
   purpose or any warranty of noninfringement. Also, you must pass this 
   disclaimer on whenever you distribute the Software.
4. That if you sue anyone over patents that you think may apply to the 
   Software for a person's use of the Software, your license to the Software 
   ends automatically. 
5. That the patent rights, if any, licensed hereunder only apply to the 
   Software, not to any derivative works you make. 
6. That your rights under this License end automatically if you breach it in 
   any way.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;

namespace Cuyahoga.Modules.Gallery.Domain
{
	/// <summary>
	/// Summary description for Comment.
	/// </summary>
	public class GalleryComment
	{
		private int _id;
		private string _commentText;
		private string _culture;
		private PhotoGallery  _gallery;
		private Cuyahoga.Core.Domain.User _user;
		private string _name;
		private string _website;
		private string _userIp;
		private DateTime _updated;
		private DateTime _created;

		#region properties

		/// <summary>
		/// Property Id (int)
		/// </summary>
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Property Article (Article)
		/// </summary>
		public PhotoGallery Gallery
		{
			get { return this._gallery; }
			set { this._gallery = value; }
		}

		/// <summary>
		/// Property User (Cuyahoga.Core.User)
		/// </summary>
		public Cuyahoga.Core.Domain.User User
		{
			get { return this._user; }
			set { this._user = value; }
		}

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public string Name
		{
			get { return this._name; }
			set { this._name = value; }
		}

		/// <summary>
		/// Property Website (string)
		/// </summary>
		public string Website
		{
			get { return this._website; }
			set { this._website = value; }
		}

		/// <summary>
		/// Current Culture of the user
		/// </summary>
		public string Culture
		{
			get { return this._culture; }
			set { this._culture = value; }
		}

		/// <summary>
		/// Property CommentText (string)
		/// </summary>
		public string CommentText
		{
			get { return this._commentText; }
			set { this._commentText = value; }
		}

		/// <summary>
		/// Property UserIp (string)
		/// </summary>
		public string UserIp
		{
			get { return this._userIp; }
			set { this._userIp = value; }
		}

		/// <summary>
		/// Property UpdateTimestamp (DateTime)
		/// </summary>
		public DateTime DateUpdated
		{
			get { return this._updated; }
			set { this._updated = value; }
		}

		/// <summary>
		/// Created Time stamp
		/// </summary>
		public DateTime DateCreated
		{
			get { return this._created; }
			set { this._created = value; }
		}

		#endregion
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public GalleryComment()
		{
			this._id = -1;
			this._created = DateTime.Now;
		}

	}
}
