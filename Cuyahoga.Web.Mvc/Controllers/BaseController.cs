using System;
using System.Collections.Generic;
using System.Security;
using System.Web.Mvc;
using Castle.Core.Logging;
using Cuyahoga.Core;
using Cuyahoga.Core.Validation;
using Cuyahoga.Web.Mvc.Filters;
using Cuyahoga.Web.Mvc.Localization;
using Cuyahoga.Web.Mvc.ViewModels;

namespace Cuyahoga.Web.Mvc.Controllers
{
	/// <summary>
	/// Base class for Cuyahoga controllers.
	/// </summary>
	[SiteFilter(Order = 1)]
	[MessagesFilter(Order = 2)]
	[LocalizationFilter(Order = 3)]
	[ExceptionFilter(ExceptionType = typeof(SecurityException))]
	public abstract class BaseController : Controller
	{
		private ICuyahogaContext _cuyahogaContext;
		private ILogger _logger = NullLogger.Instance;
		private IModelValidator _modelValidator;
		private ILocalizer _localizer;

		/// <summary>
		/// Get or sets the Cuyahoga context.
		/// </summary>
		public ICuyahogaContext CuyahogaContext
		{
			get { return this._cuyahogaContext; }
			set { this._cuyahogaContext = value; }
		}

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		public ILogger Logger
		{
			get { return this._logger; }
			set { this._logger = value; }
		}

		/// <summary>
		/// Gets or sets the localizer.
		/// </summary>
		public ILocalizer Localizer
		{
			get { return this._localizer; }
			set { this._localizer = value; }
		}

		/// <summary>
		/// Gets the object responsible for message handling.
		/// </summary>
		public MessageViewData Messages
		{
			get
			{
				if (! ViewData.ContainsKey("Messages"))
				{
					throw new InvalidOperationException("Messages are not available. Did you add the MessageFilter attribute to the controller?");
				}
				return (MessageViewData) ViewData["Messages"];
			}
		}

		/// <summary>
		/// Sets the model validator.
		/// </summary>
		/// <remarks>
		/// We could include this dependency as a constructor parameter, but this forces inheritors to also include this parameter.
		/// This way, inheriting controllers need to explicitly set the validator.
		/// </remarks>
		protected IModelValidator ModelValidator
		{
			set { this._modelValidator = value; }
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate)
		{
			return ValidateModel(objectToValidate, null, null, null);
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">Properties to check</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate, string[] includeProperties)
		{
			return ValidateModel(objectToValidate, null, includeProperties);
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="includeProperties">Properties to check</param>
		/// <param name="modelPrefix">The prefix of the form elements</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate, string[] includeProperties, string modelPrefix)
		{
			return ValidateModel(objectToValidate, null, includeProperties, modelPrefix);
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="modelValidator">A specific model validator</param>
		/// <param name="includeProperties">Properties to check</param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate, IModelValidator modelValidator, string[] includeProperties)
		{
			return ValidateModel(objectToValidate, modelValidator, includeProperties, null);
		}

		/// <summary>
		/// Validates the given object. If invalid, the errors are added to the ModelState.
		/// </summary>
		/// <param name="objectToValidate">The object to validate</param>
		/// <param name="modelValidator">A specific model validator</param>
		/// <param name="includeProperties">Properties to check</param>
		/// <param name="modelPrefix"></param>
		/// <returns>True if the object is valid</returns>
		protected virtual bool ValidateModel(object objectToValidate, IModelValidator modelValidator, string[] includeProperties, string modelPrefix)
		{
			if (modelValidator == null && this._modelValidator == null)
			{
				throw new InvalidOperationException("A call to Validate() was made while there is no IModelValidator available to perform validation.");
			}
			// if a specific modelvalidator is passed, use that one, otherwise use the modelvalidator of the controller.
			IModelValidator modelValidatorToUse = modelValidator ?? this._modelValidator;

			if (!modelValidatorToUse.IsValid(objectToValidate, includeProperties))
			{
				IDictionary<string, ICollection<string>> errorsForProperties = modelValidatorToUse.GetErrors();
				foreach (KeyValuePair<string, ICollection<string>> errorsForProperty in errorsForProperties)
				{
					string propertyName = errorsForProperty.Key;
					if (! String.IsNullOrEmpty(modelPrefix))
					{
						propertyName = modelPrefix + "." + propertyName;
					}
					foreach (string errorMessage in errorsForProperty.Value)
					{
						ViewData.ModelState.AddModelError(propertyName, errorMessage);
					}
				}
				return false;
			}
			return true;
		}

		protected string GetText(string key)
		{
			string baseName = String.Format("{0}.globalresources"
			                                , this.GetType().Namespace.Replace(".Controllers", String.Empty).ToLowerInvariant());
			return Localizer.GetString(key, baseName);
		}
	}
}