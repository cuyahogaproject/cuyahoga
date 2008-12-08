using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Text;
using Castle.Components.Validator;
using Cuyahoga.Core.Util;
using Cuyahoga.Core.Validation;

namespace Cuyahoga.Web.Mvc.Validation
{
	public class JQueryValidator : IBrowserValidatorProvider
	{
		/// <summary>
		/// Implementors should attempt to read their specific configuration
		/// from the <paramref name="parameters"/>, configure and return
		/// a class that extends from <see cref="BrowserValidationConfiguration"/>
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns>
		/// An instance that extends from <see cref="BrowserValidationConfiguration"/>
		/// </returns>
		public BrowserValidationConfiguration CreateConfiguration(System.Collections.IDictionary parameters)
		{
			JQueryConfiguration config = new JQueryConfiguration();
			config.Configure(parameters);
			return config;
		}

		/// <summary>
		/// Implementors should return their generator instance.
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="inputType"></param>
		/// <param name="propertyNameToElementId"></param>
		/// <returns>A generator instance</returns>
		public IBrowserValidationGenerator CreateGenerator(BrowserValidationConfiguration configuration, InputElementType inputType, Func<string, string> propertyNameToElementId)
		{
			return new JQueryValidationGenerator((JQueryConfiguration)configuration, inputType, propertyNameToElementId);
		}

		#region Configuration

		/// <summary>
		/// The <see cref="BrowserValidationConfiguration"/> implementation for the JQuery validate plugin.
		/// </summary>
		public class JQueryConfiguration : BrowserValidationConfiguration
		{
			readonly Dictionary<string, string> _rules = new Dictionary<string, string>();
			readonly Dictionary<String, CustomRule> _customRules = new Dictionary<String, CustomRule>();
			readonly Dictionary<string, string> _messages = new Dictionary<string, string>();
			readonly IDictionary _options = new Hashtable();

			/// <summary>
			/// Adds a custom rule.
			/// </summary>
			/// <param name="name">The name of the rule.</param>
			/// <param name="violationMessage">The violation message.</param>
			/// <param name="rule">
			/// The rule: must be an anonymous method declaration or the validation 
			/// function reference (the function's name without parenthesis).
			/// </param>
			/// <example>
			/// This example adds a custom rule by passing an anonymous method:
			/// <code>
			/// AddCustomRule( 
			///		"maxWords", 
			///		"Please enter {0} words or less.", 
			///		"function(value, element, params) { return this.optional(element) || value.match(/\b\w+\b/g).length &lt; params; }" );
			/// </code>
			/// This example adds a custom rule by passing a method reference, assuming that a 
			/// referenced javascript file contains the <c>validateMaxWords</c> function.
			/// <code>
			/// AddCustomRule( 
			///		"maxWords", 
			///		"Please enter {0} words or less.", 
			///		"validateMaxWords" );
			/// </code>
			/// </example>
			public void AddCustomRule(string name, string violationMessage, string rule)
			{
				_customRules[name] = new CustomRule(name, rule, violationMessage);
			}

