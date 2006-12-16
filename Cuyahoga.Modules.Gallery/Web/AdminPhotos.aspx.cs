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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Gallery.Domain;

using xprience.ImageLib;
using Cuyahoga.Core.Service.SiteStructure;


namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for AdminPhotos.
	/// </summary>
	public class AdminPhotos : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.TextBox txtSerie;
		protected System.Web.UI.WebControls.TextBox txtCategory;
		protected System.Web.UI.WebControls.TextBox txtOriginalSize;
		protected System.Web.UI.WebControls.Literal litGalleryName;

		protected ImageManager imgManager1;
		protected ImageManager imgManager2;
		protected ImageManager imgManager3;
		protected ImageTable imgTable;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnNew;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnBack;
		protected System.Web.UI.WebControls.Repeater rptDesciptions;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnOrganize;
		protected System.Web.UI.WebControls.CheckBox cbgenThumb;
		protected System.Web.UI.WebControls.CheckBox cbRatio;
		protected System.Web.UI.WebControls.TextBox txtWidth;
		protected System.Web.UI.WebControls.TextBox txtHeight;
		protected System.Web.UI.HtmlControls.HtmlInputButton brnGenerate;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnWatermark;
		protected System.Web.UI.WebControls.TextBox txtWatermark;
		protected System.Web.UI.WebControls.TextBox txtFontsize;
		protected System.Web.UI.WebControls.Panel pnlImageProcessing;

		private GalleryModule _module;
		private PhotoGallery _gallery = null;
		private Photo _photo = null;
		private Hashtable _htdescriptions = new Hashtable();

		private ImageProcessingConfig _processingconfig;
		private INodeService _nodeService;

		/// <summary>
		/// NodeService (injected)
		/// </summary>
		public INodeService NodeService
		{
			set { this._nodeService = value; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			_module = base.Module as GalleryModule;
			this.btnBack.Attributes.Add("onClick", String.Format("document.location.href='AdminGalleries.aspx{0}'", base.GetBaseQueryString()));
			this.btnDelete.Enabled = false;
			pnlImageProcessing.Enabled = false;
			
			this.imgManager1.EditorHeight = 200;
			this.imgManager1.EditorWidth = 200;
			this.imgManager2.EditorHeight = 200;
			this.imgManager2.EditorWidth = 200;
			this.imgManager3.EditorHeight = 700;
			this.imgManager3.EditorWidth = 400;

			string file = Server.MapPath( base.ResolveUrl("~/modules/Gallery/config/ImageProcessing.xml") );
			_processingconfig = ImageProcessingConfig.Instance( file );

			/// GetActualCulture();

			if (Request.QueryString["GalleryId"] != null )
			{
				int galleryId = Int32.Parse(Request.QueryString["GalleryId"]);
				this._gallery = this._module.GetGallery( galleryId );
				this.litGalleryName.Text = _gallery.Title;
				this.btnNew.Attributes.Add("onClick", String.Format("document.location.href='AdminPhotos.aspx{0}&GalleryId={1}&PhotoId={2}'", base.GetBaseQueryString(), _gallery.Id, 0));
				if ( _gallery.Photos.Count > 1 )
				{
					this.btnOrganize.Disabled = false;
					this.btnOrganize.Attributes.Add("OnClick", String.Format("window.open('ImageOrganizer.aspx?GalleryId={0}','organizer');",_gallery.Id ));
				}

				GetActualCulture();
				BindImageList();

				if (Request.QueryString["PhotoId"] != null )
				{
					int photoId = Int32.Parse(Request.QueryString["PhotoId"]);
					if ( photoId > 0 )
					{
						_photo = _module.GetPhoto( photoId );
						this.btnDelete.Enabled = true;
						this.pnlImageProcessing.Enabled = true;
						this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
						if ( false == Page.IsPostBack ) 
						{ 
							BindPhoto(); 
							BindSettings();
						}
					}
					else
					{
						if ( false == Page.IsPostBack ) 
						{
							ClearPhoto();
							BindSettings();
						}
					}
				}
			}
		}

		private void BindSettings()
		{
			this.cbgenThumb.Checked = _processingconfig.AutoGenerateThumb;
			this.cbRatio.Checked = _processingconfig.MaintainAspectRatio;
			this.txtWidth.Text = _processingconfig.ThumbnailWidth.ToString();
			this.txtHeight.Text = _processingconfig.ThumbnailHeight.ToString();
			this.txtWatermark.Text = _processingconfig.CopyrightText;
			this.txtFontsize.Text = _processingconfig.FontSize.ToString();
		}
		
		private void BindImageList()
		{
			imgTable.Clear();
			imgTable.NumberofCols = 8;

			foreach( Photo p in _gallery.Photos )
			{
				ImageCell cell = new ImageCell();
				HtmlImage img = new HtmlImage();
				img.Border=0;
				img.Src = p.ThumbImage;
				img.Attributes.Add( "OnClick", String.Format("document.location.href='AdminPhotos.aspx{0}&GalleryId={1}&PhotoId={2}'; return false;", base.GetBaseQueryString(), _gallery.Id, p.Id));
				img.Attributes.Add( "OnMouseOver", "this.style.cursor='pointer'");
				cell.AddImage( img );
				cell.save();
				imgTable.AddCell( cell );
			}
		}

		private void BindPhoto()
		{
			txtCategory.Text = _photo.Category;
			txtOriginalSize.Text = _photo.OriginalSize;
			txtSerie.Text = _photo.Serie;
			txtTitle.Text = _photo.Title;
			imgManager1.AddImage( _photo.ThumbImage );
			imgManager2.AddImage( _photo.ThumbOver );
			imgManager3.AddImage( _photo.LargeImage );
			this.BindDescriptions();
		}

		private void ClearPhoto()
		{
			txtTitle.Text = "";
			imgManager1.Reset();
			imgManager2.Reset();
			imgManager3.Reset();
		}

		private void MapInformation()
		{
			_photo.Category = txtCategory.Text;
			_photo.DownloadAllowed = false;
			_photo.Gallery =_gallery;

			string[] imgs;
			imgs = imgManager1.GetImages();
			if ( imgs.Length > 0 ) _photo.ThumbImage = imgs[0];
			imgs = imgManager2.GetImages();
			if ( imgs.Length > 0 ) _photo.ThumbOver = imgs[0];
			imgs = imgManager3.GetImages();
			if ( imgs.Length > 0 ) _photo.LargeImage = imgs[0];

			_photo.OriginalSize = txtOriginalSize.Text;
			_photo.Serie = txtSerie.Text;
			_photo.Title = txtTitle.Text;
			_photo.DateUpdated = DateTime.Now;
		}

		private void GetActualCulture()
		{
			IList rootNodes = this._nodeService.GetRootNodes(base.Node.Site);
			foreach (Node node in rootNodes)
			{
				// CultureInfo ci = new CultureInfo(node.Culture);
				_htdescriptions.Add(node.Culture, String.Empty);
			}
		}

		private void BindDescriptions()
		{
			// Bind descriptions thru temp datatable
			Hashtable descriptions = _module.GetObjectDescriptions( DescriptionType.Photo, _photo.Id );
			DataTable dt = new DataTable();
			dt.Columns.Add( "Culture", typeof( string ) );
			dt.Columns.Add( "Descriptions", typeof( string ) );

			foreach( Descriptions d in descriptions.Values )
			{
				_htdescriptions[ d.Culture ] = d.Description;
			}

			foreach ( string key in _htdescriptions.Keys )
			{
				DataRow dr = dt.NewRow();
				dr[ 0 ] = key;
				dr[ 1 ] = _htdescriptions[ key ];
				dt.Rows.Add( dr );
			}

			rptDesciptions.DataSource = dt;
			rptDesciptions.DataBind();
		}
		
		private Hashtable MapDescriptions()
		{
			Hashtable ht = _module.GetObjectDescriptions( DescriptionType.Photo, _photo.Id );

			foreach( string key in Request.Form.Keys )
			{
				if ( _htdescriptions.ContainsKey( key ) )
				{
					if ( ht.ContainsKey( key ) )
					{
						Descriptions d = (Descriptions) ht[ key ];
						d.Description = GalleryHelpers.ClearDescription( Request.Form[ key ] );
						d.DateUpdated = DateTime.Now;
					}
					else
					{
						Descriptions d = new Descriptions();
						d.Culture = key;
						d.ObjectUID = _photo.Id;
						d.TargetClass = DescriptionType.Photo.ToString();
						d.Description = GalleryHelpers.ClearDescription( Request.Form[ key ] );
						d.DateCreated = DateTime.Now;
						d.DateUpdated = DateTime.Now;
						ht.Add( key, d );
					}
				}
			}
			return ht;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.brnGenerate.ServerClick += new System.EventHandler(this.brnGenerate_ServerClick);
			this.btnWatermark.ServerClick += new System.EventHandler(this.btnWatermark_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (false == Page.IsValid) return;
			if ( imgManager3.GetImages().Length < 1) 
			{
				base.ShowError( "You must attach a large  image");
				return;
			}

			try
			{
				if ( _photo == null ) 
				{
					_photo = new Photo();
					_photo.DateCreated = DateTime.Now;
					_photo.Sequence = _gallery.Photos.Count;
					this.MapInformation();
					_module.SavePhoto( _photo );
					_gallery.Photos.Add( _photo );
					this.BindImageList();
				}
				else
				{
					this.MapInformation();
					_module.SavePhoto( _photo );
					Hashtable ht = this.MapDescriptions();
					_module.SaveObjectDescriptions( ht );
					this.BindDescriptions();
				}
			}

			catch (Exception ex )
			{
				//base.ShowError( ex.Message );
				throw ex;
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			Photo todelete = null;
			foreach( Photo p in _gallery.Photos )
			{
				if ( p.Id == _photo.Id ) 
				{
					todelete = p;
					break;
				}
			}
			_gallery.Photos.Remove( todelete );
			_module.DeletePhoto( _photo );

			string url = String.Format("AdminPhotos.aspx{0}&GalleryId={1}", base.GetBaseQueryString(), _gallery.Id);
			Context.Response.Redirect( url );
		}

		private void brnGenerate_ServerClick(object sender, System.EventArgs e)
		{
			// validate generate settings
			if ( this.cbgenThumb.Checked )
			{
				bool maintain = this.cbRatio.Checked;
				int width = Int32.Parse( this.txtWidth.Text );
				int height = Int32.Parse( this.txtHeight.Text);

				string source = Server.MapPath( _photo.LargeImage );
				FileInfo info = new FileInfo( source );
				string target = info.DirectoryName + "\\" + info.Name.Substring(0, info.Name.LastIndexOf(".") ) + "_thumb" + info.Extension;
				ThumbnailHelper helper = new ThumbnailHelper();
				helper.GetThumb( source, target, width, height, maintain );
				
				string thumb = _photo.LargeImage.Substring(0,_photo.LargeImage.LastIndexOf("/") + 1 );
				thumb += info.Name.Substring(0, info.Name.LastIndexOf(".") )  + "_thumb" + info.Extension;

				_photo.ThumbImage = thumb;
				_module.SavePhoto( _photo );

				this.imgManager1.Reset();
				this.imgManager1.AddImage( thumb );
			}
			else
			{
				base.ShowError("You must check the generate checkbox");
			}
		}

		private void btnWatermark_ServerClick(object sender, System.EventArgs e)
		{
			string copyright = this.txtWatermark.Text;
			string fontfamily = _processingconfig.FontFamilly;
			int fontsize = Int32.Parse( this.txtFontsize.Text );
			string image = _processingconfig.WatermarkImage;
			
			string source = Server.MapPath( _photo.LargeImage );
			FileInfo info = new FileInfo( source );
			string target = info.DirectoryName + "\\" + info.Name.Substring(0, info.Name.LastIndexOf(".") ) + "_wm" + info.Extension;
			
			WaterMark.SetWaterMark( source, target, copyright, fontfamily, fontsize );

			string thumb = _photo.LargeImage.Substring(0,_photo.LargeImage.LastIndexOf("/") + 1 );
			thumb += info.Name.Substring(0, info.Name.LastIndexOf(".") )  + "_wm" + info.Extension;
			
			_photo.LargeImage = thumb;
			_module.SavePhoto( _photo );

			this.imgManager3.Reset();
			this.imgManager3.AddImage( thumb );
		}

	}
}
