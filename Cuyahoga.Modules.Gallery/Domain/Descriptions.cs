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

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.Gallery.Domain
{
	/// <summary>
	/// XpRience.net manage the description of objectrs in multilingual way
	/// So all desctription for galleries and photos are maintained here
	/// </summary>
	public class Descriptions
	{
		private int _id;
		private  string _culture;
		private string _class;
		private int _ouid;
		private string _description;
		private DateTime _created;
		private DateTime _updated;

		#region properties
		
		/// <summary>
		/// Unique Descriptions Id
		/// </summary>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Culture
		/// </summary>
		public string Culture
		{
			get { return _culture; }
			set { _culture = value; }
		}

		/// <summary>
		/// Object class (Gallery or Photo)
		/// </summary>
		public string TargetClass
		{
			get { return _class; }
			set { _class = value.ToUpper(); }
		}

		/// <summary>
		/// Object Unique ID
		/// </summary>
		public int ObjectUID
		{
			get { return _ouid; }
			set { _ouid = value; }
		}

		/// <summary>
		/// Descriptions the the current Culture
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		/// Creation date
		/// </summary>
		public DateTime DateCreated
		{
			get { return _created; }
			set { _created = value; }
		}

		/// <summary>
		/// Modification date
		/// </summary>
		public DateTime DateUpdated
		{
			get { return _updated; }
			set { _updated = value; }
		}

		#endregion
		
		#region constructor

		public Descriptions()
		{
			this._id = -1;
			this._created = DateTime.Now;
			this._updated= DateTime.Now;
		}

		#endregion
	}
}
