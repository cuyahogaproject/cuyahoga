using System.Collections.Generic;
using Cuyahoga.Core.Validation;
using Resources.Cuyahoga.Modules.Articles;

namespace Cuyahoga.Modules.Articles.Domain
{
	public class ArticleModelValidator : CastleModelValidator<Article>
	{
		protected override void PerformValidation(Article objectToValidate, ICollection<string> includeProperties)
		{
			base.PerformValidation(objectToValidate, includeProperties);

			// Content may not be empty
			if (ShouldValidateProperty("Content", includeProperties) && string.IsNullOrEmpty(objectToValidate.Content))
			{
				AddError("Content", "ContentIsRequired", true);
			}

			// Publish date for articles is required.
			if (ShouldValidateProperty("PublishedAt", includeProperties) && ! objectToValidate.PublishedAt.HasValue)
			{
				AddError("PublishedAt", "PublishedAtIsRequired", true);
			}

			//// Publish end date must be > Publish date
			//if (ShouldValidateProperty("PublishedUntil", includeProperties) 
			//    && objectToValidate.PublishedUntil.HasValue 
			//    && objectToValidate.PublishedUntil < objectToValidate.PublishedAt)
			//{
			//    AddError("PublishedUntil", "PublishedUntilMustBeLaterThanPublishedAt", true);
			//}
		} 
	}
}
