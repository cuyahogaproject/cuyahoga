using System;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using Cuyahoga.Modules.Forum.Domain;

namespace Cuyahoga.Modules.Forum.Utils
{
	/// <summary>
	/// The TextParser class contains static methods to convert "forum code" and HTML back and forth for use in
	/// editing and presentation.
	/// </summary>
	public class TextParser
	{
		public static string Censor(string text)
		{
			// build the censored words list
			string strCleanedCensorList = "shit fuck";
			string[] strList = strCleanedCensorList.Split(new char[] { ' ' });
			// convert any stand alone words (with * before of after them) to spaces
			for (int i = 0; i < strList.Length; i++)
			{
				strList[i] = strList[i].Replace("*", " ");
			}
			// now you've got your list of naughty words, clean them out of the text
			string newWord = "";
			for (int i = 0; i < strList.Length; i++)
			{
				for (int j = 1; j <= strList[i].Length; j++)
						newWord += "*";

				Regex Cleaner = new Regex(strList[i], RegexOptions.IgnoreCase);
				text = Cleaner.Replace(text, newWord);
				newWord = "";
			}
			return text;
		}

		
		public static string HtmlToForumCode(string text, ForumModule module)
		{
			text = text.Trim();
			// replace line breaks, get block elements right
			text = text.Replace("\r\n", "");
			text = Regex.Replace(text, @"((?<!(\A|<blockquote>|</blockquote>|</p>))(<blockquote>|<p>))", "</p>$1", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			text = Regex.Replace(text, @"(</blockquote>|</p>)((?!(<p>|<blockquote>|</blockquote>))(.*</p>))", "$1<p>$2", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			text = Regex.Replace(text, "^<p>", "", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "</p>$", "", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<blockquote>", "\r\n[quote]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "</blockquote>", "[/quote]\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<p>", "\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "</p>", "\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<br>", "\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<br/>", "\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<br />", "\r\n", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"\[quote\](?!(\r\n))", "[quote]\r\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			text = Regex.Replace(text, @"(?<!(\r\n))\[/quote\]", "\r\n[/quote]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			IList tEc = module.GetAllEmoticons();
			if(tEc != null)
			{
				foreach (ForumEmoticon Item in tEc)
				{
					text = Regex.Replace(text, "<img src=\"" + module.ThemePath + Item.ImageName + "\">", Item.TextVersion, RegexOptions.IgnoreCase);
					text = Regex.Replace(text, "<img src=\"" + module.ThemePath + Item.ImageName + "\"/>", Item.TextVersion, RegexOptions.IgnoreCase);
					text = Regex.Replace(text, "<img src=\"" + module.ThemePath + Item.ImageName + "\" />", Item.TextVersion, RegexOptions.IgnoreCase);

				}
			}

			// replace basic tags
			IList tFtc = module.GetAllTags();
			if(tFtc != null)
			{
				foreach (ForumTag Item in tFtc)
				{
					text = Regex.Replace(text, Item.HtmlCodeStart, Item.ForumCodeStart, RegexOptions.IgnoreCase);
					text = Regex.Replace(text, Item.HtmlCodeEnd, Item.ForumCodeEnd, RegexOptions.IgnoreCase);
				}
			}
			text = Regex.Replace(text, @"</a>", "[/url]", RegexOptions.IgnoreCase);

			// replace img and a tags
			text = Regex.Replace(text, @"(<a href="")(\S+)("" target=""_blank"">)", "[url=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<a href="")(\S+)("" target=_blank>)", "[url=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<a href="")(\S+)("">)", "[url=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<a href="")(\S+)("">)", "[url=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<img src="")(\S+)("">)", "[image=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<img src="")(\S+)("" />)", "[image=\"$2\"]", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<img src="")(\S+)(""/>)", "[image=\"$2\"]", RegexOptions.IgnoreCase);
			
			// catch remaining HTML as invalid
			text = Regex.Replace(text, @"<.*>", "", RegexOptions.IgnoreCase);

			// convert HTML escapes
			text = Regex.Replace(text, "&nbsp;", " ", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "&amp;", "&", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "&lt;", "<", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "&gt;", ">", RegexOptions.IgnoreCase);

			return text.Trim();
		}

		public static string ForumCodeToHtml(string text,ForumModule module)
		{
			text = text.Trim();

			// build stack of forum codes, look for closure and proper nesting
			Regex openPattern = new Regex(@"\[(?!/)[\w""\?=&/;\+%\*\:~,\.\-\$@#]+\]", RegexOptions.IgnoreCase);
			Regex closePattern = new Regex(@"\[/\w+\]", RegexOptions.IgnoreCase);
			Regex bothPattern = new Regex(@"\[[\w""\?=&/;\+%\*\:~,\.\-\$@#]+\]", RegexOptions.IgnoreCase);
			ArrayList stack = new ArrayList();
			Match allMatches = bothPattern.Match(text);
			int indexAfterLastClosingTag = 0;
			while (allMatches.Success)
			{
				string tag = allMatches.ToString();
				if (openPattern.IsMatch(tag))
				{
					// it's an opening tag, add it to the stack
					stack.Add(tag);
					allMatches = allMatches.NextMatch();
				}
				else
				{
					// if it's a closing URL tag, disregard
					//if (tag == "[/url]") break;
					// this is a closing tag. see if it matches the previous opening tag
					if (stack.Count == 0)
					{
						// there are no more opening tags on the stack, so we'll have to insert one after the last closer
						string newCloser = tag.Replace("[/", "[");
						text = text.Insert(indexAfterLastClosingTag, newCloser);
						allMatches = bothPattern.Match(text, indexAfterLastClosingTag + newCloser.Length);
					}
					else
					{
						// opening tags left on the stack...
						string tempOpen = (string)stack[stack.Count - 1];
						if (tempOpen.StartsWith("[url=")) tempOpen = "[url]";
						if (tag.Replace("[/", "[") == tempOpen)
						{
							// it matches the last opening tag, remove it from the stack
							stack.RemoveAt(stack.Count - 1);
							indexAfterLastClosingTag = allMatches.Index + tag.Length;
							allMatches = allMatches.NextMatch();
						}
						else
						{
							// closer doesn't match last opener
							string previousOpeningTag = (string)stack[stack.Count - 1];
							stack.RemoveAt(stack.Count - 1);
							// put a closing tag before it
							string newCloser = previousOpeningTag.Replace("[", "[/");
							text = text.Insert(allMatches.Index, newCloser);
							indexAfterLastClosingTag = allMatches.Index + newCloser.Length;
							// if there's a future closing tag of the same type, we need an opener after this closer
							MatchCollection futureClosers = closePattern.Matches(text, indexAfterLastClosingTag);
							foreach (Match futureClose in futureClosers)
							{
								if (futureClose.ToString() == previousOpeningTag.Replace("[", "[/"))
								{
									text = text.Insert(indexAfterLastClosingTag + tag.Length, previousOpeningTag);
									break;
								}
							}
							allMatches = bothPattern.Match(text, indexAfterLastClosingTag);
						}
					}
				}
			}

			// convert HTML escapes
			text = Regex.Replace(text, "&", "&amp;", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, "<", "&lt;", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, ">", "&gt;", RegexOptions.IgnoreCase);

			// identify URL's that don't have url forum code so we can parse them to be links.
			Regex protolcPattern = new Regex(@"(?<theurl>(?<!(\]|""))(((news|(ht|f)tp(s?))\://)[\w\-\*]+(\.[\w\-/~\*]+)*/?)([\w\?=&/;\+%\*\:~,\.\-\$#])*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Regex wwwPattern = new Regex(@"(?<theurl>(?<!(\]|""|//))(?<=\s|^)(w{3}(\.[\w\-/~\*]+)*/?)([\?\w=&;\+%\*\:~,\-\$#])*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Regex emailPattern = new Regex(@"(?<theurl>(?<=\s|\])(?<!(mailto:|""\]))([\w\.\-_']+)@(([\w\-]+\.)+[\w\-]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			text = protolcPattern.Replace(text, "[url=\"${theurl}\"]${theurl}[/url]");
			text = wwwPattern.Replace(text, "[url=\"http://${theurl}\"]${theurl}[/url]");
			text = emailPattern.Replace(text, "[url=\"mailto:${theurl}\"]${theurl}[/url]");

			// replace URL tags
			text = Regex.Replace(text, @"(\[url="")(\S+?)(""\])", "<a href=\"$2\" target=\"_blank\">", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(<a href=\""mailto:)(\S+?)(\"" target=\""_blank\"">)", "<a href=\"mailto:$2\">", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"\[/url\]", "</a>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(\[image="")(\S+?)(""\])", "<img src=\"$2\" />", RegexOptions.IgnoreCase);

			// replace basic tags
			IList tFtc = module.GetAllTags();
			if(tFtc != null)
			{
				foreach (ForumTag Item in tFtc)
				{
					text = Regex.Replace(text, Regex.Escape(Item.ForumCodeStart), Item.HtmlCodeStart, RegexOptions.IgnoreCase);
					text = Regex.Replace(text, Regex.Escape(Item.ForumCodeEnd), Item.HtmlCodeEnd, RegexOptions.IgnoreCase);
				}
			}

			IList tEc =  module.GetAllEmoticons();

			if(tEc != null)
			{
				foreach (ForumEmoticon Item in tEc)
				{
					text = Regex.Replace(text, Regex.Escape(Item.TextVersion), "<img src=\"" + module.ThemePath + Item.ImageName + "\" />", RegexOptions.IgnoreCase);
				}
			}

			// replace line breaks, get block elements right
			//if (!text.StartsWith("[quote]")) text = "<p class=forum>" + text;
			//if (!text.EndsWith("[/quote]")) text += "</p>";
			text = Regex.Replace(text, @"\[quote\]", "<blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"\[/quote\]", "</blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(?<!(</blockquote>))\r\n\r\n(?!(<p>|<blockquote>|</blockquote>))", "</p><p>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(?<=(</p>|<blockquote>|</blockquote>|\A))(\r\n)*<blockquote>", "<blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"\r\n\r\n<blockquote>", "</p><blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"<blockquote>\r\n(?!(<p>|<blockquote>|</blockquote>))", "<blockquote><p>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(?<!(</p>|<blockquote>|</blockquote>))\r\n</blockquote>", "</p></blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</blockquote>\r\n\r\n", "</blockquote><p>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</blockquote>\r\n</blockquote>", "</blockquote></blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(?<!(</p>|<blockquote>|</blockquote>|\A))<blockquote>", "</p><blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"<blockquote>(?!(<p>|<blockquote>|</blockquote>))", "<blockquote><p>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"(?<!(</p>|<blockquote>|</blockquote>))</blockquote>", "</p></blockquote>", RegexOptions.IgnoreCase);
			text = Regex.Replace(text, @"</blockquote>(\r\n)?(?!(<p>|<blockquote>|</blockquote>|\Z))", "</blockquote><p>", RegexOptions.IgnoreCase);
			text = text.Replace("\r\n", "<br />");

			text = TextParser.Censor(text);

			return text.Trim();
		}
	}
}
