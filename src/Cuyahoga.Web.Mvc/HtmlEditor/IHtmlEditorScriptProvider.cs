namespace Cuyahoga.Web.Mvc.HtmlEditor
{
	public interface IHtmlEditorScriptProvider
	{
		/// <summary>
		/// Gets an array of required script files for the editor to work. These include the 
		/// directory name of the specific editor package (e.g. /tiny_mce/jquery.tinymce.js).
		/// The editor js files should be placed in the ~/Support directory.
		/// </summary>
		/// <returns></returns>
		string[] GetSupportScriptFiles();
	}
}