			/// <summary>
			/// Configures the JQuery Validate plugin based on the supplied parameters.
			/// </summary>
			/// <param name="parameters">The parameters.</param>
			public override void Configure(IDictionary parameters)
			{
				AddParameterToOptions(parameters, JQueryOptions.Debug, false);
				AddParameterToOptions(parameters, JQueryOptions.ErrorClass, true);
				AddParameterToOptions(parameters, JQueryOptions.ErrorContainer, true);
				AddParameterToOptions(parameters, JQueryOptions.ErrorElement, true);
				AddParameterToOptions(parameters, JQueryOptions.ErrorLabelContainer, true);
				AddParameterToOptions(parameters, JQueryOptions.ErrorPlacement, false);
				AddParameterToOptions(parameters, JQueryOptions.FocusCleanup, false);
				AddParameterToOptions(parameters, JQueryOptions.FocusInvalid, false);
				AddParameterToOptions(parameters, JQueryOptions.Highlight, false);
				AddParameterToOptions(parameters, JQueryOptions.Ignore, true);
				AddParameterToOptions(parameters, JQueryOptions.Messages, false);
				AddParameterToOptions(parameters, JQueryOptions.Meta, true);
				AddParameterToOptions(parameters, JQueryOptions.OnClick, false);
				AddParameterToOptions(parameters, JQueryOptions.OnFocusOut, false);
				AddParameterToOptions(parameters, JQueryOptions.OnKeyUp, false);
				AddParameterToOptions(parameters, JQueryOptions.OnSubmit, false);
				AddParameterToOptions(parameters, JQueryOptions.ShowErrors, false);
				AddParameterToOptions(parameters, JQueryOptions.SubmitHandler, false);
				AddParameterToOptions(parameters, JQueryOptions.Success, false);
				AddParameterToOptions(parameters, JQueryOptions.Unhighlight, false);
				AddParameterToOptions(parameters, JQueryOptions.Wrapper, true);
				AddParameterToOptions(parameters, JQueryOptions.IsAjax, false);

				AddCustomRules();
			}

			public override string CreateBeforeFormClosed(string formId)
			{
				var sb = new StringBuilder();
				sb.Append("$(document).ready(function() { ");
				sb.Append(Environment.NewLine);
				// Custom rules definitions
				if (_customRules.Count > 0)
				{
					foreach (CustomRule rule in _customRules.Values)
					{
						sb.Append("jQuery.validator.addMethod('");
						sb.Append(rule.Name);
						sb.Append("', ");
						sb.Append(rule.Rule);
						sb.Append(", '");
						sb.Append(rule.ViolationMessage);
						sb.Append("' );");
						sb.Append(Environment.NewLine);
					}
				}

				sb.AppendFormat("$(\"#{0}\")", formId);
				sb.Append(".validate({ ");
				sb.Append(Environment.NewLine);

				// Rules
				sb.Append("\trules : { ");
				sb.Append(Environment.NewLine);
				int current = 0;
				foreach (KeyValuePair<string, string> rule in this._rules)
				{
					sb.AppendFormat("\t\t{0}: {1}", rule.Key, rule.Value);
					if (current < this._rules.Count - 1)
					{
						sb.Append(",");
					}
					sb.Append(Environment.NewLine);
					current++;
				}
				sb.Append("\t}, ");
				sb.Append(Environment.NewLine);

				// Messages
				sb.Append("\tmessages : { ");
				sb.Append(Environment.NewLine);
				current = 0;
				foreach (KeyValuePair<string, string> message in this._messages)
				{
					sb.AppendFormat("\t\t{0}: {1}", message.Key, message.Value);
					if (current < this._messages.Count - 1)
					{
						sb.Append(",");
					}
					sb.Append(Environment.NewLine);
					current++;
				}
				sb.Append("\t} ");
				sb.Append(Environment.NewLine);

				sb.Append("});");
				sb.Append(Environment.NewLine);
				sb.Append("});"); // $(document).ready(function() 
				sb.Append(Environment.NewLine);
				return sb.ToString();
			}

			internal void AddRule(string target, string rule)
			{
				AddToJsDictionary(target, rule, this._rules);
			}

			internal void AddMessage(string target, string message)
			{
				AddToJsDictionary(target, message, this._messages);
			}

			internal void AddToJsDictionary(string target, string item, Dictionary<string, string> jsDictionary)
			{
				if (jsDictionary.ContainsKey(target))
				{
					string originalItem = jsDictionary[target];

					if (originalItem.StartsWith("{"))
					{
						originalItem = originalItem.Substring(1, originalItem.LastIndexOf("}") - 1);
					}
					jsDictionary[target] = string.Concat("{ ", originalItem, ", ", item, " }");
				}
				else
				{
					jsDictionary.Add(target, string.Concat("{ ", item, " }"));
				}
			}

