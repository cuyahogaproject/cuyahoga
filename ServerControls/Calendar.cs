using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Cuyahoga.ServerControls
{
	/// <summary>
	/// Summary description for Calendar.
	/// </summary>
	[DefaultProperty("Text"), ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>")]
	public class Calendar : WebControl, INamingContainer
	{
		private string _text;
		private TextBox _dateTextBox;
		private Image _calendarImage;

		#region properties
	
		[Bindable(true), Category("Appearance"), DefaultValue("")] 
		public string Text 
		{
			get { return this._text; }
			set { this._text = value; }
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

		[Bindable(true), Category("Behavior"), DefaultValue("%Y-%m-%d")]
		public string DateFormat
		{
			get { return (ViewState["DateFormat"] != null ? (string)ViewState["DateFormat"] : ""); }
			set { ViewState["DateFormat"] = value; }		
		}

		[Bindable(true), Category("Behavior"), DefaultValue("%H:%M")]
		public string TimeFormat
		{
			get { return (ViewState["TimeFormat"] != null ? (string)ViewState["TimeFormat"] : ""); }
			set { ViewState["TimeFormat"] = value; }		
		}

		[Bindable(true), Category("Behavior")]
		public DateTime SelectedDate
		{
			get { return (ViewState["SelectedDate"] != null ? (DateTime)ViewState["SelectedDate"] : DateTime.MinValue); }
			set { ViewState["SelectedDate"] = value; }
		}

		public override ControlCollection Controls
		{
			get
			{
				EnsureChildControls();
				return base.Controls;
			}
		}


		#endregion

		public Calendar()
		{
			// Set defaults
			this.Theme = CalendarTheme.system;
			this.Language = CalendarLanguage.en;
			this.SupportDir = "~/Support/JsCalendar";
			this.DisplayTime = false;
			this.DateFormat = "%Y-%m-%d";
			this.TimeFormat = "%H:%M";
		}

		protected override void CreateChildControls()
		{
			this._dateTextBox = new TextBox();
			this._calendarImage = new Image();

			this._dateTextBox.ID = "dateTextBox";

			this._calendarImage.ID = "trigger";
			this._calendarImage.ImageUrl = GetClientFileUrl("cal.gif");
			this._calendarImage.Attributes["align"] = "absmiddle";

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
				output.Write(Text);
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
			Page.RegisterClientScriptBlock("calendarscript", GetClientScriptInclude("calendar.js"));
			Page.RegisterClientScriptBlock("calendarsetupscript", GetClientScriptInclude("calendar-setup.js"));
			string languageFile = String.Format("lang/calendar-{0}.js", this.Language.ToString());
			Page.RegisterClientScriptBlock("calendarlanguagescript", GetClientScriptInclude(languageFile));
			string setupScript = GetCalendarSetupScript(this._dateTextBox.ClientID, this.DateFormat, this.ClientID);
			Page.RegisterStartupScript(this.ClientID + "script", setupScript);
		}


		private string GetClientFileUrl(string fileName)
		{
			return this.Page.ResolveUrl(this.SupportDir + "/" + fileName);
		}

		private string GetClientScriptInclude(string scriptFile) 
		{
			return "<script language=\"JavaScript\" src=\"" +
				GetClientFileUrl(scriptFile) + "\"></script>";
		}

		private string GetClientCssImport(string fileName)
		{
			return "<style type=\"text/css\">@import url(" + GetClientFileUrl(fileName) + ");</style>";
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
			sb.Append("\" } ); </script>");
			return sb.ToString();
		}
	}

	public enum CalendarTheme
	{
		winter,
		blue,
		summer,
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
