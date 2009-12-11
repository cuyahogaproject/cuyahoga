using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuyahoga.Web.Mvc.HtmlEditor
{
	public class FckEditorScriptProvider : IHtmlEditorScriptProvider
	{
		private const string FckEditorDir = "fckeditor/";

		/// <summary>
		/// Gets an array of required script files for the editor to work. These include the 
		/// directory name of the specific editor package (e.g. tiny_mce/jquery.tinymce.js).
		/// The editor js files should be placed in the ~/Support directory.
		/// </summary>
		/// <returns></returns>
		public string[] GetSupportScriptFiles()
		{
			IList<string> scripts = new List<string>();
			scripts.Add(FckEditorDir + "jquery.FCKEditor.js");
			scripts.Add(FckEditorDir + "cuyahoga.fckeditor.js");
			return scripts.ToArray();
		}
	}
}
