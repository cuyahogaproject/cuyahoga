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

using Cuyahoga.Core.Domain;


namespace Cuyahoga.Modules.Gallery.Domain
{
	/// <summary>
	/// Summary description for Gallery.
	/// </summary>
	public class PhotoGallery
	{
		
		private int _id;
		private string _title;
		private string _name;
		private string _thumb;
		private string _virtualpath;
		private string _artist;
		private DateTime _created;
		private DateTime _updated;
		private Cuyahoga.Core.Domain.User _owner;
		private string _currentDescription;
		private bool _include = true;
		private int _sequence = 0;

		private IList _comments;
		private IList _photos;

		private SortedList _sort;
		
		#region properties

		/// <summary>
		/// Unique ID
		/// </summary>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Gallery Short name for referencing
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name= value.ToUpper(); }
		}

		/// <summary>
		/// Title of the Gallery
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		/// Reference to the thumbnail image
		/// </summary>
		public string ThumbImage
		{
			get { return _thumb; }
			set { _thumb = value; }
		}

		/// <summary>
		/// Virtual Path where inages are stored
		/// </summary>
		public string VirtualPath
		{
			get { return _virtualpath; }
			set { _virtualpath= value; }
		}

		/// <summary>
		/// Name of the Artist
		/// </summary>
		public string Artist
		{
			get { return _artist; }
			set { _artist= value; }
		}

		/// <summary>
		/// Is the Gallery to be included in the main list
		/// </summary>
		public bool Include
		{
			get { return _include; }
			set { _include= value; }
		}

		/// <summary>
		/// Is the Gallery to be included in the main list
		/// </summary>
		public int Sequence
		{
			get { return _sequence; }
			set { _sequence = value; }
		}

		/// <summary>
		/// Creation timestamp
		/// </summary>
		public DateTime DateCreated
		{
			get { return _created; }
			set { _created= value; }
		}

		/// <summary>
		/// Modification timestamp
		/// </summary>
		public DateTime DateUpdated
		{
			get { return _updated; }
			set { _updated= value; }
		}

		/// <summary>
		/// Reference to the registered user as the owner
		/// </summary>
		public Cuyahoga.Core.Domain.User Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}
		
		/// <summary>
		/// List of comments
		/// </summary>
		public IList GalleryComments
		{
			get { return _comments; }
			set { _comments= value; }
		}

		/// <summary>
		/// List of photes
		/// </summary>
		public IList Photos
		{
			get { return _photos; }
			set { _photos= value; }
		}
		
		public SortedList SortedPhotos
		{
			get 
			{
				if ( _sort == null ) _sort = new SortedList();
				return Sort();
			}
		}

		/// <summary>
		/// Set or get the current description according the current culture (helper property)
		/// </summary>
		public string CurrentDescription
		{
			get { return _currentDescription; }
			set { _currentDescription = value; }
		}

		#endregion
		
		#region constructor

		public PhotoGallery()
		{
			this._id = -1;
			this._comments = new ArrayList();
			this._photos = new ArrayList();
			this._created = DateTime.Now;
			this._updated = DateTime.Now;
		}

		#endregion

		/// <summary>
		/// Calculate the average gallery rating and visits baed on each photo visit and rating
		/// </summary>
		/// <returns>an GalleryAverage object</returns>
		public  GalleryAverage AverageData()
		{
			long sumViews = 0;
			long sumVotes = 0;
			decimal avgRate = 0;
			int photowithVote = 0;
			foreach( Photo p in _photos )
			{
				sumVotes += p.TotalVotes;
				photowithVote += p.TotalVotes > 0 ?  1 : 0;
				sumViews += p.Views;
				avgRate += p.GetRating();
			}
			decimal rating = photowithVote > 0 ? avgRate / photowithVote : 0;
			GalleryAverage avg = new GalleryAverage( sumViews , sumVotes,  photowithVote, rating );
			return avg;
		}

		// Resort the photos in the collection when editor as reorganize
		private SortedList Sort()
		{
			_sort.Clear();
			foreach( Photo p in _photos )
			{
				_sort.Add( p.Sequence, p );
			}
			return _sort;
		}
	
	}

	/// <summary>
	/// Simple strcuture to pass gallery average visit and pgoto ratings
	/// </summary>
	public struct GalleryAverage
	{
		private long _visit;
		private long _votes;
		private int _withvode;
		private decimal _rate;

		/// <summary>
		/// Number of visits
		/// </summary>
		public long Visits
		{
			get { return _visit; }
		}

		/// <summary>
		/// Number of votes
		/// </summary>
		public long Votes
		{
			get { return _votes; }
		}

		/// <summary>
		/// Number of Photos with votes
		/// </summary>
		public int PhotoVoted
		{
			get { return _withvode; }
		}

		/// <summary>
		/// Average ratings
		/// </summary>
		public decimal Rating
		{
			get { return _rate; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="visit">visit</param>
		/// <param name="rate">rate</param>
		public GalleryAverage( long visit, long vote, int photovoted, decimal rate )
		{
			_visit = visit;
			_votes = vote;
			_withvode = photovoted;
			_rate = rate;
		}
	}
}
