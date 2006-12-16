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
namespace Cuyahoga.Modules.Gallery.Web
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Cuyahoga.Core.Util;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Gallery.Domain;
	using Cuyahoga.Modules.Gallery.UI;

	/// <summary>
	///		Summary description for xpPhotoList.
	/// </summary>
	public class PhotoList : BaseGalleryControl
	{
		protected System.Web.UI.WebControls.Literal litGalleryTitle;
		protected System.Web.UI.WebControls.Literal litDescription;
		protected System.Web.UI.WebControls.Panel pnlDescription;
		protected System.Web.UI.WebControls.Panel pnlRating;

		protected ImageTable imgTable;

		private PhotoGallery _gallery;
		private PhotoListModule _module;
		private PageEngine _page;
		private string _culture;
		private bool _includeDomRoll;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as PhotoListModule;
			
			this._page = base.Page as PageEngine;
			this._culture = _page.ActiveNode.Culture;
			this._module.ModulePathInfo = Context.Request.PathInfo;

			// get the galleryid from the module
			int galleryid = _module.CurrentGalleryId;

			if ( galleryid < 0 )
			{
				// try to get from GallerySection object
				GallerySection gs = _module.GetGallerySection( _module.Section.Id );
				if ( gs != null ) galleryid = gs.GalleryId;
			}

			if ( galleryid > 0 )
			{
				_gallery = _module.GetGallery( galleryid );

				if (_gallery != null )
				{
					this.litGalleryTitle.Text = _gallery.Title;
					string description = _module.GetObjectDescriptionText( DescriptionType.Gallery, _gallery.Id,_culture );
					if ( _module.ShowGalleryDescription )
					{
						this.litDescription.Text = description;
						this.pnlDescription.Visible = true;
					}
					else
					{
						this.pnlDescription.Visible = false;
					}
					BuildImageList();
				}
			}
			this.pnlRating.Visible=false;
		}

		private void BuildImageList()
		{
			imgTable.Clear();
			imgTable.NumberofCols = _module.NumberOfColumns;
			imgTable.CssClass = "xpPhotoList";
			_includeDomRoll = false;

			foreach( Photo p in _gallery.Photos )
			{
				ImageCell cell = new ImageCell();
				cell.CssClass = "xpPhotoList";
				HtmlImage img = new HtmlImage();
				img.ID = "p" + p.Id.ToString();
				img.Attributes.Add( "Name", img.ID );
				img.Src = p.ThumbImage;
				img.Border = 0;
				img.Alt = p.Title;
				img.Attributes.Add( "title", p.Title );

				// Try to set the roll over
				if ( p.ThumbOver != null && p.ThumbOver != String.Empty)
				{
					img.Attributes.Add("class", "domroll " + p.ThumbOver );
					_includeDomRoll = true;
				}
				else
				{
					string str = "this.style.cursor='pointer'; this.className='highlightOn'";
					img.Attributes.Add("OnMouseOver", str );
					img.Attributes.Add("OnMouseOut", "this.className='highlightOff';" );
					img.Attributes.Add("class", "highlightOff" );
				}

				// now we set the onclick action
				string url = String.Empty;
				switch( _module.PhotoListAction )
				{
					case PhotoShowAction.PopupWindow :
						url = base.ResolveUrl("~/Modules/Gallery/ShowImage.aspx");
						url = String.Format("Popup('" + url + "?GalleryId={0}&PhotoId={1}&Culture={2}','{3}','{4}','{5}','{6}'); return false;",_gallery.Id, p.Id, _culture, _module.PopupWidth, _module.PopupHeight, _module.PopupTop, _module.PopupLeft);
						img.Attributes.Add("OnClick", url );
						cell.AddImage( img );
						break;

					case PhotoShowAction.RevealImage :
						HyperLink hpl = new HyperLink();
						hpl.NavigateUrl = p.LargeImage;
						hpl.Attributes.Add("rel", "lightbox");
						hpl.Controls.Add( img );
						cell.AddControl( hpl );
						break;

					case PhotoShowAction.ShowFullScreen :
						url = base.ResolveUrl("~/Modules/Gallery/DownloadImage.aspx");
						url = String.Format("FullWin('" + url + "?GalleryId={0}&PhotoId={1}&Culture={2}&Action=view'); return false;",_gallery.Id, p.Id, _culture);
						img.Attributes.Add("OnClick", url );
						cell.AddImage( img );
						break;

					default :
						cell.AddImage( img );
						break;
				}
				cell.AddBreak();
				cell.AddText( p.Title, "xpImageTitle");

				if ( _module.Display == PhotoListDisplay.ShortInfo )
				{
					cell.AddBreak();
					HtmlGenericControl c = new HtmlGenericControl("span");
					StringBuilder sb = new StringBuilder();
					sb.Append( base.GetText("MUNBEROFVIEWS") + " " + p.Views.ToString() );
					sb.Append("<br>");
					sb.Append(  base.GetText("CURRENTRATING") );
					sb.Append("<br>");
					c.InnerHtml = sb.ToString();
					cell.AddControl( c );
					HtmlGenericControl c2 = new HtmlGenericControl("div");
					c2.Attributes[ "class" ] = "xpRating";
					c2.InnerHtml = this.GetRatingHtml( p ); 
					cell.AddControl( c2 );
				}
				if ( _module.AllowDownload )
				{
					cell.AddBreak();
					cell.AddControl( this.GetDownloadUrl( p ) );
				}
				cell.save();
				imgTable.AddCell( cell );
			}
		}

		private string GetRatingHtml( Photo p )
		{
			StringWriter writer = new StringWriter();
			System.Web.UI.HtmlTextWriter hwriter = new System.Web.UI.HtmlTextWriter( writer );
			this.pnlRating.Attributes[ "style" ] = "position: relative; ";
			this.pnlRating.RenderControl( hwriter );
			string html = writer.ToString();
			writer.Close();
			string file1 = base.ResolveUrl("~/Modules/Gallery/Images/rating2_backg.gif");
			string file2 = base.ResolveUrl("~/Modules/Gallery/Images/rating2.gif");
			int width = (int)Math.Round( (p.GetRating() * 60 ) / 9 );
			
			html = String.Format( html, file1, width, file2 );

			return html;
		}

		private System.Web.UI.Control GetDownloadUrl( Photo photo )
		{
			HtmlGenericControl c = new HtmlGenericControl();
			c.TagName = "span";
			string url = base.ResolveUrl("~/Modules/Gallery/DownloadImage.aspx");
			url = String.Format("Popup('" + url + "?GalleryId={0}&PhotoId={1}&Culture={2}&Action=download','320','240','100','100'); return false;",_gallery.Id, photo.Id, _culture);
			c.Attributes.Add("OnClick", url );
			c.Attributes.Add("style", "text-decoration: underline;");
			c.Attributes.Add("OnMouseOver", "this.style.cursor='pointer';");
			c.InnerText= base.GetText("DOWNLOAD") + " (" + photo.Downloads + ")";

			return c;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			BaseTemplate template = _page.TemplateControl;
			Literal ph = template.FindControl("JavaScripts") as Literal;
			bool popupAdded = false;

			if (ph != null )
			{
				if ( _includeDomRoll )
				{
					string jsfile = base.ResolveUrl("~/Modules/Gallery/js/chrisdomroll.js");
					ph.Text += PhotoActionHelper.GetJSScripTag( jsfile );
				}

				switch ( _module.PhotoListAction )
				{
					case PhotoShowAction.PopupWindow :
						ph.Text += PhotoActionHelper.GetJSPopUp();
						popupAdded = true;
						break;

					case PhotoShowAction.ShowFullScreen :
						ph.Text += PhotoActionHelper.GetJSFullWin();
						break;

					case PhotoShowAction.RevealImage :
						string jsfile = base.ResolveUrl("~/Modules/Gallery/js/lightbox/lightbox.js");
						ph.Text += PhotoActionHelper.GetJSScripTag( jsfile );
						string css = base.ResolveUrl("~/Modules/Gallery/js/lightbox/lightbox.css");
						_page.RegisterStylesheet("lightbox", css );
						break;
				}
				if ( _module.AllowDownload && false == popupAdded)
				{
					ph.Text += PhotoActionHelper.GetJSPopUp();
				}
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
