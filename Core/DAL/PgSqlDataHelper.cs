using System;
using System.Data;
using Npgsql;

namespace Cuyahoga.Core.DAL
{
	/// <summary>
	/// Summary description for PgSqlDataHelper.
	/// </summary>
	public class PgSqlDataHelper
	{
		public PgSqlDataHelper()
		{
		}

		public static NpgsqlParameter MakeInParam(string paramName, object value)
		{
			return new NpgsqlParameter(paramName, value);
		}

		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="paramName">Name of param.</param>
		/// <param name="dbType">Param type.</param>
		/// <param name="size">Param size.</param>
		/// <param name="value">Param value.</param>
		/// <returns>New parameter.</returns>
		public static NpgsqlParameter MakeInParam(string paramName, DbType dbType, int size, object value) 
		{
			return MakeParam(paramName, dbType, size, ParameterDirection.Input, value);
		}		

		/// <summary>
		/// Make input param.
		/// </summary>
		/// <param name="paramName">Name of param.</param>
		/// <param name="dbType">Param type.</param>
		/// <param name="size">Param size.</param>
		/// <returns>New parameter.</returns>
		public static NpgsqlParameter MakeOutParam(string paramName, DbType dbType, int size) 
		{
			return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
		}		

		/// <summary>
		/// Make query parameter.
		/// </summary>
		/// <param name="paramName">Name of param.</param>
		/// <param name="dbType">Param type.</param>
		/// <param name="size">Param size.</param>
		/// <param name="direction">Parm direction.</param>
		/// <param name="value">Param value.</param>
		/// <returns>New parameter.</returns>
		public static NpgsqlParameter MakeParam(string paramName, DbType dbType, int size, ParameterDirection direction, object value) 
		{
			NpgsqlParameter param;

			if(size > 0)
				param = new NpgsqlParameter(paramName, dbType, size);
			else
				param = new NpgsqlParameter(paramName, dbType);

			param.Direction = direction;
			if (!(direction == ParameterDirection.Output && value == null))
				param.Value = value;

			return param;
		}
	}
}
