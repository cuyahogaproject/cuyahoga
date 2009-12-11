using System.Collections.Generic;
using System.Linq;

namespace Cuyahoga.Web.Mvc.HtmlEditor
{
	public class TinyMceScriptProvider : IHtmlEditorScriptProvider
	{
		private const string TinyMceDir = "tiny_mce/";

		public string[] GetSupportScriptFiles()
		{
			IList<string> scripts = new List<string>();
			scripts.Add(TinyMceDir + "jquery.tinymce.js");
			scripts.Add(TinyMceDir + "cuyahoga.tinymce.js");
			return scripts.ToArray();
		}
	}
}
