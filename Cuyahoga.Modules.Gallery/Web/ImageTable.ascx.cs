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

	using Cuyahoga.Modules.Gallery.Domain;

	/// <summary>
	///		ImageTable user control will render a collection of Photo inside a table layout.
	///		This control is used by both admin and module level control.
	/// </summary>
	public class ImageTable : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTable tblImages;

		private ArrayList _cells = new ArrayList();
		private int _numCols = 1;
		private string _tableClass = "";
		private string _name="";

		/// <summary>
		/// Maximum number of cells per row
		/// </summary>
		public int NumberofCols
		{
			get { return _numCols; }
			set { _numCols = value; }
		}

		/// <summary>
		/// Css Class attribute for the table
		/// </summary>
		public string CssClass
		{
			get { return _tableClass; }
			set { _tableClass = value; }
		}

		/// <summary>
		/// Name attribute for the table
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Clear current content
		/// </summary>
		public void Clear()
		{
			_cells.Clear();
		}

		/// <summary>
		/// Add a xpImageCell to the table
		/// </summary>
		/// <param name="cell"></param>
		public void AddCell( ImageCell cell )
		{
			_cells.Add( cell );
		}
		
		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			// set table attributes
			if ( _name != String.Empty ) tblImages.Attributes.Add( "name", _name );
			if (_tableClass != String.Empty ) tblImages.Attributes.Add( "class", _tableClass );

			int i = -1;
			HtmlTableRow row = new HtmlTableRow();

			foreach( ImageCell cell in _cells )
			{
				i++;
				if ( i >= _numCols )
				{
					tblImages.Rows.Add( row );
					row = new HtmlTableRow();
					i = 0;
				}

				row.Cells.Add( cell as HtmlTableCell );
			}
			if ( i < _numCols ) tblImages.Rows.Add( row );
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
