using System;

namespace Cuyahoga.Core.Service.Membership
{
	/// <summary>
	/// Cuyahoga permission attribute. Use this to specify the required rights for accessing a method or property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class)]
	public class CuyahogaPermissionAttribute : Attribute
	{
		private string _rights;
		private string[] _rightsArray;

		/// <summary>
		/// The required rights as a comma-separated string.
		/// </summary>
		public string RequiredRights
		{
			get { return _rights; }
			set { SetRights(value); }
		}

		/// <summary>
		/// Array of required rights.
		/// </summary>
		public string[] RightsArray
		{
			get { return this._rightsArray; }
		}

		private void SetRights(string rightsAsString)
		{
			this._rights = rightsAsString;
			this._rightsArray = rightsAsString.Split(new char[1] {','});
		}
	}
}
