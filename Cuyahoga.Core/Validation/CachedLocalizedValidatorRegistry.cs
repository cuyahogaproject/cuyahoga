using System;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.Threading;
using Castle.Components.Validator;

namespace Cuyahoga.Core.Validation
{
	/// <summary>
	/// Validator registry that caches validator attributes. Also, validation messages are localized by translating the error
	/// message. The error message that comes from the attribute is the key for the error message.
	/// </summary>
	public class CachedLocalizedValidatorRegistry : ILocalizedValidatorRegistry
	{
		private static ResourceManager defaultResourceManager;
		private ResourceManager resourceManager;

		private readonly IDictionary propertiesPerType = Hashtable.Synchronized(new Hashtable());
		private readonly IDictionary attrsPerProperty = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// Initializes the <see cref="CachedLocalizedValidatorRegistry"/> class.
		/// </summary>
		static CachedLocalizedValidatorRegistry()
		{
			defaultResourceManager =
				new ResourceManager("Castle.Components.Validator.Messages",
									typeof(CachedValidationRegistry).Assembly);
		}

		/// <summary>
		/// Initializes the <see cref="CachedLocalizedValidatorRegistry"/> class.
		/// </summary>
		public CachedLocalizedValidatorRegistry()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="CachedLocalizedValidatorRegistry"/> class.
		/// </summary>
		/// <param name="resourceManager">The resource manager.</param>
		public CachedLocalizedValidatorRegistry(ResourceManager resourceManager)
		{
			this.resourceManager = resourceManager;
		}

		/// <summary>
		/// The resource manager
		/// </summary>
		public ResourceManager ResourceManager
		{
			get { return this.resourceManager; }
			set { this.resourceManager = value; }
		}


		/// <summary>
		/// Gets all validators associated with a <see cref="Type"/>.
		/// <para>
		/// The validators returned are initialized.
		/// </para>
		/// </summary>
		/// <param name="validatorRunner">The validator runner.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="runWhen">Restrict the set returned to the phase specified</param>
		/// <returns>A Validator array</returns>
		public IValidator[] GetValidators(IValidatorRunner validatorRunner, Type targetType, RunWhen runWhen)
		{
			PropertyInfo[] properties = (PropertyInfo[])propertiesPerType[targetType];

			if (properties == null)
			{
				this.propertiesPerType[targetType] = properties = ResolveProperties(targetType);
			}

			ArrayList list = new ArrayList();

			foreach (PropertyInfo prop in properties)
			{
				list.AddRange(GetValidators(validatorRunner, targetType, prop, runWhen));
			}

			return (IValidator[])list.ToArray(typeof(IValidator));
		}

		/// <summary>
		/// Gets all validators associated with a property.
		/// <para>
		/// The validators returned are initialized.
		/// </para>
		/// </summary>
		/// <param name="validatorRunner">The validator runner.</param>
		/// <param name="targetType">Target type.</param>
		/// <param name="property">The property.</param>
		/// <param name="runWhen">Restrict the set returned to the phase specified</param>
		/// <returns>A Validator array</returns>
		public IValidator[] GetValidators(IValidatorRunner validatorRunner, Type targetType, PropertyInfo property, RunWhen runWhen)
		{
			object[] builders = (object[])this.attrsPerProperty[property];

			if (builders == null)
			{
				builders = property.GetCustomAttributes(typeof(IValidatorBuilder), true);
				this.attrsPerProperty[property] = builders;
			}

			ArrayList validators = new ArrayList();

			foreach (IValidatorBuilder builder in builders)
			{
				IValidator validator = builder.Build(validatorRunner, targetType);

				if (!IsValidatorOnPhase(validator, runWhen)) continue;

				validator.Initialize(this, property);
				// Translate the error message of the validator. This way we can use the error message of a validator as resource key.
				string translatedErrorMessage = TranslateErrorMessage(validator.ErrorMessage);
				if (translatedErrorMessage != null)
				{
					validator.ErrorMessage = translatedErrorMessage;
				}
				validators.Add(validator);
			}

			return (IValidator[])validators.ToArray(typeof(IValidator));
		}

		public Accessor GetPropertyAccessor(PropertyInfo property)
		{
			return AccessorUtil.GetAccessor(property);
		}

		public Accessor GetFieldOrPropertyAccessor(Type targetType, string path)
		{
			return AccessorUtil.GetAccessor(targetType, path);
		}

		/// <summary>
		/// Gets the string from resource by key
		/// </summary>
		/// <param name="key">The key.</param>
		/// <remarks>Only used when there is no error message given via the attribute.</remarks>
		/// <returns></returns>
		public string GetStringFromResource(string key)
		{
			return defaultResourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
		}

		/// <summary>
		/// Gets a translated error message from the resource manager.
		/// </summary>
		/// <param name="originalMessage"></param>
		/// <remarks></remarks>
		/// <returns></returns>
		public string TranslateErrorMessage(string originalMessage)
		{
			if (resourceManager != null)
			{
				string result = resourceManager.GetString(originalMessage, Thread.CurrentThread.CurrentUICulture);
				if (result != null)
					return result;
			}
			return null;
		}

		/// <summary>
		/// Resolve properties that will be inspected for registered validators
		/// </summary>
		/// <param name="targetType">the type to examinate properties for</param>
		/// <returns>resolved properties</returns>
		protected virtual PropertyInfo[] ResolveProperties(Type targetType)
		{
			return targetType.GetProperties();
		}

		private static bool IsValidatorOnPhase(IValidator validator, RunWhen when)
		{
			if (validator.RunWhen == RunWhen.Everytime) return true;

			return ((validator.RunWhen & when) != 0);
		}
	}
}
