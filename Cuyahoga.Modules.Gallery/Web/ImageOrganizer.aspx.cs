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

using Cuyahoga.Modules.Gallery.Domain;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for ImageOrganizer.
	/// </summary>
	public class ImageOrganizer : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hgalleryid;
	
		protected ImageTable imgTable;
		private GalleryModuleBase _module;
		protected System.Web.UI.WebControls.Label lblmessage;
		private PhotoGallery _gallery;

		private void Page_Load(object sender, System.EventArgs e)
		{
			_module = new GalleryModuleBase();
			int galleryId = -1;

			if ( false == Page.IsPostBack )
			{
				if (Request.QueryString["GalleryId"] != null )
				{
					galleryId = Int32.Parse(Request.QueryString["GalleryId"]);
					this._gallery = this._module.GetGallery( galleryId );

					BindImageList();

					this.hgalleryid.Value = galleryId.ToString();
				}
				else
				{
					Response.End();
				}
			}
			// Page is post back
			else
			{
				if ( Request.Form["hgalleryid"] != null )
				{
					galleryId = Int32.Parse( Request.Form["hgalleryid"] );
					this._gallery = this._module.GetGallery( galleryId );
					Hashtable ht = new Hashtable();
					foreach( string key in Request.Form.Keys)
					{
						if ( key.StartsWith("hd") )
						{
							string s = Request.Form[ key ];
							int pid = Int32.Parse( s.Substring(2) );
							int pos = Int32.Parse( key.Substring(2) );
							ht.Add( pid, pos);
						}
					}
					foreach( Photo p in _gallery.Photos)
					{
						int pos = (int)ht[ p.Id ];
						if ( pos != p.Sequence )
						{
							p.Sequence = pos;
							_module.SavePhoto( p );
						}
					}
					this._gallery = this._module.GetGallery( galleryId );
					BindImageList();
					this.lblmessage.Text = "New sequence saved";
				}
			}
		}// End page load

		private void BindImageList()
		{
			imgTable.Clear();
			imgTable.NumberofCols = 8;
			
			int i = 0;
			foreach( Photo p in _gallery.SortedPhotos.GetValueList() )
			{
				ImageCell cell = new ImageCell();
				cell.ID = "cell" + i;
				HtmlImage img = new HtmlImage();
				img.ID="ph" + p.Id.ToString();
				img.Attributes.Add( "name", img.ID );
				img.Border=0;
				img.Src = p.ThumbImage;
				img.Attributes.Add( "OnMouseOver", "this.style.cursor='pointer'");
				cell.AddImage( img );
				cell.CssClass = "normal";
				cell.Attributes.Add( "OnClick", "SetSelected(" + i.ToString() + "); return false;" );
				cell.save();
				imgTable.AddCell( cell );

				HtmlInputHidden hd = new HtmlInputHidden();
				hd.ID="hd" + i.ToString();
				hd.Name = hd.ID;
				hd.Value = p.Id.ToString();
				this.Panel1.Controls.Add( hd );
				i++;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
