using SimpleClassCreator.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SimpleClassCreator.DataAccess
{
	/// <summary>
	/// The base Data Access Layer
	/// All Data Access Layers should inherit from this base class
	/// </summary>
	public abstract class BaseRepository
	{
		private string _connectionString;

		public string ConnectionString { get; private set; }

		public BaseRepository()
		{

		}

		public BaseRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void ChangeConnectionString(string connectionString)
		{
			_connectionString = connectionString;
		}

		protected DataTable ExecuteDataTable(string sql)
		{
			if (string.IsNullOrWhiteSpace(sql))
				throw new Exception("SQL command cannot be null, blank or white space.");

			try
			{
				using (var con = new SqlConnection(_connectionString))
				{
					con.Open();

					using (var cmd = new SqlCommand(sql, con))
					{
						cmd.CommandTimeout = 0;

						var dt = new DataTable(); //Cannot be null

						//If you don't use SQL Server then change this to your flavor of DB Driver
						using (var a = new SqlDataAdapter())
						{
							a.SelectCommand = cmd;
							a.FillSchema(dt, SchemaType.Source);
							a.Fill(dt);

							return dt;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ex.Data.Add("Query", sql);

				throw;
			}

		}

		protected IDataReader ExecuteStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
		{
			var con = new SqlConnection(_connectionString);

			con.Open();

			var cmd = new SqlCommand(storedProcedure, con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 0;

			if (parameters.Any())
			{
				cmd.Parameters.AddRange(parameters);
			}

			var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

			return reader;
		}

		protected object ExecuteScalar(string sql)
		{
			using (var con = new SqlConnection(_connectionString))
			{
				con.Open();

				using (var cmd = new SqlCommand(sql, con))
				{
					cmd.CommandTimeout = 0;

					return cmd.ExecuteScalar();
				}
			}
		}

		public ConnectionResult TestConnectionString()
		{
			var result = new ConnectionResult();

			try
			{
				var obj = ExecuteScalar("SELECT 1;");

				result.Success = Convert.ToInt32(obj) == 1;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.ReturnedException = ex;
			}

			return result;
		}
	}
}
