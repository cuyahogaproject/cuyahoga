using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Validation.ModelValidators
{
	public class FileResourceModelValidator : CastleModelValidator<FileResource>
	{
		protected override void PerformValidation(FileResource objectToValidate, ICollection<string> includeProperties)
		{
			base.PerformValidation(objectToValidate, includeProperties);

			// Check publish date
			if (ShouldValidateProperty("PublishedAt", includeProperties)
				&& ! objectToValidate.PublishedAt.HasValue)
			{
				AddError("PublishedAt", "PublishedAtIsRequired", true);
			}

			// File name
			if (ShouldValidateProperty("FileName", includeProperties)
				&& String.IsNullOrEmpty(objectToValidate.FileName))
			{
				AddError("FileName", "FileIsRequired", true);
			}
		}
	}
}
