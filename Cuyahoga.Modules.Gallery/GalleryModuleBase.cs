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
using System.Text.RegularExpressions;

using NHibernate;
using NHibernate.Expression;
using NHibernate.Type;

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Communication;

using Cuyahoga.Modules.Gallery.Domain;
using Cuyahoga.Core.Service.SiteStructure;

namespace Cuyahoga.Modules.Gallery
{
	/// <summary>
	/// Main management module used by all "visual components"
	/// </summary>
	public class GalleryModuleBase : ModuleBase, INHibernateModule
	{
		protected int _galleryId = -1;
		protected int _photoId = -1;

		protected GalleryModuleAction _currentAction = GalleryModuleAction.None;

		public GalleryModuleAction CurrentAction
		{
			get { return _currentAction; }
		}
		
		public int CurrentGalleryId
		{
			get { 	return _galleryId; }
		}

		public int CurrentPhotoId
		{
			get { return _photoId; }
		}
		
		/// <summary>
		/// Default constrcutor we need to register our domain class
		/// </summary>
		/// <param name="section"></param>
		public GalleryModuleBase()
		{
			this._galleryId = -1;
			this._photoId = -1;
		}

		#region Gallery management
		/// <summary>
		/// Return a Gallery object by Id
		/// </summary>
		public PhotoGallery GetGallery(int galleryId)
		{
			try
			{
				return (PhotoGallery)base.NHSession.Load(typeof(PhotoGallery), galleryId);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Gallery", ex);
			}
		}

		public PhotoGallery GetGallery(string name)
		{
			try
			{
				string hql = "from Gallery g where g.Name = :name";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetString("name", name.ToUpper());
				IList list = q.List();
				if ( list.Count > 0 )
				{
					return list[0] as PhotoGallery;
				}
				else
				{
					return null;
				}

			}
			catch (Exception ex )
			{
				throw new Exception("unable to get Gallery", ex );
			}
		}

		public GallerySection GetGallerySection( int sectionid )
		{	
			try
			{
				string hql = "from GallerySection g where g.SectionId = :sectionId";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("sectionId", sectionid);
				IList list = q.List();
				if ( list.Count > 0 )
				{
					return list[0] as GallerySection;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get GalleryId", ex);
			}
		}
		public PhotoGallery GetGalleryBySection(int sectionid)
		{
			try
			{	
				GallerySection gs = this.GetGallerySection( sectionid );
				if ( gs != null )
				{
					return GetGallery( gs.GalleryId );
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Gallery", ex);
			}
		}

		public IList GetAllGalleries()
		{
			try
			{
				string hql = "from Gallery g order by g.Sequence";
				return base.NHSession.Find(hql);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Gallery", ex);
			}
		}

		public IList GetIncludedGalleries()
		{
			try
			{
				string hql = "from Gallery g where g.Include = :include order by g.Sequence";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetBoolean( "include", true );
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get GalleryId", ex);
			}
		}
		
		public IList GetAllSections( int galleryid )
		{
			try
			{
				string hql = "from GalerySection g where g.GalleryId = :galleryid";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("galleryid", galleryid );
				return q.List();
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get GalleryId", ex);
			}
		}

		public void SaveGallery(PhotoGallery gallery)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (gallery.Id == -1)
				{
					gallery.DateUpdated = DateTime.Now;
					base.NHSession.Save(gallery);
				}
				else
				{
					base.NHSession.Update(gallery);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Gallery", ex);
			}
		}

		public void DeleteGallery(PhotoGallery gallery)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				int galleryid = gallery.Id;
				base.NHSession.Delete(gallery);
				tx.Commit();
				this.DeleteObjectDescriptions( DescriptionType.Gallery, galleryid );
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Gallery", ex);
			}
		}

