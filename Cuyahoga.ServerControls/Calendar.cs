using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// An ASP.NET Server Control that wraps mishoo's javascript calendar 
	/// (http://www.dynarch.com/projects/calendar/). 
	/// </summary>
	[DefaultProperty("Text"), ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>")]
	[ValidationProperty("Text")]
	public class Calendar : WebControl, INamingContainer
	{
		private TextBox _dateTextBox;
		private Image _calendarImage;

		#region properties
	
		[Bindable(true), Category("Appearance"), DefaultValue("")] 
		public string Text 
		{
			get 
			{
				EnsureChildControls();
				return this._dateTextBox.Text;
			}
			set 
			{ 
				EnsureChildControls();
				this._dateTextBox.Text = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue(CalendarTheme.system)]
		public CalendarTheme Theme
		{
			get { return (ViewState["Theme"] != null ? (CalendarTheme)ViewState["Theme"] : CalendarTheme.system); }
			set { ViewState["Theme"] = value; }	
		}

		[Bindable(true), Category("Behavior"), DefaultValue(CalendarLanguage.en)]
		public CalendarLanguage Language
		{
			get { return (ViewState["Language"] != null ? (CalendarLanguage)ViewState["Language"] : CalendarLanguage.en); }
			set { ViewState["Language"] = value; }		
		}

		[Bindable(true), Category("Behavior"), DefaultValue("~/Support/JsCalendar")]
		public string SupportDir
		{
			get { return (ViewState["SupportDir"] != null ? (string)ViewState["SupportDir"] : ""); }
			set { ViewState["SupportDir"] = value; }		
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool DisplayTime
		{
			get { return (ViewState["DisplayTime"] != null ? (bool)ViewState["DisplayTime"] : false); }
			set { ViewState["DisplayTime"] = value; }		
		}

		[Browsable(false), Bindable(false)]
		public string DateFormat
		{
			get { return (ViewState["DateFormat"] != null ? (string)ViewState["DateFormat"] : ""); }
			set { ViewState["DateFormat"] = value; }		
		}

		[Browsable(false), Bindable(false)]
		public string TimeFormat
		{
			get { return (ViewState["TimeFormat"] != null ? (string)ViewState["TimeFormat"] : ""); }
			set { ViewState["TimeFormat"] = value; }		
		}

		[Bindable(true), Category("Behavior")]
		public DateTime SelectedDate
		{
			get 
			{ 
				EnsureChildControls();
				if (this.Text.Length > 0)
				{
					try
					{
						return DateTime.Parse(this.Text);
					}
					catch (FormatException ex)
					{
						System.Diagnostics.Trace.WriteLine("Invalid datetime: " + this._dateTextBox.Text + " " + ex.Message, this.GetType().FullName);
						return DateTime.MinValue;
					}
				}
				else
				{
					return DateTime.MinValue;
				}
			}
			set 
			{ 
				EnsureChildControls();
				this.Text = value.ToShortDateString();
				if (this.DisplayTime)
				{
					this.Text += " " + value.ToShortTimeString();
				}
			}
		}

		public override ControlCollection Controls
		{
			get
			{
				EnsureChildControls();
				return base.Controls;
			}
		}
		
		[TypeConverter(typeof(UnitConverter))]
		public override Unit Width
		{
			get
			{
				EnsureChildControls();
				return base.Width;
			}
			set
			{
				EnsureChildControls();
				base.Width = value;
				this._dateTextBox.Width = Unit.Pixel((int)value.Value - 24);
			}
		}



		#endregion

		public event System.EventHandler DateChanged;

		protected virtual void OnDateChanged(object sender)
		{
			if (DateChanged != null)
			{
				DateChanged(sender, System.EventArgs.Empty);
			}
		}

		public Calendar()
		{
			// Set defaults
			this.Theme = CalendarTheme.system;
			this.SupportDir = "~/Support/JsCalendar";
			this.DisplayTime = false;
			this.DateFormat = ConvertDateFormat(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
			this.TimeFormat = ConvertTimeFormat(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
			SetLanguage();
		}

		protected override void CreateChildControls()
		{
			this._dateTextBox = new TextBox();
			this._calendarImage = new Image();

			this._dateTextBox.EnableViewState = true;
			this._dateTextBox.ID = "dateTextBox";
			this._dateTextBox.TextChanged += new EventHandler(DateTextBox_TextChanged);

			this._calendarImage.EnableViewState = false;
			this._calendarImage.ID = "trigger";
			this._calendarImage.ImageUrl = GetClientFileUrl("cal.gif");
			this._calendarImage.Attributes["align"] = "top";
			this._calendarImage.Attributes["hspace"] = "4";

			Controls.Add(this._dateTextBox);
			Controls.Add(this._calendarImage);
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			if (this.Site != null && this.Site.DesignMode)
			{
				this._dateTextBox.RenderControl(output);
				output.Write("[" + this.ID + "]");
			}
			else
			{
				base.Render(output);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			string themeCss = GetClientCssImport(String.Format("calendar-{0}.css", this.Theme.ToString().Replace("_", "-")));
			Page.RegisterClientScriptBlock("calendarcss", themeCss);

			string calendarScripts = "";
			calendarScripts += GetClientScriptInclude("calendar.js");
			calendarScripts += GetClientScriptInclude("calendar-setup.js");
			string languageFile = String.Format("lang/calendar-{0}.js", this.Language.ToString());
			calendarScripts += GetClientScriptInclude(languageFile);
			Page.RegisterClientScriptBlock("calendarscripts", calendarScripts);
			string setupScript = GetCalendarSetupScript(this._dateTextBox.ClientID, GetFormatString(), this.ClientID);
			Page.RegisterStartupScript(this.ClientID + "script", setupScript);
		}

		private void SetLanguage()
		{
			string currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			try
			{
				CalendarLanguage cl = (CalendarLanguage)Enum.Parse(typeof(CalendarLanguage), currentLanguage);
				this.Language = cl;
			}
			catch
			{
				// Default is 'en'
				this.Language = CalendarLanguage.en;
			}
		}

		private string GetClientFileUrl(string fileName)
		{
			return ResolveUrl(this.SupportDir + "/" + fileName);
		}

		private string GetClientScriptInclude(string scriptFile) 
		{
			return "<script language=\"JavaScript\" src=\"" +
				GetClientFileUrl(scriptFile) + "\"></script>\n";
		}

		private string GetClientCssImport(string fileName)
		{
			return "<style type=\"text/css\">@import url(" + GetClientFileUrl(fileName) + ");</style>\n";
		}

		private string GetCalendarSetupScript(string inputField, string format, string trigger)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\">Calendar.setup( { inputField : \"");
			sb.Append(inputField);
			sb.Append("\", ifFormat : \"");
			sb.Append(format);
			sb.Append("\", button : \"");
			sb.Append(trigger);
			sb.Append("\", showsTime : ");
			sb.Append(this.DisplayTime.ToString().ToLower());
			sb.Append(" } ); </script>");
			return sb.ToString();
		}

		public string GetFormatString()
		{
			if (this.DisplayTime)
			{
				return this.DateFormat + " " + this.TimeFormat;
			}
			else
			{
				return this.DateFormat;
			}
		}

		private void DateTextBox_TextChanged(object sender, EventArgs e)
		{
			OnDateChanged(sender);
		}

		private string ConvertDateFormat(string shortDateFormat)
		{
			string tempFormat = ReplaceFormatCharacter(shortDateFormat, "y", "%Y");
			tempFormat = ReplaceFormatCharacter(tempFormat, "M", "%m");
			tempFormat = ReplaceFormatCharacter(tempFormat, "d", "%d");
			return tempFormat;
		}

		private string ConvertTimeFormat(string shortTimeFormat)
		{
			string tempFormat = ReplaceFormatCharacter(shortTimeFormat, "H", "%H");
			tempFormat = ReplaceFormatCharacter(tempFormat, "m", "%M");
			tempFormat = ReplaceFormatCharacter(tempFormat, "h", "%I");
			tempFormat = tempFormat.Replace("tt", "%p");
			return tempFormat;
		}

		private string ReplaceFormatCharacter(string shortDateFormat, string from, string to)
		{
			// This method replaces 1 to 4 occurences of the given 'from' string to the 'to'
			// string.
			string pattern = from + "{1,4}";
			Regex regex = new Regex(pattern, RegexOptions.Compiled);
			return regex.Replace(shortDateFormat, to);
		}
	}

	public enum CalendarTheme
	{
		aqua,
		blue,
		blue2,
		brown,
		green,
		win2k_1,
		win2k_2,
		win2k_cold_1,
		win2k_cold_2,
		system
	}

	public enum CalendarLanguage
	{
		af,
		br,
		ca,
		da,
		de,
		du,
		el,
        en,
		es,
		fi,
		fr,
		hr,
		hu,
		it,
		jp,
		ko,
		lt,
		nl,
		no,
		pl,
		pt,
		ro,
		ru,
		sl,
		si,
		sk,
		sp,
		sv,
		tr,
		zh
	}
}
