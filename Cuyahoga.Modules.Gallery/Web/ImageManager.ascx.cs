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
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using System.Text.RegularExpressions;

	/// <summary>
	///		Summary description for ImageManager.
	/// </summary>
	public class ImageManager : System.Web.UI.UserControl
	{
		public enum EditorMode
		{
			OneImage,
			MultipleImages
		}
		
		protected FredCK.FCKeditorV2.FCKeditor fckContent;

		public int EditorWidth = 650;
		public int EditorHeight = 350;
		public ImageManager.EditorMode Mode = ImageManager.EditorMode.OneImage;

		private void Page_Load(object sender, System.EventArgs e)
		{
			this.fckContent.BasePath = this.Page.ResolveUrl("~/Support/FCKEditor/");
			this.fckContent.CustomConfigurationsPath = this.Page.ResolveUrl("~/Modules/Gallery/Config/fckconfig.js");
			this.fckContent.ToolbarSet = "Image";
			this.fckContent.Width = EditorWidth;
			this.fckContent.Height = EditorHeight;
		}

		public void AddImage( string url )
		{
			this.fckContent.Value += BuildImgTag( url );
		}

		public void Reset()
		{
			this.fckContent.Value = String.Empty;
		}

		private string BuildImgTag( string url )
		{
			if (url != null && url.Trim() != String.Empty )
                return "<img src=\"" + url + "\"><br>";
			else
				return "";
		}

		public string[] GetImages()
		{
			ArrayList ar = new ArrayList();

			string regex = "<img [^\\>]+ src[ ]*=[ ]*\\\"([^\\\"]+)\\\"[^\\>]*\\>";
			System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline) 
				| System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
			System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regex, options);
			
			string buffer = this.fckContent.Value;
			if (reg.IsMatch( buffer ) )
			{
				foreach( Match m in reg.Matches( buffer ) )
				{
					ar.Add( m.Groups[1].Value );
				}
			}
			return ar.ToArray( typeof( string ) ) as string[];
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
