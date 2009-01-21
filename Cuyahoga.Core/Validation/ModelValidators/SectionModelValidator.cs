using System;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Validation.ModelValidators
{
	public class SectionModelValidator : CastleModelValidator<Section>
	{
		protected override void PerformValidation(Section sectionToValidate, System.Collections.Generic.ICollection<string> includeProperties)
		{
			base.PerformValidation(sectionToValidate, includeProperties);
		
			if (ShouldValidateProperty("Settings", includeProperties))
			{
				// Check if section settings are correct
				foreach (ModuleSetting moduleSetting in sectionToValidate.ModuleType.ModuleSettings)
				{
					if (moduleSetting.IsRequired
						&& (!sectionToValidate.Settings.Contains(moduleSetting.Name)
							|| String.IsNullOrEmpty(sectionToValidate.Settings[moduleSetting.Name].ToString())))
					{
						// TODO section settings and errors localization.
						AddError(moduleSetting.Name, moduleSetting.FriendlyName + " is required", false);
					}
					else
					{
						object settingValue = sectionToValidate.Settings[moduleSetting.Name];
						if (settingValue != null)
						{
							try
							{
								// Check if the datatype is correct -> brute force casting :)
								Type type = moduleSetting.GetRealType();
								if (type.IsEnum && settingValue is string)
								{
									settingValue = Enum.Parse(type, settingValue.ToString());
								}
								else
								{
									if (settingValue.ToString().Length > 0)
									{
										object testObj = Convert.ChangeType(settingValue, type);
									}
								}
							}
							catch (Exception)
							{
								// TODO: localization.
								AddError(moduleSetting.Name, String.Format("Invalid value entered for {0}: {1}", moduleSetting.FriendlyName, settingValue), false);
							}
						}
					}
				}
			}
		}
	}
}
