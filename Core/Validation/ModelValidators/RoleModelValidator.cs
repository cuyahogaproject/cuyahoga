using System;
using System.Collections.Generic;
using Cuyahoga.Core.DataAccess;
using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Core.Validation.ModelValidators
{
	public class RoleModelValidator : CastleModelValidator<Role>
	{
		private readonly ICommonDao _commonDao;

		public RoleModelValidator(ICommonDao commonDao)
		{
			_commonDao = commonDao;
		}

		protected override void PerformValidation(Role objectToValidate, ICollection<string> includeProperties)
		{
			base.PerformValidation(objectToValidate, includeProperties);

			// Check username uniqueness.
			if (ShouldValidateProperty("Name", includeProperties)
				&& !String.IsNullOrEmpty(objectToValidate.Name))
			{
				Role role = this._commonDao.GetObjectByDescription(typeof(Role), "Name", objectToValidate.Name) as Role;
				if (role != null && role.Id != objectToValidate.Id)
				{
					AddError("Name", "RoleNameValidatorNotUnique", true);
				}
			}
		}
	}
}