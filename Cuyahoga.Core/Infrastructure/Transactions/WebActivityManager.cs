using System;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Castle.Services.Transaction;

namespace Cuyahoga.Core.Infrastructure.Transactions
{
	public class WebActivityManager : MarshalByRefObject, IActivityManager
	{
		private const string Key = "Castle.Services.Transaction.WebActivity";

		private readonly object lockObj = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="WebActivityManager"/> class.
		/// </summary>
		public WebActivityManager()
		{
		}

		#region MarshalByRefObject

		///<summary>
		///Obtains a lifetime service object to control the lifetime policy for this instance.
		///</summary>
		///
		///<returns>
		///An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"></see> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"></see> property.
		///</returns>
		///
		///<exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" /></PermissionSet>
		public override object InitializeLifetimeService()
		{
			return null;
		}

		#endregion

		/// <summary>
		/// Gets the current activity.
		/// </summary>
		/// <value>The current activity.</value>
		public Activity CurrentActivity
		{
			get
			{
				lock(lockObj)
				{
					Activity activity = ObtainActivity();

					if (activity == null)
					{
						activity = new Activity();
						StoreActivity(activity);
					}

					return activity;
				}
			}
		}

		private void StoreActivity(Activity activity)
		{
			EnsureHttpContext();
			HttpContext.Current.Items[Key] = activity;
		}

		private Activity ObtainActivity()
		{
			EnsureHttpContext();
			return (HttpContext.Current.Items[Key] as Activity);			
		}

		private void EnsureHttpContext()
		{
			if (HttpContext.Current == null)
			{
				throw new InvalidOperationException("Unable to obtain the current HttpContext that is required for the activity manager.");
			}
		}
	}

}
