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
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

using Cuyahoga.Web.UI;
using Cuyahoga.Web.Util;
using Cuyahoga.Modules.Gallery.Domain;
using Cuyahoga.Core.Service.SiteStructure;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for EditGallery.
	/// </summary>
	public class EditGallery : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.Panel pnlInfo;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvName;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvTitle;
		protected System.Web.UI.WebControls.TextBox txtArtist;
		protected System.Web.UI.WebControls.TextBox txtVpath;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfvVpath;
		
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnDelete;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		protected System.Web.UI.WebControls.Panel pnlDesciption;
		protected System.Web.UI.WebControls.Panel pnlPhotos;
		protected System.Web.UI.WebControls.Repeater rptDesciptions;

		protected System.Web.UI.WebControls.TextBox txtVirtualPath;
		protected System.Web.UI.WebControls.CheckBox cbInclude;
		protected System.Web.UI.WebControls.TextBox txtSequence;
		protected System.Web.UI.WebControls.RegularExpressionValidator revSequence;

		protected ImageManager imgManager;

		private GalleryModule _module;
		private PhotoGallery _gallery;
		private Hashtable _htdescriptions = new Hashtable();
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
			this.btnCancel.Attributes.Add("onClick", String.Format("document.location.href='AdminGalleries.aspx{0}'", base.GetBaseQueryString()));
			
			this.imgManager.EditorHeight = 200;
			this.imgManager.EditorWidth = 200;

			GetActualCulture();
			
			if (Request.QueryString["GalleryId"] != null)
			{
				int galleryId = Int32.Parse(Request.QueryString["GalleryId"]);
				if (galleryId > 0)
				{
					this._gallery = this._module.GetGallery( galleryId );
					if (! this.IsPostBack)
					{
						BindGallery();
					}
					this.btnDelete.Visible = true;
					this.btnDelete.Attributes.Add("onClick", "return confirm('Are you sure?');");
				}
			}
	
		}
		
		private void BindGallery()
		{
			this.cbInclude.Checked = _gallery.Include;
			this.txtSequence.Text = _gallery.Sequence.ToString();
			this.txtName.Text = _gallery.Name;
			this.txtTitle.Text = _gallery.Title;
			this.txtArtist.Text = _gallery.Artist;
			this.txtVirtualPath.Text = _gallery.VirtualPath;
			this.imgManager.AddImage( _gallery.ThumbImage );
			this.BindDescriptions();
		}

		private void BindDescriptions()
		{
			// Bind descriptions thru temp datatable
			Hashtable descriptions = _module.GetObjectDescriptions( DescriptionType.Gallery, _gallery.Id );
			DataTable dt = new DataTable();
			dt.Columns.Add( "Culture", typeof( string ) );
			dt.Columns.Add( "Descriptions", typeof( string ) );

			foreach( Descriptions d in descriptions.Values )
			{
				_htdescriptions[ d.Culture ] =   GalleryHelpers.ResetDescription( d.Description );
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

		private void MapInformation()
		{
			_gallery.Include = this.cbInclude.Checked;
			_gallery.Sequence = Int32.Parse( txtSequence.Text );
			_gallery.Name = txtName.Text;
			_gallery.Title = this.txtTitle.Text;
			_gallery.Artist= this.txtArtist.Text;
			_gallery.VirtualPath = this.txtVirtualPath.Text;
			_gallery.DateUpdated = DateTime.Now;

			string[] img = imgManager.GetImages();
			if ( img.Length > 0 )
			{
				_gallery.ThumbImage = img[0];
			}
		}

		private Hashtable MapDescriptions()
		{
			Hashtable ht = _module.GetObjectDescriptions( DescriptionType.Gallery, _gallery.Id );

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
						d.ObjectUID = _gallery.Id;
						d.TargetClass = DescriptionType.Gallery.ToString();
						d.Description = GalleryHelpers.ClearDescription( Request.Form[ key ] );
						d.DateCreated = DateTime.Now;
						d.DateUpdated = DateTime.Now;
						ht.Add( key, d );
					}
				}
			}
			return ht;
		}
		
		private void GetActualCulture()
		{
			IList rootNodes = this._nodeService.GetRootNodes(base.Node.Site);
			foreach (Node node in rootNodes)
			{
				// CultureInfo ci = new CultureInfo(node.Culture);
				_htdescriptions.Add( node.Culture, String.Empty );
			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if ( _gallery == null ) 
				{
					_gallery = new PhotoGallery();
					_gallery.Owner = (Cuyahoga.Core.Domain.User)this.User.Identity;
				}

				this.MapInformation();
				_module.SaveGallery( _gallery );
				
				Hashtable ht = this.MapDescriptions();
				_module.SaveObjectDescriptions( ht );
			
				Response.Redirect(String.Format("AdminGalleries.aspx{0}", base.GetBaseQueryString()));
			}
			catch (Exception ex )
			{
				//base.ShowError( ex.Message );
				throw ex;
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if (this._gallery != null)
			{
				try
				{
					int galleryid = this._gallery.Id;
					this._module.DeleteGallery(this._gallery);
					Response.Redirect(String.Format("AdminGalleries.aspx{0}", base.GetBaseQueryString()));
				}
				catch (Exception ex)
				{
					ShowError(ex.Message);
				}
			}
			else
			{
				ShowError("No article found to delete");
			}

		}
	}
}
