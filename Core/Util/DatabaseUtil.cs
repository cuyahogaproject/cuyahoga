using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

using NHibernate;
using NHibernate.Dialect;
using log4net;

using Cuyahoga.Core.Domain;
using Cuyahoga.Core.Service;

namespace Cuyahoga.Core.Util
{
	/// <summary>
	/// The DataBaseUtil class contains helper methods for misc. database related tasks.
	/// </summary>
	public class DatabaseUtil
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseUtil));

		private DatabaseUtil()
		{
		}

		/// <summary>
		/// Get the current database type.
		/// </summary>
		/// <returns></returns>
		public static DatabaseType GetCurrentDatabaseType()
		{
			ISessionFactory nhSessionFactory = GetNHibernateSessionFactory();
			if (nhSessionFactory.Dialect is MsSql2000Dialect)
			{
				return DatabaseType.MsSql2000;
			}
			else if (nhSessionFactory.Dialect is PostgreSQLDialect)
			{
				return DatabaseType.PostgreSQL;
			}
			else if (nhSessionFactory.Dialect is MySQLDialect)
			{
				return DatabaseType.MySQL;
			}
			throw new Exception("Unknown database type configured.");
		}

		/// <summary>
		/// Execute a given SQL script file.
		/// </summary>
		/// <param name="scriptFilePath"></param>
		public static void ExecuteSqlScript(string scriptFilePath)
		{
			log.Info("Executing script: " + scriptFilePath);
			string delimiter = GetDelimiter();
			StreamReader scriptFileStreamReader = new StreamReader(scriptFilePath);
			string completeScript = scriptFileStreamReader.ReadToEnd();

			ISessionFactory nhSessionFactory = GetNHibernateSessionFactory();
			IDbConnection connection = nhSessionFactory.ConnectionProvider.GetConnection();
			connection.Open();
			IDbTransaction transaction = connection.BeginTransaction();
			try
			{
				IDbCommand cmd = connection.CreateCommand();
				string[] sqlCommands = Regex.Split(completeScript, delimiter, RegexOptions.IgnoreCase);
				foreach (string sqlCommand in sqlCommands)
				{
					log.Info("Executing the follwing command: " + sqlCommand);
					cmd.CommandText = sqlCommand;
					cmd.ExecuteNonQuery();
				}

				log.Info("Committing transaction for script: " + scriptFilePath);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				log.Warn("Rolling back transaction for script: " + scriptFilePath);
				log.Error("An error occured while the following script: " + scriptFilePath, ex);
				transaction.Rollback();
			}
			finally
			{
				connection.Close();
				scriptFileStreamReader.Close();
			}
		}

		/// <summary>
		/// Get the version of the given assembly from the database.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static Version GetAssemblyVersion(string assembly)
		{
			Version version = null;

			ISessionFactory nhSessionFactory = GetNHibernateSessionFactory();
			IDbConnection connection = nhSessionFactory.ConnectionProvider.GetConnection();
			// TODO: create proper NHibernate mapping for version :).
			string sql = String.Format("SELECT major, minor, patch FROM cuyahoga_version WHERE assembly = '{0}'", assembly);
			log.Info("Version query: " + sql);
			IDbCommand cmd = connection.CreateCommand();
			cmd.CommandText = sql;

			//connection.Open();
			try
			{
				IDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					version = new Version(Convert.ToInt32(dr["major"]), Convert.ToInt32(dr["minor"]), Convert.ToInt32(dr["patch"]));
				}
				dr.Close();
			}
			catch (Exception ex)
			{
				log.Error(String.Format("An error occured while retrieving the version for {0}.", assembly), ex);
			}
			finally
			{
				connection.Close();
			}
			return version;
		}

		private static ISessionFactory GetNHibernateSessionFactory()
		{
			SessionFactory sf = SessionFactory.GetInstance();
			return sf.GetNHibernateFactory();
		}

		private static string GetDelimiter()
		{
			switch (GetCurrentDatabaseType())
			{
				case DatabaseType.MsSql2000:
					return "go";
				case DatabaseType.PostgreSQL:
				case DatabaseType.MySQL:
					return ";";
				default:
					throw new Exception("Unknown database type.");
			}
		}
	}
}
