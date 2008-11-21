using System;
using System.Web.Mvc;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Web.Mvc
{
	public static class ValidationExtensions
	{
		/// <summary>
		/// Emits Javascript code to validate form elements
		/// </summary>
		/// <param name="modelToValidate">Your data object whose properties have [ValidateXXX] attributes, defining validation rules</param>
		/// <param name="formId">HTML element ID of the form.</param>
		/// <returns>A <script>...</script> block. Put it *after* any HTML elements you want to validate.</returns>
		public static string ClientSideValidation(this HtmlHelper html, object modelToValidate, string formId)
		{
			return ClientSideValidation(html, modelToValidate, formId, s => s);
		}

		/// <summary>
		/// Emits Javascript code to validate form elements
		/// </summary>
		/// <param name="modelToValidate">Your data object whose properties have [ValidateXXX] attributes, defining validation rules</param>
		/// <param name="formId">HTML element ID of the form.</param>
		/// <param name="propertyNameToElementId">A function to map property names to HTML element IDs, e.g. (prop => "Prefix." + prop)</param>
		/// <returns>A <script>...</script> block. Put it *after* any HTML elements you want to validate.</returns>
		public static string ClientSideValidation(this HtmlHelper html, object modelToValidate, string formId
		                                          , Func<string, string> propertyNameToElementId)
		{
			BrowserValidationEngine browserValidationEngine = ValidationEngineAccessor.Current.BrowserValidationEngine;
			return browserValidationEngine.GenerateClientScript(modelToValidate, formId, propertyNameToElementId);
		}
	}
}