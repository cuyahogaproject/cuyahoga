using System;
using System.Collections.Generic;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service.Membership;

namespace Cuyahoga.Core.Validation
{
	public class UserModelValidator : CastleModelValidator<User>
	{
		private readonly IUserService _userService;

		public UserModelValidator(IUserService userService, ILocalizedValidatorRegistry validatorRegistry) : base(validatorRegistry)
		{
			_userService = userService;
		}

		protected override void PerformValidation(User objectToValidate, ICollection<string> includeProperties)
		{
			base.PerformValidation(objectToValidate, includeProperties);

			// Check username uniqueness.
			if ((includeProperties == null || includeProperties.Count == 0) 
				|| includeProperties.Contains("UserName")
				&& ! String.IsNullOrEmpty(objectToValidate.UserName))
			{
				if (this._userService.FindUsersByUsername(objectToValidate.UserName).Count > 0)
				{
					AddError("UserName", "UserNameValidatorNotUnique", true);
				}
			}
		}
	}
}