		public void SaveGallerySection( int galleryid, int sectionid )
		{
			GallerySection gs = this.GetGallerySection( sectionid );
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (gs == null)
				{
					gs = new GallerySection();
					gs.GalleryId = galleryid;
					gs.SectionId = sectionid;
					base.NHSession.Save(gs);
				}
				else
				{
					gs.GalleryId = galleryid;
					gs.SectionId = sectionid;
					base.NHSession.Update(gs);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save GallerySection", ex);
			}
		}
		public void DeleteGallerySection( int galleryid, int sectionid )
		{
			GallerySection gs = this.GetGallerySection( sectionid );
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(gs);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete GallerySection", ex);
			}
		}

		#endregion

		#region Photo management

		public Photo GetPhoto( int photoid )
		{
			try
			{
				return (Photo)base.NHSession.Load(typeof(Photo), photoid);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Photo", ex);
			}
		}

		public void SavePhoto( Photo photo )
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (photo.Id == -1)
				{
					photo.DateUpdated = DateTime.Now;
					base.NHSession.Save(photo);
				}
				else
				{
					base.NHSession.Update(photo);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Photo", ex);
			}
		}

		public void DeletePhoto( Photo photo )
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(photo);
				tx.Commit();
				Hashtable hd = this.GetObjectDescriptions( DescriptionType.Photo, photo.Id );
				this.DeleteObjectDescriptions( hd );
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Photo", ex);
			}
		}

		#endregion

		#region Comment management

		public void SaveComment(GalleryComment comment)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				if (comment.Id == -1)
				{
					comment.DateUpdated = DateTime.Now;
					base.NHSession.Save(comment);
				}
				else
				{
					base.NHSession.Update(comment);
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Comment", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comment"></param>
		public void DeleteComment(GalleryComment comment)
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(comment);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete Comment", ex);
			}
		}
		
		#endregion

		#region Descriptions management

		public string GetObjectDescriptionText( DescriptionType objecttype, int objectid, string culture )
		{
			Descriptions d = GetObjectDescriptions(objecttype, objectid, culture );
			return d.Description;
		}
		
		public Descriptions GetObjectDescriptions( DescriptionType objecttype, int objectid, string culture )
		{
			Hashtable ht = this.GetObjectDescriptions( objecttype, objectid );
			if ( ht[ culture ] != null ) return ht[ culture ] as Descriptions;
			if ( false == culture.StartsWith("en") )
			{
				if ( ht[ "en-US" ] != null ) return ht[ "en-US" ] as Descriptions;
			}
			Descriptions d = new Descriptions();
			d.Culture = "en-US";
			d.Description = "";
			return d;
		}

		public Hashtable GetObjectDescriptions( DescriptionType objecttype, int objectid )
		{
			string t = objecttype.ToString().ToUpper();

			Hashtable ht = new Hashtable();
			try
			{
				string hql = "from Descriptions d where d.ObjectUID = :objectid and d.TargetClass = :objectClass";
				IQuery q = base.NHSession.CreateQuery(hql);
				q.SetInt32("objectid", objectid );
				q.SetString("objectClass", t );
				
				IList list = q.List();
				foreach( Descriptions d in list )
				{
					ht.Add( d.Culture, d );
				}
				return ht;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to get Descriptions", ex);
			}
		}

		public void SaveObjectDescriptions( Hashtable descriptions )
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				foreach( Descriptions d in descriptions.Values)
				{
					if (d.Id == -1)
					{
						d.DateUpdated = DateTime.Now;
						base.NHSession.Save(d);
						
					}
					else
					{
						base.NHSession.Update(d);
					}
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to save Descriptions", ex);
			}
		}

		private void DeleteObjectDescriptions( Hashtable descriptions )
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				foreach( Descriptions d in descriptions.Values)
				{
					base.NHSession.Delete( d );
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to Delete Descriptions", ex);
			}
		}

		private void DeleteObjectDescriptions( DescriptionType objecttype, int objectid )
		{
			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				Hashtable ht = this.GetObjectDescriptions( objecttype, objectid );
				foreach( Descriptions d in ht.Values)
				{
					base.NHSession.Delete( d );
				}
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to Delete Descriptions", ex);
			}
		}

		#endregion

		public override void DeleteModuleContent()
		{
			GallerySection gs = this.GetGallerySection( base.Section.Id );
			if ( gs == null ) return;

			ITransaction tx = base.NHSession.BeginTransaction();
			try
			{
				base.NHSession.Delete(gs);
				tx.Commit();
			}
			catch (Exception ex)
			{
				tx.Rollback();
				throw new Exception("Unable to delete GallerySection", ex);
			}
		}

		protected override void ParsePathInfo()
		{
			base.ParsePathInfo();
			string[] actions = Enum.GetNames(typeof(GalleryModuleAction));

			if (base.ModuleParams.Length > 0)
			{
				// First pathinfo parameter is the module action.
				// Second pathinfo is always the galeeryid
				// Third (if present) is the photoid
				try
				{
					for ( int i = 0; i < base.ModuleParams.Length; i++)
					{
						if ( base.ModuleParams[ i ].Length > 0 )
						{
						
							string str = base.ModuleParams[ i ].ToLower();

							// check if we have a parameter for us
							int k;
							for ( k = 0; k < actions.Length; k++)
							{
								if (str == actions[ k ].ToLower() ) break;
							}

							// we expect two parameters
							if ( k < actions.Length - 1 )
							{
								this._currentAction = (GalleryModuleAction)Enum.Parse(typeof(GalleryModuleAction), str, true );
								this._galleryId = Int32.Parse(base.ModuleParams[i+1]);
								if ( k  < actions.Length - 2 )
								{
									this._photoId = Int32.Parse(base.ModuleParams[i + 2 ]);
								}
							}
						}
					}
				}
				
				catch (ArgumentException ex)
				{
					throw new Exception("Unable to parse GalleryModuleAction", ex );
				}
				catch (Exception ex)
				{
					throw new Exception("Error when parsing module parameters: " + base.ModulePathInfo, ex);
				}
			}
		}

		protected bool IsValidSettings( object setting )
		{
			if ( setting == null ) return false;
			if ( setting is string )
			{
				return ( (string)setting != String.Empty );
			}
			return false;
		}
	}

	public enum DescriptionType
	{
		Photo,
		Gallery
	}

	public enum GalleryModuleAction
	{
		None,
		ViewGallery,
		ViewGalleryComments
	}
}
