using System;
using System.Text;
using System.Text.RegularExpressions;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class TemplateViewData
	{
		private Template _template;
		private string _templateHtml;
		private string _templateCss;
		private string _cssIdPrefix = String.Empty;

		public Template Template
		{
			get { return _template; }
		}

		public string TemplateHtml
		{
			get { return _templateHtml; }
		}

		public string TemplateCss
		{
			get { return _templateCss; }
		}

		public string CssIdPrefix
		{
			get { return _cssIdPrefix; }
		}

		public TemplateViewData(Template template, string templateHtml, string _templateCss)
		{
			_template = template;
			this._templateCss = _templateCss;
			_templateHtml = templateHtml;
		}

		public void PrepareTemplateDataForEmbedding(string absoluteSiteDataRoot)
		{
			string templateBasePath = absoluteSiteDataRoot + this._template.BasePath;
			// prepend css prefix in the css to all elements to avoid collisions with the admin css.
			this._cssIdPrefix = "template-" + this._template.Id;

			StringBuilder sb = new StringBuilder(); 
			// regex from Nick Franceschina: http://regexlib.com/REDetails.aspx?regexp_id=916, thanks!
			string cssRegex = @"((\s*(?<element>[^,{]+)\s*,?\s*)*?){((\s*([^:]+)\s*:\s*([^;]+?)\s*;\s*)*?)}";
			MatchCollection matches = Regex.Matches(this._templateCss, cssRegex);
			foreach (Match match in matches)
			{
				string cssFragment = match.Value;
				// replace relative urls for images in template with absolute urls
				cssFragment = Regex.Replace(cssFragment, @"url\(../", "url(" + templateBasePath + "/");
				// append prefix and remove line breaks
				cssFragment = "#" + this._cssIdPrefix + " " + cssFragment.Replace("\r\n", String.Empty);
				// replace body declaration with id of the containing div
				if (cssFragment.StartsWith("#" + this._cssIdPrefix + " body"))
				{
					cssFragment = cssFragment.Replace(" body", String.Empty);
				}
				sb.AppendLine(cssFragment);
			}
			this._templateCss = sb.ToString();

			
		}
	}
}
