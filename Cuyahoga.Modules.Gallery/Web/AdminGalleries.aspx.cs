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

using Cuyahoga.Core.Util;
using Cuyahoga.Modules.Gallery.Domain;
using Cuyahoga.Web.UI;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for AdminGalleries.
	/// </summary>
	public class AdminGalleries : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.Repeater rptGalleries;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnNew;
		
		GalleryModule _module;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as GalleryModule;
			this.btnNew.Attributes.Add("onClick", String.Format("document.location.href='EditGallery.aspx{0}&GalleryId=-1'", base.GetBaseQueryString()));
			if ( false == this.IsPostBack )
			{
				this.rptGalleries.DataSource = _module.GetAllGalleries();
				this.rptGalleries.DataBind();
			}
		}

		private void rptGallery_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			PhotoGallery g = e.Item.DataItem as PhotoGallery;

			Literal litDateUpdated = e.Item.FindControl("litDateUpdated") as Literal;
			if (litDateUpdated != null)
			{
				litDateUpdated.Text = TimeZoneUtil.AdjustDateToUserTimeZone(g.DateUpdated, this.User.Identity).ToString();
			}

			HyperLink hplEdit = e.Item.FindControl("hpledit") as HyperLink;
			if (hplEdit != null)
			{
				hplEdit.NavigateUrl = String.Format("~/Modules/Gallery/EditGallery.aspx{0}&GalleryId={1}", base.GetBaseQueryString(), g.Id);
			}

			HyperLink hplPhoto = e.Item.FindControl("hpPhotos") as HyperLink;
			if (hplPhoto != null)
			{
				hplPhoto.NavigateUrl = String.Format("~/Modules/Gallery/AdminPhotos.aspx{0}&GalleryId={1}", base.GetBaseQueryString(), g.Id);
			}

			HyperLink hplComments = e.Item.FindControl("hplComments") as HyperLink;
			if (hplComments != null)
			{
				hplComments.NavigateUrl = String.Format("~/Modules/Gallery/AdminGalleryComments.aspx{0}&GalleryId={1}", base.GetBaseQueryString(), g.Id);
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
			this.rptGalleries.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rptGallery_ItemDataBound);


		}
		#endregion
	}
}
