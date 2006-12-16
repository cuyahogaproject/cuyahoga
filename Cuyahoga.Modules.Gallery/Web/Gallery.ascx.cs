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
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text.RegularExpressions;

	using Cuyahoga.Core.Util;
	using Cuyahoga.Core.Domain;
	using Cuyahoga.Web.UI;
	using Cuyahoga.Web.Util;
	using Cuyahoga.Modules.Gallery.Domain;
	using Cuyahoga.Modules.Gallery.UI;

	/// <summary>
	///		This component display the list of avaiable Galleries 
	///		Output is formatted as a table and rendering is done thru css
	/// </summary>
	public class Gallery : BaseGalleryControl
	{

		private GalleryModule _module;
		private string _culture;
		private bool _allowComments;
		private bool _allowAnonymousComments ;
		private PageEngine _page;

		protected System.Web.UI.WebControls.Panel pnlGalleries;
		protected System.Web.UI.WebControls.Repeater rptGalleries;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = this.Module as GalleryModule;
			this._page = base.Page as PageEngine;
			this._culture = _page.ActiveNode.Culture;

			this._allowComments = _module.AllowComments;
			this._allowAnonymousComments = _module.AllowAnonymousComments;

			IList galleries = _module.GetIncludedGalleries();
			foreach( PhotoGallery g in galleries )
			{
				Descriptions d  = _module.GetObjectDescriptions( DescriptionType.Gallery, g.Id, _culture );
				g.CurrentDescription =  d.Description;
			}
			
			this.pnlGalleries.Visible=true;
			this.rptGalleries.DataSource = galleries;
			this.rptGalleries.DataBind();
			
		}

		private void pnlGalleries_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			PhotoGallery g = e.Item.DataItem as PhotoGallery;
			
			HyperLink hplGallery = e.Item.FindControl("hplGallery") as HyperLink;
			if (hplGallery != null)
			{
				string url = GetGalleryUrl( g );
				if (url != String.Empty ) 
					hplGallery.NavigateUrl = url;
				else
					hplGallery.NavigateUrl = "";

				hplGallery.Text = g.Title;
			}

			HtmlImage image = e.Item.FindControl("imgBack") as HtmlImage;
			image.Attributes["width"] = this.GetPixels( g.AverageData() ).ToString();
			image.Attributes["src"] = base.ResolveUrl("~/Modules/Gallery/Images/rating_backg.gif");

			HyperLink hplComments = e.Item.FindControl("hplComments") as HyperLink;
			if (this._allowComments)
			{
				string url = GetCommentUrl( g.Id );
				if (url != String.Empty ) 
					hplComments.NavigateUrl = url;
				else
					hplComments.NavigateUrl = "";

				hplComments.Text = base.GetText("COMMENTS") + " " + g.GalleryComments.Count.ToString();
			}

		}

		private string GetGalleryUrl(PhotoGallery g)
		{
			// priority are: virtual path - SectionTo - DefaultPage

			if ( g.VirtualPath != null && g.VirtualPath != String.Empty )
			{
				string url = g.VirtualPath;
				if ( url.IndexOf("{") >= 0) url = string.Format( url, _culture.Substring(0,2) );
				Node node = _module.GetNodeByShortDescriptionAndSite(url, _page.CurrentSite);
				if ( node != null )
				{
					return UrlHelper.GetFriendlyUrlFromNode( node ) + "/" + GalleryModuleAction.ViewGallery.ToString() + "/" +g.Id.ToString();
				}
			}
			
			Section sectionTo = this._module.Section.Connections["ViewGallery"] as Section;
			if (sectionTo != null)
			{
				return UrlHelper.GetUrlFromSection(sectionTo) + "/ViewGallery/" + g.Id.ToString();
			}

			if ( _module.TargetPage != String.Empty )
			{
				Node node = _module.GetNodeByShortDescriptionAndSite( _module.TargetPage , _page.CurrentSite );
				if ( node != null )
				{
					return UrlHelper.GetFriendlyUrlFromNode( node ) +"/ViewGallery/" + g.Id.ToString();
				}
			}
			return String.Empty;
		}

		private string GetCommentUrl(int galleryid)
		{
			Section sectionTo = this._module.Section.Connections["ViewGalleryComments"] as Section;
			if (sectionTo != null)
			{
				return UrlHelper.GetUrlFromSection(sectionTo) + "/ViewGallery/" + galleryid.ToString() +"#Comments";
			}
			return "";
		}

		private int GetPixels(GalleryAverage avg)
		{
			if ( avg.Votes == 0 ) return 1;
			return (int)Math.Round( (avg.Rating * 83 ) / 9 );
			
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
			this.rptGalleries.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.pnlGalleries_ItemDataBound);
		}
		#endregion
	}
}
