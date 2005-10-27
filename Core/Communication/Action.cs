using System;

namespace Cuyahoga.Core.Communication
{
	/// <summary>
	/// The Action class describes a single action that a module can perform when calling or
	/// called from other modules.
	/// </summary>
	public class Action
	{
		private string _name;
		private string[] _parameters;
		
		/// <summary>
		/// The name of the action.
		/// </summary>
		public string Name
		{
			get { return this._name; }
		}

		/// <summary>
		/// The list of parameters that are allowed for the action. 
		/// NOTE: this is kind of loosely coupled. Only the parameter names are required.
		/// No type specification is required. The modules have to take care of validating
		/// the parameters themselves.
		/// </summary>
		public string[] Parameters
		{
			get { return this._parameters; }
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Action(string name, string[] parameters)
		{
			this._name = name;
			this._parameters = parameters;
		}

		/// <summary>
		/// Equals override.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Action)
			{
				bool isEqual = true;
				Action otherAction = (Action)obj;
				if (this._name != otherAction.Name)
				{
					isEqual = false;
				}
				else
				{
					if (this._parameters == null)
					{
						isEqual = otherAction.Parameters == null;
					}
					else
					{
						if (this._parameters.Length == otherAction.Parameters.Length)
						{
							for (int i = 0; i < this._parameters.Length; i++)
							{
								if (this._parameters[i] != otherAction.Parameters[i])
								{
									isEqual = false;
								}
							}
						}
						else
						{
							isEqual = false;
						}
					}
				}
				return isEqual;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// GetHashCode override.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this._name.GetHashCode() ^ this._parameters.GetHashCode();
		}


	}
}
