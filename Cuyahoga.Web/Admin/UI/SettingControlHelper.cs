using System;
using System.Web.UI.WebControls;

namespace Cuyahoga.Web.Admin.UI
{
	/// <summary>
	/// Summary description for SettingControlHelper.
	/// </summary>
	public class SettingControlHelper
	{
		private SettingControlHelper()
		{
		}

		/// <summary>
		/// Create a webcontrol for administration of a custom setting.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="settingType"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static WebControl CreateSettingControl(string name, Type settingType, string value)
		{
			switch (settingType.FullName)
			{
				case "System.String":
					return CreateSettingTextBox(name, value, 300);
				case "System.Int16":
				case "System.Int32":
					return CreateSettingTextBox(name, value, 50);
				case "System.Boolean":
					return CreateSettingCheckBox(name, value);
				default:
					return CreateCustomControl(name, settingType, value);
			}
		}

		private static TextBox CreateSettingTextBox(string name, string value, int width)
		{
			TextBox tbx = new TextBox();
			tbx.ID = name;
			tbx.Text = value;
			tbx.Width = Unit.Pixel(width);
			return tbx;
		}

		private static CheckBox CreateSettingCheckBox(string name, string value)
		{
			CheckBox chk = new CheckBox();
			chk.ID = name;
			if (value != null)
			{
				chk.Checked = (Boolean.Parse(value));
			}
			return chk;
		}

		private static WebControl CreateCustomControl(string name, Type settingType, string value)
		{
			if (settingType.IsEnum)
			{
				DropDownList ddl = new DropDownList();
				ddl.ID = name;
				for (int i = 0; i < Enum.GetValues(settingType).Length; i++)
				{
					string option = Enum.GetName(settingType, i);
					ddl.Items.Add(new ListItem(option, option));
				}
				ListItem li = ddl.Items.FindByText(value);
				if (li != null)
				{
					li.Selected = true;
				}
				return ddl;
			}
			else
			{
				return null;
			}
		}
	}
}
