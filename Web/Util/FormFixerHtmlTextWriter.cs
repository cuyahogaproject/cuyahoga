using System;
using System.IO;
using System.Web.UI;

namespace Cuyahoga.Web.Util
{
	/// <summary>
	/// Custom HtmlWriter to override the default action attribute of the form tag.
	/// We want this when the page is called with a virtual url (rewritten).
	/// Thanks to Jesse Ezell for this solution: http://weblogs.asp.net/jezell/archive/2004/03/15/90045.aspx
	/// </summary>
	public class FormFixerHtmlTextWriter : HtmlTextWriter
	{
		private bool _inForm = false;
		private string _action;

		public FormFixerHtmlTextWriter(TextWriter writer, string tabString, string action) : base(writer, tabString)
		{
			this._action = action;
		}

		public override void WriteBeginTag(string tagName)
		{
			this._inForm = (tagName.Equals("form"));
			base.WriteBeginTag (tagName);
		}


		public override void WriteAttribute(string name, string value, bool fEncode)
		{
			if (this._inForm)
			{
				if (name.Equals("action"))
				{
					value = this._action;
				}
			}
			base.WriteAttribute (name, value, fEncode);
		}
	}
}