			private void AddCustomRules()
			{
				AddCustomRule("notEqualTo", "Must not be equal to {0}.", "function(value, element, param) { return value != jQuery(param).val(); }");
				AddCustomRule("greaterThan", "Must be greater than {0}.", "function(value, element, param) { return ( IsNaN( value ) && IsNaN( jQuery(param).val() ) ) || ( value > jQuery(param).val() ); }");
				AddCustomRule("lesserThan", "Must be lesser than {0}.", "function(value, element, param) { return ( IsNaN( value ) && IsNaN( jQuery(param).val() ) ) || ( value < jQuery(param).val() ); }");

				string numberRegex = @"/^-?(?:\d+|\d{1,3}(?:\_group_\d{3})+)(?:\_separator_\d+)?$/"
					.Replace("_group_", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
					.Replace("_separator_", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
				AddCustomRule("numberNative", "Not a valid number."
							  , "function(value, element, param) { return this.optional(element) || " + numberRegex + ".test(value); }");
				string simpleDateRegex = @"/^\d{1,2}\_separator_\d{1,2}\_separator_\d{4}$/"
					.Replace("_separator_", CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator);
				AddCustomRule("simpleDate", "Not a valid date."
							  , "function(value, element, param) { return this.optional(element) || " + simpleDateRegex + ".test(value); }");
			}

			private void AddParameterToOptions(IDictionary parameters, string parameterName, bool quote)
			{
				string parameterValue = CommonUtils.ObtainEntryAndRemove(parameters, parameterName, null);

				if (parameterValue != null)
				{
					if (quote)
						if (!parameterValue.StartsWith("'") && !parameterValue.StartsWith("\""))
							_options.Add(parameterName, CommonUtils.SQuote(parameterValue));
						else
							_options.Add(parameterName, parameterValue);
					else
						_options.Add(parameterName, parameterValue);
				}
			}

			#region Nested classes

			/// <summary>
			/// Group definition.
			/// </summary>
			class Group
			{
				private readonly string _groupName;
				private readonly string _violationMessage;
				private readonly List<string> _groupItems = new List<string>();

				/// <summary>
				/// Initializes a new instance of the <see cref="Group"/> class.
				/// </summary>
				/// <param name="groupName">Name of the group.</param>
				/// <param name="violationMessage">The violation message.</param>
				public Group(string groupName, string violationMessage)
				{
					_groupName = groupName;
					_violationMessage = violationMessage;
				}

				/// <summary>
				/// Gets the name of the group.
				/// </summary>
				/// <value>The name of the group.</value>
				public string GroupName
				{
					get { return _groupName; }
				}

				/// <summary>
				/// Gets the violation message.
				/// </summary>
				/// <value>The violation message.</value>
				public string ViolationMessage
				{
					get { return _violationMessage; }
				}

				/// <summary>
				/// Gets the group items.
				/// </summary>
				/// <value>The group items.</value>
				public List<string> GroupItems
				{
					get { return _groupItems; }
				}

				/// <summary>
				/// Gets the group items.
				/// </summary>
				/// <returns></returns>
				public string GetFormattedGroupItems()
				{
					StringBuilder build = new StringBuilder();
					foreach (string groupItem in _groupItems)
					{
						build.AppendFormat("{0} ", groupItem.Replace('_', '.'));
					}
					return build.ToString();
				}

				/// <summary>
				/// Gets the group items.
				/// </summary>
				/// <returns></returns>
				public string GetFormattedGroup()
				{
					return string.Format("{0}: \"{1}\"", _groupName, GetFormattedGroupItems());
				}

				/// <summary>
				/// Gets the custom rule.
				/// </summary>
				/// <returns></returns>
				public string GetCustomRuleFunction()
				{
					StringBuilder builder = new StringBuilder();

					builder.Append(" function() { if(");
					for (int groupItem = 0; groupItem < _groupItems.Count; groupItem++)
					{
						string target = _groupItems[groupItem];

						builder.Append(string.Format("$(\"#{0}\").val()!=''", target));

						if (groupItem < _groupItems.Count - 1)
						{
							builder.Append(" || ");
						}
					}
					builder.Append(") { return true } else { return false; } }");

					return builder.ToString();
				}
			}

			class CustomRule
			{
				public readonly string Name;
				public readonly string Rule;
				public readonly string ViolationMessage;

				public CustomRule(string name, string rule, string violationMessage)
				{
					Name = name;
					Rule = rule;
					ViolationMessage = violationMessage;
				}
			}

			static class JQueryOptions
			{

				public const string Debug = "debug";
				public const string ErrorClass = "errorClass";
				public const string ErrorContainer = "errorContainer";
				public const string ErrorElement = "errorElement";
				public const string ErrorLabelContainer = "errorLabelContainer";
				public const string ErrorPlacement = "errorPlacement";
				public const string FocusCleanup = "focusCleanup";
				public const string FocusInvalid = "focusInvalid";
				public const string Highlight = "highlight";
				public const string Ignore = "ignore";
				public const string Messages = "messages";
				public const string Meta = "meta";
				public const string OnClick = "onclick";
				public const string OnFocusOut = "onfocusout";
				public const string OnKeyUp = "onkeyup";
				public const string OnSubmit = "onsubmit";
				public const string Rules = "rules";
				public const string ShowErrors = "showErrors";
				public const string SubmitHandler = "submitHandler";
				public const string Success = "success";
				public const string Unhighlight = "unhighlight";
				public const string Wrapper = "wrapper";
				public const string IsAjax = "isAjax";

			}

			#endregion Nested classes

		}

		#endregion Configuration
	}

