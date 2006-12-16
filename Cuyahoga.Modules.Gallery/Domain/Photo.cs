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
using System.Collections;

namespace Cuyahoga.Modules.Gallery.Domain
{
	/// <summary>
	/// A photo object
	/// </summary>
	public class Photo
	{
		
		private int _id;
		private PhotoGallery _gallery;
		private int _seq;
		private long _views;
		private long _downloads;
		private string _title;
		private string _thumb;
		private string _thumbover;
		private string _image;
		private string _category;
		private string _serie;
		private string _size;
		private bool _downloadallowed;
		private DateTime _createdl;
		private DateTime _updated;
		private long _rank1;
		private long _rank2;
		private long _rank3;
		private long _rank4;
		private long _rank5;
		private long _rank6;
		private long _rank7;
		private long _rank8;
		private long _rank9;

		#region properties
		
		/// <summary>
		/// Photo unique ID
		/// </summary>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Reference to parent GAllery
		/// </summary>
		public PhotoGallery Gallery
		{
			get { return _gallery; }
			set { _gallery = value; }
		}
		
		/// <summary>
		/// Seq in the gallery of the photo
		/// </summary>
		public int Sequence
		{
			get { return _seq; }
			set { _seq = value; }
		}

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		/// Number of views (for large imange)
		/// </summary>
		public long Views
		{
			get { return _views; }
			set { _views = value; }
		}

		/// <summary>
		/// Number of downloads (for large imange)
		/// </summary>
		public long Downloads
		{
			get { return _downloads; }
			set { _downloads = value; }
		}

		/// <summary>
		/// Thumbnail image referrence
		/// </summary>
		public string ThumbImage
		{
			get { return _thumb; }
			set { _thumb = value; }
		}

		/// <summary>
		/// Thumbnail Mouse Over image referrence (optional)
		/// </summary>
		public string ThumbOver
		{
			get { return _thumbover; }
			set { _thumbover = value; }
		}

		/// <summary>
		/// Large image referrence
		/// </summary>
		public string LargeImage
		{
			get { return _image; }
			set { _image = value; }
		}

		/// <summary>
		/// Photo is associated to a specific serie
		/// </summary>
		public string Serie
		{
			get { return _serie; }
			set { _serie = value; }
		}

		/// <summary>
		/// Photo has some categories
		/// </summary>
		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		/// <summary>
		/// Original size
		/// </summary>
		public string OriginalSize
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Creation time stam,p
		/// </summary>
		public DateTime DateCreated
		{
			get { return _createdl; }
			set { _createdl = value; }
		}

		/// <summary>
		/// Update timestamp
		/// </summary>
		public DateTime DateUpdated
		{
			get { return _updated; }
			set { _updated = value; }
		}


		/// <summary>
		/// Can the user download the image
		/// </summary>
		public bool DownloadAllowed
		{
			get { return _downloadallowed; }
			set { _downloadallowed = value; }
		}

		/// <summary>
		/// Rank 1 (-100)
		/// </summary>
		public long Rank1
		{
			get { return _rank1; }
			set {_rank1 = value; }
		}
		/// <summary>
		/// Rank 2 (-75)
		/// </summary>
		public long Rank2
		{
			get { return _rank2; }
			set {_rank2 = value; }
		}
		/// <summary>
		/// Rank 3 (-50)
		/// </summary>
		public long Rank3
		{
			get { return _rank3; }
			set {_rank3 = value; }
		}

		/// <summary>
		/// Rank 4 (-25)
		/// </summary>
		public long Rank4
		{
			get { return _rank4; }
			set {_rank4 = value; }
		}
		
		/// <summary>
		/// Rank 5 (0)
		/// </summary>
		public long Rank5
		{
			get { return _rank5; }
			set {_rank5 = value; }
		}

		/// <summary>
		/// Rank 6 (25)
		/// </summary>
		public long Rank6
		{
			get { return _rank6; }
			set {_rank6 = value; }
		}

		/// <summary>
		/// Rank 7 (50)
		/// </summary>
		public long Rank7
		{
			get { return _rank7; }
			set {_rank7 = value; }
		}

		/// <summary>
		/// Rank 8 (75)
		/// </summary>
		public long Rank8
		{
			get { return _rank8; }
			set {_rank8 = value; }
		}

		/// <summary>
		/// Rank 9 (100)
		/// </summary>
		public long Rank9
		{
			get { return _rank9; }
			set {_rank9 = value; }
		}


		/// <summary>
		/// Read-only. The total number of votes that have been submitted.
		/// </summary>
		public long TotalVotes
		{
			get
			{
				return _rank1 + _rank2 + _rank3 + _rank4 + _rank5 + _rank6 + _rank7 + _rank8 + _rank9;
			}
		}

		#endregion

		#region methods

		/// <summary>
		/// This method returns the average rating, given the previous rating values.
		/// </summary>
		/// <returns>decimal</returns>
		public decimal GetRating()
		{
			long sumValues = (_rank1 ) + (_rank2 * 2) + (_rank3 * 3) + (_rank4 * 4) + (_rank5 * 5) + (_rank6 * 6) + (_rank7 * 7) + (_rank8 * 8) + (_rank9 * 9);
			long totalVotes = _rank1 + _rank2 + _rank3 + _rank4 + _rank5 + _rank6 + _rank7 + _rank8 + _rank9;

			if (totalVotes == 0) return 0M;
			decimal rank = Math.Round( (decimal) sumValues / (decimal) totalVotes, 2);
			
			return rank;
		}

		#endregion

		#region constructor

		public Photo()
		{
			_id = -1;
			_views = 0;
			_downloads = 0;
			_rank1 = 0;
			_rank2 = 0;
			_rank3 = 0;
			_rank4 = 0;
			_rank5 = 0;
			_rank6 = 0;
			_rank7 = 0;
			_rank8 = 0;
			_rank9 = 0;
			_downloadallowed = false;
		}

		#endregion


	}
}
