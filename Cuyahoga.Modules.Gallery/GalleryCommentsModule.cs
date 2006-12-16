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
	/// Summary description for xpGalleryCommentsModule.
	/// </summary>
	public class GalleryCommentsModule : GalleryModuleBase,  IActionConsumer, IActionProvider
	{
		
		private int _rows = 10;
		private bool _comments = true;
		private bool _anonymous = false;

		#region Section Settings Properties
		
		public int NumberOfRows
		{
			get { return _rows; }
		}

		public bool AllowComments
		{
			get { return _comments; }
		}

		public bool AllowAnonymousComments
		{
			get { return _anonymous; }
		}

		#endregion
		
		public GalleryCommentsModule()
		{
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();
			if (IsValidSettings(base.Section.Settings["COMMENTS_ROW_COUNT"]))
			{
				this._rows = Int32.Parse(base.Section.Settings["COMMENTS_ROW_COUNT"] as string);
			}
			if (IsValidSettings(base.Section.Settings["ALLOW_COMMENTS"]))
			{
				this._comments = Boolean.Parse(base.Section.Settings["ALLOW_COMMENTS"] as string);
			}
			if (IsValidSettings(base.Section.Settings["ALLOW_ANONYMOUS_COMMENTS"]))
			{
				this._anonymous = Boolean.Parse(base.Section.Settings["ALLOW_ANONYMOUS_COMMENTS"] as string);
			}
		}

		public ActionCollection GetInboundActions()
		{
			ActionCollection actions = new ActionCollection();
			actions.Add(new Action("ViewGalleryComments", new string[1] {"GalleryId"}));
			return actions;
		}

		public ActionCollection GetOutboundActions()
		{
			ActionCollection actions = new ActionCollection();
			actions.Add(new Action("Register", null));
			actions.Add(new Action("ViewProfile", new string[1] {"UserId"}));
			return actions;
		}
	}
}
