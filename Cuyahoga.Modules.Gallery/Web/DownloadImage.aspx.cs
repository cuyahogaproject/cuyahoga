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

using Cuyahoga.Modules.Gallery.Domain;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Summary description for DownloadImage.
	/// </summary>
	public class DownloadImage : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlImage imgMain;

		private const int BUFFER_SIZE = 8192;

		private string _action = "view";

		private GalleryModuleBase _module;
		private int _photoid;
		private Photo _photo;
		private HtmlGenericControl _body;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			_body = this.Page.FindControl("body") as HtmlGenericControl;

			if ( Request.QueryString["PhotoId"] == null ) Response.Close();
			string s = Request.QueryString["PhotoId"] as string;

			if ( Request.QueryString["Action"] != null && Request.QueryString["Action"] != String.Empty )
			{
				_action = Request.QueryString["Action"];
			}
			try
			{
				_photoid = Int32.Parse( s );
				_module = new GalleryModuleBase();
				_photo = _module.GetPhoto( _photoid );
				if ( _photo == null ) Response.Close();

				if (_action != "download" )
				{
					_body.Attributes.Add("onload", "show()" );
					this.imgMain.Src = _photo.LargeImage;

					// check if the photo has been viewed during the user session
					string views = "/";
					if ( Session[ "photoviewed" ] != null ) views = (string)Session[ "photoviewed" ];
					s = String.Concat( "/", _photoid.ToString(), "/");
					if ( views.IndexOf( s ) < 0 )
					{
						_photo.Views += 1;
						_module.SavePhoto( _photo );
						Session[ "photoviewed" ] = String.Concat( views, _photoid.ToString(), "/" );
					}
				}
				else
				{
					// Script from the Download module
					DownloadCurrentFile();
				}
				
			}
			catch
			{
				Response.Close();
			}
		}

		private void DownloadCurrentFile()
		{
			string physicalFilePath = Server.MapPath( _photo.LargeImage );
			if (System.IO.File.Exists(physicalFilePath))
			{
				Stream fileStream = null;
				try
				{
					byte[] buffer = new byte[BUFFER_SIZE];
					// Open the file.
					fileStream = new System.IO.FileStream(physicalFilePath, System.IO.FileMode.Open,
						System.IO.FileAccess.Read, System.IO.FileShare.Read);

					// Total bytes to read:
					long dataToRead = fileStream.Length;

					// Support for resuming downloads
					long position = 0;
					if (Request.Headers["Range"] != null)
					{
						Response.StatusCode = 206;
						Response.StatusDescription = "Partial Content";
						position = long.Parse(Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
					}
					if (position != 0)
					{
						Response.AddHeader("Content-Range", "bytes " + position.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
					}
					Response.ContentType = "image/jpeg";
					Response.AppendHeader("Content-Disposition", "attachment; filename=" + _photo.LargeImage);
					// The content length depends on the amount that is already transfered in an earlier request.
					Response.AppendHeader("Content-Length", (fileStream.Length - position).ToString());
					
					// Stream the actual content
					bool isInterrupted = false;
					while (dataToRead > 0 && ! isInterrupted)
					{
						// Verify that the client is connected.
						if (Response.IsClientConnected)
						{
							// Read the data in buffer.
							int length = fileStream.Read(buffer, 0, BUFFER_SIZE);

							// Write the data to the current output stream.
							Response.OutputStream.Write(buffer, 0, length);

							// Flush the data to the HTML output.
							Response.Flush();

							buffer = new byte[BUFFER_SIZE];
							dataToRead = dataToRead - length;
						}
						else
						{
							//prevent infinite loop if user disconnects
							isInterrupted = true;
						}
					}

					// Only update download statistics if the download is succeeded.
					if (! isInterrupted)
					{
						_photo.Downloads++;
						_module.SavePhoto( _photo );
					}
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
					Response.End();
				}
			}
			else
			{
				throw new Exception("The physical file was not found on the server.");
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
