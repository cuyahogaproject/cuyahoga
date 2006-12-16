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
using System.Xml;

namespace Cuyahoga.Modules.Gallery.Web
{
	/// <summary>
	/// Mange the defaults for Image Processing (ImageProcessing.xml)
	/// </summary>
	public class ImageProcessingConfig
	{
		private static bool _loaded = false;
		private static string _configfile;
		private static ImageProcessingConfig _instance;
		private static object lockobject = new object();
		
		private bool _autogeneratethumb = false;
		private bool _maintainaspectratio = false;
		private int _width = 0;
		private int _height = 0;

		private bool _includewatermark = false;
		private WatermarkPlacement _placement;
		private string _copyright = "";
		private string _watermarkfile = "";
		private string _fontfamilly = "Arial";
		private int _fontsize = 5;

		public bool AutoGenerateThumb
		{
			get { return _autogeneratethumb; }
			set { _autogeneratethumb = value; }
		}

		public bool MaintainAspectRatio
		{
			get { return _maintainaspectratio; }
			set { _maintainaspectratio = value; }
		}

		public int ThumbnailWidth
		{
			get { return _width; }
			set { _width = value; }
		}

		public int ThumbnailHeight
		{
			get { return _height; }
			set { _height = value; }
		}

		public bool IncludeWatermark
		{
			get { return _includewatermark; }
			set { _includewatermark = value; }
		}

		public WatermarkPlacement WatermarkPlacement
		{
			get { return _placement; }
			set { _placement = value; }
		}

		public string CopyrightText
		{
			get { return _copyright; }
			set { _copyright = value; }
		}

		public string FontFamilly
		{
			get { return _fontfamilly; }
			set { _fontfamilly = value; }
		}

		public int FontSize
		{
			get { return _fontsize; }
			set { _fontsize = value; }
		}

		public string WatermarkImage
		{
			get { return _watermarkfile; }
			set { _watermarkfile = value; }
		}
		
		private ImageProcessingConfig( string configuration )
		{
			if ( false == _loaded ) this.LoadSettings( configuration );
		}

		public static ImageProcessingConfig Instance( string configuration )
		{
			if ( _instance == null )
			{
				lock( lockobject )
				{
					_instance = new ImageProcessingConfig( configuration );
				}
			}
			return _instance;
		}

		private void LoadSettings( string configuration )
		{
			try
			{
				_configfile = configuration;
				XmlDocument doc = new XmlDocument();
				doc.Load( configuration );

				// process thumbnail settings
				XmlElement thumb = (XmlElement)doc.GetElementsByTagName("thumbnail")[0];
				_autogeneratethumb = Boolean.Parse( thumb.GetElementsByTagName("autogenerate")[0].InnerText );
				_maintainaspectratio = Boolean.Parse( thumb.GetElementsByTagName("preserveratio")[0].InnerText );
				_width = Int32.Parse( thumb.GetElementsByTagName("width")[0].InnerText );
				_height = Int32.Parse( thumb.GetElementsByTagName("height")[0].InnerText );

				// process copyright for watermarking
				XmlElement waterm = (XmlElement)doc.GetElementsByTagName("watermark")[0];
				_includewatermark = Boolean.Parse( waterm.GetElementsByTagName("include")[0].InnerText );
				XmlNode n = waterm.GetElementsByTagName("text")[0];
				_copyright = waterm.GetElementsByTagName("text")[0].InnerText;
				string str = waterm.GetElementsByTagName("placement")[0].InnerText;
				_placement = (WatermarkPlacement)Enum.Parse( typeof(WatermarkPlacement), str.Replace("-",""), true );
				_fontfamilly = waterm.GetElementsByTagName("font-familly")[0].InnerText;
				_fontsize = Int32.Parse( waterm.GetElementsByTagName("font-size")[0].InnerText );
				_watermarkfile = waterm.GetElementsByTagName("image")[0].InnerText;

				_loaded = true;

			}
			catch (Exception ex )
			{
				throw new Exception("Error during ImageProcessing configuration file load", ex);
			}
		}
	}

	public enum WatermarkPlacement
	{
		TopLeft,
		TopCenter,
		TopRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}
}
