using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuyahoga.Web.Manager.Helpers
{
	public static class FileTypeExtensions
	{
		private static IDictionary<string, string> FileImagesMap = new Dictionary<string, string>();

		static FileTypeExtensions()
		{
			InitFileImagesMap();
		}

		public static string FileImage(this HtmlHelper htmlHelper, string imageDirectory, string fileName)
		{
			string fileImage = "page_white.png";
			string extension = Path.GetExtension(fileName);
			if (FileImagesMap.ContainsKey(extension))
			{
				fileImage = FileImagesMap[extension];
			}
			string imagePath = VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(imageDirectory, fileImage));
			TagBuilder tb = new TagBuilder("img");
			tb.MergeAttribute("src", imagePath);
			tb.MergeAttribute("alt", extension);
			return tb.ToString(TagRenderMode.SelfClosing);
		}

		private static void InitFileImagesMap()
		{
			FileImagesMap.Add(".pdf", "page_white_acrobat.png");
			FileImagesMap.Add(".zip", "page_white_compressed.png");
			FileImagesMap.Add(".rar", "page_white_compressed.png");
			FileImagesMap.Add(".7z", "page_white_compressed.png");
			FileImagesMap.Add(".gz", "page_white_compressed.png");
			FileImagesMap.Add(".xls", "page_white_excel.png");
			FileImagesMap.Add(".xlsx", "page_white_excel.png");
			FileImagesMap.Add(".swf", "page_white_flash.png");
			FileImagesMap.Add(".fla", "page_white_flash.png");
			FileImagesMap.Add(".flv", "page_white_flash.png");
			FileImagesMap.Add(".gif", "page_white_picture.png");
			FileImagesMap.Add(".png", "page_white_picture.png");
			FileImagesMap.Add(".jpg", "page_white_picture.png");
			FileImagesMap.Add(".bmp", "page_white_picture.png");
			FileImagesMap.Add(".ppt", "page_white_powerpoint.png");
			FileImagesMap.Add(".pptx", "page_white_powerpoint.png");
			FileImagesMap.Add(".txt", "page_white_text.png");
			FileImagesMap.Add(".doc", "page_white_word.png");
			FileImagesMap.Add(".docx", "page_white_word.png");
		}

	}
}
