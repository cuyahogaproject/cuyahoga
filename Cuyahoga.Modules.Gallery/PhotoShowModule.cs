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

using Cuyahoga.Core;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;
using Cuyahoga.Core.Search;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Communication;

namespace Cuyahoga.Modules.Gallery
{
	/// <summary>
	/// Summary description for xpPhotoShowModule.
	/// </summary>
	public class PhotoShowModule : GalleryModuleBase , IActionConsumer
	{
		
		private PhotoShowType _showtype = PhotoShowType.RandomImage;
		private PhotoShowAction _showaction = PhotoShowAction.None;
		private bool _usethumb = true;
		private int _areasize = 0;
		private int _fadetime = 4000;
		private int _width = 750;
		private int _height = 600;
		private int _left = 100;
		private int _top = 75;

		public PhotoShowType ShowType
		{
			get { return _showtype; }
		}

		public PhotoShowAction ShowAction
		{
			get { return _showaction; }
		}

		public int AreaSize
		{
			get { return _areasize; }
		}

		public bool UseThumbnails
		{
			get { return _usethumb; }
		}

		public int FadeTiming
		{
			get { return _fadetime; }
		}
		
		public int PopupWidth
		{
			get { return _width; }
		}

		public int PopupHeight
		{
			get { return _height; }
		}
		public int PopupTop
		{
			get { return _top; }
		}
		public int PopupLeft
		{
			get { return _left; }
		}


		public PhotoShowModule( ) 
		{ 
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();
			
			if (IsValidSettings(base.Section.Settings["PHOTOSHOW_TYPE"]))
			{
				this._showtype = (PhotoShowType)Enum.Parse(typeof(PhotoShowType), base.Section.Settings["PHOTOSHOW_TYPE"] as string);
			}
			if (IsValidSettings(base.Section.Settings["PHOTOSHOW_ACTION"]))
			{
				this._showaction = (PhotoShowAction)Enum.Parse(typeof(PhotoShowAction), base.Section.Settings["PHOTOSHOW_ACTION"] as string);
			}
			if (IsValidSettings(base.Section.Settings["PHOTOSHOW_AREASIZE"]))
			{
				this._areasize = Int32.Parse(base.Section.Settings["PHOTOSHOW_AREASIZE"] as string);
			}
			if (IsValidSettings(base.Section.Settings["FADEIN_TIME"]))
			{
				this._fadetime = Int32.Parse(base.Section.Settings["FADEIN_TIME"] as string) * 1000;
			}
			if (IsValidSettings(base.Section.Settings["USE_THUMBNAILS"]))
			{
				this._usethumb = Boolean.Parse(base.Section.Settings["USE_THUMBNAILS"] as string);
			}
			if (IsValidSettings(base.Section.Settings["POPUP_WIDTH"]))
			{
				this._width = Int32.Parse(base.Section.Settings["POPUP_WIDTH"] as string);
			}
			if (IsValidSettings(base.Section.Settings["POPUP_HEIGHT"]))
			{
				this._height = Int32.Parse(base.Section.Settings["POPUP_HEIGHT"] as string);
			}
			if (IsValidSettings(base.Section.Settings["POPUP_TOP"]))
			{
				this._top = Int32.Parse(base.Section.Settings["POPUP_TOP"] as string);
			}
			if (IsValidSettings(base.Section.Settings["POPUP_LEFT"]))
			{
				this._left = Int32.Parse(base.Section.Settings["POPUP_LEFT"] as string);
			}
		}

		public ActionCollection GetInboundActions()
		{
			ActionCollection actions = new ActionCollection();
			actions.Add(new Action("SlideShow", new string[1] {"GalleryId"}));
			return actions;
			
		}

	}

	public enum PhotoShowType
	{
		RandomImage,
		FadeIn
	}

	public enum PhotoShowAction
	{
		None,
		PopupWindow,
		RevealImage,
		ShowFullScreen
	}
}
