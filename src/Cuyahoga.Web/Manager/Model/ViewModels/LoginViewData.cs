using Castle.Components.Validator;

namespace Cuyahoga.Web.Manager.Model.ViewModels
{
	public class LoginViewData
	{
		[ValidateNonEmpty("UserNameValidatorNonEmpty")]
		public string Username { get; set; }
		[ValidateNonEmpty("PasswordValidatorNonEmpty")]
		[ValidateLength(5, 50, "PasswordValidatorLength")]
		public string Password { get; set; }
	}
}