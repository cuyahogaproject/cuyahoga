using System;
using FreeTextBoxControls;
using FreeTextBoxControls.Support;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// The CuyahogaEditor is a subclassed FreeTextBox 2.0.x control with WordClean support, 
	/// an ImageBrowser and a Hyperlink browser.
	/// See also http://www.freetextbox.com.
	/// </summary>
	public class CuyahogaEditor : FreeTextBox
	{
		private string _imageDir;

		/// <summary>
		/// The (virtual) path where the images are located for this editor. If left empty, the ImageDir 
		/// from web.config will be used.
		/// </summary>
		public string ImageDir
		{
			get { return this._imageDir; }
			set { this._imageDir = value; }
		}

		/// <summary>
		/// Initializes a new CuyahogaEditor class.
		/// </summary>
		public CuyahogaEditor() : base()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			Toolbar customToolbar = new Toolbar();
			customToolbar.Items.Add(new InsertLink(this.SupportFolder + "Custom/"));
			customToolbar.Items.Add(new ImageGallery(this.SupportFolder + "Custom/", this.Page.ResolveUrl(this._imageDir)));
			customToolbar.Items.Add(new WordClean());
			this.Toolbars.Add(customToolbar);

			base.OnInit (e);
		}

		/// <summary>
		/// WordClean class. Cleaning javascript found somewhere on the net. Don't know where,
		/// probably from some other free web editor.
		/// </summary>
		public class WordClean : ToolbarButton
		{
			public WordClean() : base("Clean Word html", "FTB_WordClean", "wordclean")
			{
				this.ScriptBlock = @"
				function FTB_WordClean(ftbName) {
				var wordData = editor.document.body.innerHTML; 
				if (wordData.indexOf('class=Mso')>=0){ 
					// make one line 
					wordData = wordData.replace(/\r\n/g, ''); 
					wordData = wordData.replace(/\n/g, ''); 
					wordData = wordData.replace(/\r/g, '');       
					wordData = wordData.replace(/\&nbsp\;/g,''); 
					// keep tags, strip attributes 
					wordData = wordData.replace(/ class=[^\s|>]*/gi,''); 
					//wordData = wordData.replace(/<p [^>]*TEXT-ALIGN: justify[^>]*>/gi,'<p align=""justify"">'); 
					wordData = wordData.replace(/ style=\""[^>]*\""/gi,''); 
					//clean up tags 
					wordData = wordData.replace(/<b [^>]*>/gi,'<b>'); 
					wordData = wordData.replace(/<i [^>]*>/gi,'<i>'); 
					wordData = wordData.replace(/<li [^>]*>/gi,'<li>'); 
					wordData = wordData.replace(/<ul [^>]*>/gi,'<ul>'); 
					// replace outdated tags 
					wordData = wordData.replace(/<b>/gi,'<strong>'); 
					wordData = wordData.replace(/<\/b>/gi,'</strong>'); 
					wordData = wordData.replace(/<i>/gi,'<em>'); 
					wordData = wordData.replace(/<\/i>/gi,'</em>'); 
					// kill unwanted tags 
					wordData = wordData.replace(/<\?xml\:[^>]*>/g, ''); // Word xml 
					wordData = wordData.replace(/<\/?st1\:[^>]*>/g,''); // Word SmartTags 
					wordData = wordData.replace(/<\/st1\:[^>]*>/g,''); // 
					wordData = wordData.replace(/<st1\:[^>]*>/g,''); // 
					wordData = wordData.replace(/<\/?[a-z]\:[^>]*>/g,''); // All other funny Word non-HTML stuff 
					wordData = wordData.replace(/<\/[a-z]\:[^>]*\/>/g,''); // 
					wordData = wordData.replace(/<\/?font[^>]*>/gi,''); // Disable if you want to keep font formatting 
					wordData = wordData.replace(/<\/?span[^>]*>/gi,''); 
					wordData = wordData.replace(/<span[^>]*>/gi,''); 
					wordData = wordData.replace(/<\/?div[^>]*>/gi,''); 
					//remove empty tags 
					wordData = wordData.replace(/<strong><\/strong>/gi,''); 
					wordData = wordData.replace(/<p[^>]*><\/P>/gi,''); 

					editor.document.body.innerHTML = wordData; 
				}
				}";
				// end scriptblock
			}
		}
		public class InsertLink : ToolbarButton
		{
			public InsertLink(string supportFolder) : base("Insert Hyperlink", "CED_InsertLink", "createlink")
			{
				this.ScriptBlock = @"
					function CED_InsertLink(ftbName)
					{
						myWindow = window.open(""" + supportFolder + @"LinkBrowser.aspx?textboxname="" + ftbName + ""&descr="" + CED_GetSelectedText(), ""window"", ""width=550,height=470"");
					}

					function CED_GetSelectedText()
					{
						var sel;
						if (isIE) {
							range = editor.document.selection.createRange();
							sel = range.htmlText;
						}
						else {
							sel = editor.window.getSelection();
						}
						return sel;
					}
					";
			}
		}

		public class ImageGallery : ToolbarButton
		{
			public ImageGallery(string supportFolder, string imageDir) : base("Insert Image from gallery", "CED_ImageGallery", "insertimagefromgallery")
			{
				this.ScriptBlock = @"
					function CED_ImageGallery(ftbName)
					{
						myWindow = window.open(""" + supportFolder + @"ImageBrowser.aspx?textboxname="" + ftbName + ""&imagedir=" + imageDir + @""" , ""window"", ""width=510,height=450"");
					}
					";
			}
		}
	}
}
