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

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Util;
using Cuyahoga.Web.UI;

using Cuyahoga.Modules.Gallery.Domain;


namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for AdminGallerySection.
	/// </summary>
	public class AdminGallerySection : ModuleAdminBasePage
	{
		protected System.Web.UI.WebControls.RadioButtonList rblGalleries;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnNew;
	
		GalleryModuleBase _module;
		GallerySection _gs;
		int _selected = -1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this._module = base.Module as GalleryModuleBase;
			this.btnNew.Attributes.Add("onClick", String.Format("document.location.href='EditGallery.aspx{0}&GalleryId=-1'", base.GetBaseQueryString()));
			_gs = _module.GetGallerySection( base.Section.Id );

			if ( false == this.IsPostBack )
			{
				if ( _gs != null ) _selected = _gs.GalleryId;
			
				int i = 0;
				foreach( PhotoGallery g in _module.GetAllGalleries() )
				{
					rblGalleries.Items.Add( new ListItem( g.Name, g.Id.ToString() ) );
					if ( g.Id == _selected ) rblGalleries.SelectedIndex = i;
					i++;
				}
			}
			else
			{
				_module.SaveGallerySection( Int32.Parse(rblGalleries.SelectedValue), base.Section.Id );
				base.ShowMessage( "Settings have been saved" );
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
