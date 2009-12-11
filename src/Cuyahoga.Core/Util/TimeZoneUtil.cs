using System;
using System.Collections;
using System.Security.Principal;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// The TimeZone class provides helper methods for TimeZone specific issues.
	/// </summary>
	public class TimeZoneUtil
	{
		private TimeZoneUtil()
		{
		}

		/// <summary>
		/// Get the current server time zone offset, corrected for daylight savings.
		/// </summary>
		/// <returns>The timezone offset in minutes.</returns>
		/// <remarks>
		/// Solution found on http://communityserver.org/forums/486891/ShowPost.aspx.
		/// </remarks>
		public static int GetMachineTimeZoneOffset()
		{
			DateTime d = DateTime.Now;
			DateTime d1 = d.ToUniversalTime();
			if(System.TimeZone.CurrentTimeZone.IsDaylightSavingTime(d))
			{
				d1 = d1.AddHours(1);
			}

			double offset = d.Subtract(d1).TotalMinutes;
			return Convert.ToInt32(offset);
		}

		/// <summary>
		/// Adjust a date to the timezone of the current user.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static DateTime AdjustDateToUserTimeZone(DateTime dt, IIdentity user)
		{
			User cuyahogaUser = user as User;
			if (cuyahogaUser != null)
			{
				return dt.AddMinutes(cuyahogaUser.TimeZone - GetMachineTimeZoneOffset());
			}
			else
			{
				return dt;
			}
		}

		/// <summary>
		/// Adjust a date from the timezone of the current user to the timezone of
		/// the server.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static DateTime AdjustDateToServerTimeZone(DateTime dt, IIdentity user)
		{
			User cuyahogaUser = user as User;
			if (cuyahogaUser != null)
			{
				return dt.AddMinutes(GetMachineTimeZoneOffset() - cuyahogaUser.TimeZone);
			}
			else
			{
				return dt;
			}
		}

		/// <summary>
		/// Get a list of available timezones.
		/// </summary>
		/// <returns></returns>
		public static SortedList GetTimeZones()
		{
			SortedList timeZones = new SortedList(30);

			timeZones.Add(-720, "(GMT - 12:00) Enitwetok, Kwajalien");
			timeZones.Add(-660, "(GMT - 11:00) Midway Island, Samoa");
			timeZones.Add(-600, "(GMT - 10:00) Hawaii");
			timeZones.Add(-540, "(GMT - 9:00) Alaska");
			timeZones.Add(-480, "(GMT - 8:00) Pacific Time (US & Canada)");
			timeZones.Add(-420, "(GMT - 7:00) Mountain Time (US & Canada)");
			timeZones.Add(-360, "(GMT - 6:00) Central Time (US & Canada), Mexico City");
			timeZones.Add(-300, "(GMT - 5:00) Eastern Time (US & Canada), Bogota, Lima, Quito");
			timeZones.Add(-240, "(GMT - 4:00) Atlantic Time (Canada), Caracas, La Paz");
			timeZones.Add(-210, "(GMT - 3:30) Newfoundland");
			timeZones.Add(-180, "(GMT - 3:00) Brazil, Buenos Aires, Georgetown, Falkland Is.");
			timeZones.Add(-120, "(GMT - 2:00) Mid-Atlantic, Ascention Is., St Helena");
			timeZones.Add(-60, "(GMT - 1:00) Azores, Cape Verde Islands");
			timeZones.Add(0, "(GMT) Casablanca, Dublin, Edinburgh, London, Lisbon, Monrovia");
			timeZones.Add(60, "(GMT + 1:00) Amsterdam, Berlin, Brussels, Madrid, Paris, Rome");
			timeZones.Add(120, "(GMT + 2:00) Kaliningrad, South Africa, Warsaw");
			timeZones.Add(180, "(GMT + 3:00) Baghdad, Riyadh, Moscow, Nairobi");
			timeZones.Add(210, "(GMT + 3:30) Tehran");
			timeZones.Add(240, "(GMT + 4:00) Adu Dhabi, Baku, Muscat, Tbilisi");
			timeZones.Add(270, "(GMT + 4:30) Kabul");
			timeZones.Add(300, "(GMT + 5:00) Ekaterinburg, Islamabad, Karachi, Tashkent");
			timeZones.Add(330, "(GMT + 5:30) Bombay, Calcutta, Madras, New Delhi");
			timeZones.Add(360, "(GMT + 6:00) Almaty, Colomba, Dhakra");
			timeZones.Add(420, "(GMT + 7:00) Bangkok, Hanoi, Jakarta");
			timeZones.Add(480, "(GMT + 8:00) Beijing, Hong Kong, Perth, Singapore, Taipei");
			timeZones.Add(540, "(GMT + 9:00) Osaka, Sapporo, Seoul, Tokyo, Yakutsk");
			timeZones.Add(570, "(GMT + 9:30) Adelaide, Darwin");
			timeZones.Add(600, "(GMT + 10:00) Melbourne, Papua New Guinea, Sydney, Vladivostok");
			timeZones.Add(660, "(GMT + 11:00) Magadan, New Caledonia, Solomon Islands");
			timeZones.Add(720, "(GMT + 12:00) Auckland, Wellington, Fiji, Marshall Island");

			return timeZones;
		}
	}
}
