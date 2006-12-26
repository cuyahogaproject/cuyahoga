using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Cuyahoga.Modules.Flash.Domain;
using Cuyahoga.Web.UI;
using Xylem.Controls;

namespace Cuyahoga.Modules.Flash.Web
{
	/// <summary>
	///		Summary description for Flash.
	/// </summary>
	public class Flash : BaseModuleControl
	{
		private FlashModule _module;
		protected PlaceHolder plcContent;

		protected FlashMovie flashMovie;

		private void InitFlashControl()
		{
			this._module = this.Module as FlashModule;
			if (this._module != null && ! base.HasCachedOutput)
			{			
				if(this.Module.Section.Settings["MAJORPLUGINVERSION"].ToString() != string.Empty)
					flashMovie.MajorPluginVersion = Convert.ToInt32(this.Module.Section.Settings["MAJORPLUGINVERSION"]);
							
				if(this.Module.Section.Settings["MAJORPLUGINVERSIONREVISION"].ToString() != string.Empty)
					flashMovie.MajorPluginVersionRevision = Convert.ToInt32(this.Module.Section.Settings["MAJORPLUGINVERSIONREVISION"]);
							
				if(this.Module.Section.Settings["MINORPLUGINVERSION"].ToString() != string.Empty)
					flashMovie.MinorPluginVersion = Convert.ToInt32(this.Module.Section.Settings["MINORPLUGINVERSION"]);
							
				if(this.Module.Section.Settings["MINORPLUGINVERSIONREVISION"].ToString() != string.Empty)
					flashMovie.MinorPluginVersionRevision = Convert.ToInt32(this.Module.Section.Settings["MINORPLUGINVERSIONREVISION"]);
							
				if(this.Module.Section.Settings["MOVIEALIGN"].ToString() != string.Empty)
					flashMovie.MovieAlign = Convert.ToString(this.Module.Section.Settings["MOVIEALIGN"]);
							
				if(this.Module.Section.Settings["MOVIEBGCOLOR"].ToString() != string.Empty)
					flashMovie.MovieBGColor = ColorTranslator.FromHtml(this.Module.Section.Settings["MOVIEBGCOLOR"].ToString());
							
				if(this.Module.Section.Settings["MOVIEHEIGHT"].ToString() != string.Empty)
					flashMovie.MovieHeight = Convert.ToString(this.Module.Section.Settings["MOVIEHEIGHT"]);

				if(this.Module.Section.Settings["MOVIEWIDTH"].ToString() != string.Empty)
					flashMovie.MovieWidth = Convert.ToString(this.Module.Section.Settings["MOVIEWIDTH"]);
							
				if(this.Module.Section.Settings["MOVIENAME"].ToString() != string.Empty)
				{
					string movie = Convert.ToString(this.Module.Section.Settings["MOVIENAME"]);
					flashMovie.MovieName = this.Page.ResolveUrl(movie);
				}
							
				if(this.Module.Section.Settings["MOVIEQUALITY"].ToString() != string.Empty)
					flashMovie.MovieQuality = Convert.ToString(this.Module.Section.Settings["MOVIEQUALITY"]);
							
				if(this.Module.Section.Settings["MOVIESCRIPTACCESS"].ToString() != string.Empty)
					flashMovie.MovieScriptAccess = Convert.ToString(this.Module.Section.Settings["MOVIESCRIPTACCESS"]);

                if (this.Module.Section.Settings["MOVIEVARS"].ToString() != string.Empty)
                {
                    Dictionary<string, string> flashVars = ParseFlashVars(this.Module.Section.Settings["MOVIEVARS"].ToString());
                    foreach(string key in flashVars.Keys)
                    {
                        flashMovie.MovieVariables.Add(key, flashVars[key]);
                    }
                }

				//set alt content
				if(this.Module.Section.Settings["ALTERNATEDIVID"].ToString() != string.Empty)
				{
					flashMovie.DivId = this.Module.Section.Settings["ALTERNATEDIVID"].ToString();
				}
				else 
				{
					LoadAlternateContent();
				}

				if(HttpContext.Current.Request["EditAlternateContent"] != null)
					if(Boolean.Parse(HttpContext.Current.Request["EditAlternateContent"].ToString()))
						flashMovie.Visible = false;
			}
		}

		private void LoadAlternateContent()
		{
			if (this._module != null && ! base.HasCachedOutput)
			{
				Literal htmlControl = new Literal();
				AlternateContent shc = this._module.GetContent();
				if (shc != null)
				{
					htmlControl.Text = shc.Content;
				}
				else
				{
					htmlControl.Text = String.Empty;
				}
				flashMovie.Controls.Add(htmlControl);
			}
			
		}
        
        private Dictionary<string, string> ParseFlashVars(string vars)
        {
            Regex regex = new Regex(@"^((\s*([^:]+)\s*:\s*([^;]+?)\s*;\s*)*?)$",
                            RegexOptions.IgnoreCase
                            | RegexOptions.Multiline
                            | RegexOptions.IgnorePatternWhitespace
                            | RegexOptions.Compiled
                            );

            Dictionary<string,string> styles = new Dictionary<string, string>();
            MatchCollection matches = regex.Matches(EndsWithSemiColon(vars));
            if(matches.Count > 0)
            {
                GroupCollection groups = matches[0].Groups;
                for (int i = 0; i < groups[3].Captures.Count; i++)
                {
                    styles.Add(groups[3].Captures[i].Value, groups[4].Captures[i].Value);
                }
            }
            else
            {
                throw new InvalidFlashVarsException("Unable to parse flash vars : " + vars);
            }

            return styles;
        }

        private string EndsWithSemiColon(string vars)
        {
            if (vars.EndsWith(";"))
                return vars;
            else
                return String.Concat(vars,";");
        }

		override protected void OnInit(EventArgs e)
		{
			InitFlashControl();
			base.OnInit(e);
		}

	}

    public class InvalidFlashVarsException : ApplicationException
    {
        public InvalidFlashVarsException(string message)
            : base(message)
        {
        }
    }
}
