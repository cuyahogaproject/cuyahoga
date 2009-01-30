using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Web.Manager.Helpers
{
	public static class SectionAdminExtensions
	{
		public static string SectionSetting(this HtmlHelper htmlHelper, ModuleSetting moduleSetting, string name, object value)
		{
			return SectionSetting(htmlHelper, moduleSetting, name, value, false);
		}

		public static string SectionSetting(this HtmlHelper htmlHelper, ModuleSetting moduleSetting, string name, object value, bool fixedTextBoxWidth)
		{
			Type settingType = moduleSetting.GetRealType();

			switch (settingType.FullName)
			{
				case "System.String":
					return fixedTextBoxWidth ? CreateSettingTextBox(htmlHelper, name, value, 300) : CreateSettingTextBox(htmlHelper, name, value, null);
				case "System.Int16":
				case "System.Int32":
					return fixedTextBoxWidth ? CreateSettingTextBox(htmlHelper, name, value, 50) : CreateSettingTextBox(htmlHelper, name, value, null);
				case "System.Boolean":
					return CreateSettingCheckBox(name, value);
				default:
					return CreateCustomControl(name, settingType, value);
			}
		}

		private static string CreateSettingTextBox(HtmlHelper htmlHelper, string name, object value, int? length)
		{
			IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
			if (length.HasValue)
			{
				htmlAttributes.Add("style", string.Format("width:{0}px", length.Value));
			}
			return htmlHelper.TextBox(name, value, htmlAttributes);
		}

		private static string CreateSettingCheckBox(string name, object value)
		{
			TagBuilder tb = new TagBuilder("input");
			tb.MergeAttribute("type", "checkbox");
			tb.MergeAttribute("id", name);
			tb.MergeAttribute("name", name);
			bool isChecked = Convert.ToBoolean(value);
			if (isChecked)
			{
				tb.MergeAttribute("checked", "checked");
			}
			return tb.ToString();
		}

		private static string CreateCustomControl(string name, Type settingType, object value)
		{
			if (settingType.IsEnum)
			{
				TagBuilder tb = new TagBuilder("select");
				tb.MergeAttribute("id", name);
				tb.MergeAttribute("name", name);
				StringBuilder options = new StringBuilder();
				for (int i = 0; i < Enum.GetValues(settingType).Length; i++)
				{
					string optionText = Enum.GetName(settingType, i);
					TagBuilder optionBuilder = new TagBuilder("option");
					optionBuilder.InnerHtml = optionText;
					if (value != null && optionText == value.ToString())
					{
						optionBuilder.MergeAttribute("selected", "selected");
					}
					options.Append(optionBuilder.ToString());
				}
				tb.InnerHtml = options.ToString();
				return tb.ToString();
			}
			else
			{
				return null;
			}
		}
	}
}