	/// <summary>
	/// The <see cref="IBrowserValidationGenerator"/> implementation for the JQuery validate plugin.
	/// </summary>
	public class JQueryValidationGenerator : IBrowserValidationGenerator
	{
		#region Instance Variables

		readonly InputElementType _inputElementType;
		private readonly Func<string, string> _propertyNameToElementId;
		readonly JQueryValidator.JQueryConfiguration _config;

		#endregion Instance Variables

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="JQueryValidationGenerator"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="inputElementType">Type of the input element.</param>
		/// <param name="propertyNameToElementId">The attributes.</param>
		public JQueryValidationGenerator(JQueryValidator.JQueryConfiguration configuration, InputElementType inputElementType, Func<string, string> propertyNameToElementId)
		{
			_inputElementType = inputElementType;
			_propertyNameToElementId = propertyNameToElementId;
			_config = configuration;
		}

		#endregion Constructors


		private string GetPrefixedFieldld(string target, string field)
		{
			// Prefixes end with . by convention
			if (this._propertyNameToElementId != null)
			{
				return this._propertyNameToElementId(field).Replace(".", @"\\.");
			}
			else
			{
				return field;
			}
		}

		static string GetPrefixJQuerySelector(string target)
		{
			string selector = target;
			if (!selector.StartsWith("#"))
			{
				selector = string.Format("#{0}", selector);
			}
			// Strip single or double quotes from selector.
			return selector.Replace("\'", String.Empty).Replace("\"", String.Empty);
		}

		#region IBrowserValidationGenerator Members

