using System;
using System.Collections;

namespace Cuyahoga.Modules.Downloads.Util
{
	/// <summary>
	/// The FileTypesMap class couples a file extension to an icon.
	/// </summary>
	public class FileTypesMap
	{
		private static Hashtable fileTypesMap;

		private FileTypesMap()
		{
		}

		/// <summary>
		/// Try to find a corresponding icon image for the give extension.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static string GetIconFilename(string extension)
		{
			if (fileTypesMap == null)
			{
				InitMap();
			}
			string iconFilename = "file.gif"; // default
			if (fileTypesMap[extension] != null)
			{
				iconFilename = fileTypesMap[extension].ToString();
			}
			return iconFilename;
		}

		private static void InitMap()
		{
			fileTypesMap = new Hashtable();
			fileTypesMap.Add(".asf", "mpg.gif");
			fileTypesMap.Add(".avi", "mpg.gif");
			fileTypesMap.Add(".bmp", "bmp.gif");
			fileTypesMap.Add(".chm", "chm.gif");
			fileTypesMap.Add(".cs", "cs.gif");
			fileTypesMap.Add(".doc", "doc.gif");
			fileTypesMap.Add(".dot", "doc.gif");
			fileTypesMap.Add(".exe", "exe.gif");
			fileTypesMap.Add(".gif", "gif.gif");
			fileTypesMap.Add(".gz", "zip.gif");
			fileTypesMap.Add(".gzip", "zip.gif");
			fileTypesMap.Add(".htm", "htm.gif");
			fileTypesMap.Add(".html", "html.gif");
			fileTypesMap.Add(".jpg", "jpg.gif");
			fileTypesMap.Add(".jpeg", "jpg.gif");
			fileTypesMap.Add(".mdb", "mdb.gif");
			fileTypesMap.Add(".mov", "mpg.gif");
			fileTypesMap.Add(".mp3", "wav.gif");
			fileTypesMap.Add(".mpg", "mpg.gif");
			fileTypesMap.Add(".mpeg", "mpg.gif");
			fileTypesMap.Add(".pdf", "pdf.gif");
			fileTypesMap.Add(".ppt", "ppt.gif");
			fileTypesMap.Add(".rar", "zip.gif");
			fileTypesMap.Add(".rtf", "doc.gif");
			fileTypesMap.Add(".tgz", "zip.gif");
			fileTypesMap.Add(".txt", "txt.gif");
			fileTypesMap.Add(".wav", "wav.gif");
			fileTypesMap.Add(".wma", "wav.gif");
			fileTypesMap.Add(".xls", "xls.gif");
			fileTypesMap.Add(".xml", "xml.gif");
			fileTypesMap.Add(".zip", "zip.gif");
		}
	}
}