		/// <summary>
		/// Set that a field should only accept digits.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetDigitsOnly(string target, string violationMessage)
		{
			_config.AddRule(target, "digits: true");
			_config.AddMessage(target, String.Format("digits: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Set that a field should only accept numbers.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetNumberOnly(string target, string violationMessage)
		{
			_config.AddRule(target, "numberNative: true");
			_config.AddMessage(target, String.Format("numberNative: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that a field is required.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetAsRequired(string target, string violationMessage)
		{
			_config.AddRule(target, "required: true");
			_config.AddMessage(target, String.Format("required: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that a field value must match the specified regular expression.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="regExp">The reg exp.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetRegExp(string target, string regExp, string violationMessage)
		{
		}

		/// <summary>
		/// Sets that a field value must be a valid email address.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetEmail(string target, string violationMessage)
		{
			_config.AddRule(target, "email: true");
			_config.AddMessage(target, String.Format("email: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must have an exact lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="length">The length.</param>
		public void SetExactLength(string target, int length)
		{
			SetExactLength(target, length, String.Format("Must be exactly {0} charcters", length));
		}

		/// <summary>
		/// Sets that field must have an exact lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="length">The length.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetExactLength(string target, int length, string violationMessage)
		{
			_config.AddRule(target, string.Format("rangelength: [{0}, {0}]", length));
			_config.AddMessage(target, String.Format("rangelength: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must have an minimum lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minLength">The minimum length.</param>
		public void SetMinLength(string target, int minLength)
		{
			SetMinLength(target, minLength, String.Format("Minimal {0} charcters", minLength));
		}

		/// <summary>
		/// Sets that field must have an minimum lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minLength">The minimum length.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetMinLength(string target, int minLength, string violationMessage)
		{
			_config.AddRule(target, string.Format("minlength: {0}", minLength));
			_config.AddMessage(target, String.Format("minlength: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must have an maximum lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="maxLength">The maximum length.</param>
		public void SetMaxLength(string target, int maxLength)
		{
			SetMaxLength(target, maxLength, String.Format("Maximal {0} charcters", maxLength));
		}

		/// <summary>
		/// Sets that field must have an maximum lenght.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="maxLength">The maximum length.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetMaxLength(string target, int maxLength, string violationMessage)
		{
			_config.AddRule(target, string.Format("maxlength: {0}", maxLength));
			_config.AddMessage(target, String.Format("maxlength: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must be between a length range.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minLength">The minimum length.</param>
		/// <param name="maxLength">The maximum length.</param>
		public void SetLengthRange(string target, int minLength, int maxLength)
		{
			SetLengthRange(target, minLength, maxLength, String.Format("Must be between {0} and {1} characters", minLength, maxLength));
		}

		/// <summary>
		/// Sets that field must be between a length range.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minLength">The minimum length.</param>
		/// <param name="maxLength">The maximum length.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetLengthRange(string target, int minLength, int maxLength, string violationMessage)
		{
			_config.AddRule(target, string.Format("rangelength: [{0}, {1}]", minLength, maxLength));
			_config.AddMessage(target, String.Format("rangelength: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must be between a value range.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minValue">Minimum value.</param>
		/// <param name="maxValue">Maximum value.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetValueRange(string target, int minValue, int maxValue, string violationMessage)
		{
			_config.AddRule(target, string.Format("range: [{0}, {1}]", minValue, maxValue));
			_config.AddMessage(target, String.Format("range: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must be between a value range.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minValue">Minimum value.</param>
		/// <param name="maxValue">Maximum value.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetValueRange(string target, decimal minValue, decimal maxValue, string violationMessage)
		{
			_config.AddRule(target, string.Format("range: [{0}, {1}]", minValue, maxValue));
			_config.AddMessage(target, String.Format("range: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that field must be between a value range.
		/// </summary>
		/// <remarks>This is not yet implemented in the JQuery validate plugin. It should be in next version: 1.2.3</remarks>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minValue">Minimum value.</param>
		/// <param name="maxValue">Maximum value.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetValueRange(string target, DateTime minValue, DateTime maxValue, string violationMessage)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Sets that field must be between a value range.
		/// </summary>
		/// <remarks>Note that the string values must only contains numbers.</remarks>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="minValue">Minimum value: a number as a string.</param>
		/// <param name="maxValue">Maximum value: a number as a string.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetValueRange(string target, string minValue, string maxValue, string violationMessage)
		{
			_config.AddRule(target, string.Format("range: [{0}, {1}]", minValue, maxValue));
			_config.AddMessage(target, String.Format("range: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Set that a field value must be the same as another field's value.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="comparisonFieldName">The name of the field to compare with.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetAsSameAs(string target, string comparisonFieldName, string violationMessage)
		{
			string prefixedComparisonFieldName = GetPrefixJQuerySelector(GetPrefixedFieldld(target, comparisonFieldName));
			_config.AddRule(target, String.Format("equalTo: \"{0}\"", prefixedComparisonFieldName));
			_config.AddMessage(target, String.Format("equalTo: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Set that a field value must _not_ be the same as another field's value.
		/// </summary>
		/// <remarks>Not implemented by the JQuery validate plugin.</remarks>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="comparisonFieldName">The name of the field to compare with.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetAsNotSameAs(string target, string comparisonFieldName, string violationMessage)
		{
			string prefixedComparisonFieldName = GetPrefixJQuerySelector(GetPrefixedFieldld(target, comparisonFieldName));
			_config.AddRule(target, String.Format("notEqualTo: \"{0}\"", prefixedComparisonFieldName));
			_config.AddMessage(target, String.Format("notEqualTo: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Set that a field value must be a valid date.
		/// </summary>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetDate(string target, string violationMessage)
		{
			_config.AddRule(target, "simpleDate: true");
			_config.AddMessage(target, String.Format("simpleDate: \"{0}\"", violationMessage));
		}

		/// <summary>
		/// Sets that a field's value must greater than another field's value.
		/// </summary>
		/// <remarks>
		/// Only numeric values can be compared for now. The JQuery validation plugin does not yet support dates comparison.
		/// </remarks>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="comparisonFieldName">The name of the field to compare with.</param>
		/// <param name="validationType">The type of data to compare.</param>
		/// <param name="violationMessage">The violation message.</param>
		/// <remarks>Not implemented by the JQuery validate plugin. Done via a custom rule.</remarks>
		public void SetAsGreaterThan(string target, string comparisonFieldName, IsGreaterValidationType validationType, string violationMessage)
		{
			if (validationType == IsGreaterValidationType.Decimal || validationType == IsGreaterValidationType.Integer)
			{
				string prefixedComparisonFieldName = GetPrefixJQuerySelector(GetPrefixedFieldld(target, comparisonFieldName));
				_config.AddRule(target, String.Format("greaterThan: \"{0}\"", prefixedComparisonFieldName));
				_config.AddMessage(target, String.Format("greaterThan: \"{0}\"", violationMessage));
			}
		}

		/// <summary>
		/// Sets that a field's value must be lesser than another field's value.
		/// </summary>
		/// <remarks>Not implemented by the JQuery validate plugin. Done via a custom rule.</remarks>
		/// <param name="target">The target name (ie, a hint about the controller being validated)</param>
		/// <param name="comparisonFieldName">The name of the field to compare with.</param>
		/// <param name="validationType">The type of data to compare.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetAsLesserThan(string target, string comparisonFieldName, IsGreaterValidationType validationType, string violationMessage)
		{
			if (validationType == IsGreaterValidationType.Decimal || validationType == IsGreaterValidationType.Integer)
			{
				string prefixedComparisonFieldName = GetPrefixJQuerySelector(GetPrefixedFieldld(target, comparisonFieldName));
				_config.AddRule(target, String.Format("lesserThan: \"{0}\"", prefixedComparisonFieldName));
				_config.AddMessage(target, String.Format("lesserThan: \"{0}\"", violationMessage));
			}
		}

		/// <summary>
		/// Sets that a flied is part of a group validation.
		/// </summary>
		/// <remarks>Not implemented by the JQuery validate plugin. Done via a custom rule.</remarks>
		/// <param name="target">The target.</param>
		/// <param name="groupName">Name of the group.</param>
		/// <param name="violationMessage">The violation message.</param>
		public void SetAsGroupValidation(string target, string groupName, string violationMessage)
		{
			//_config.AddValidateNotEmptyGroupItem(groupName, violationMessage, target);
			throw new NotImplementedException();
		}

		#endregion IBrowserValidationGenerator Members

	}
}