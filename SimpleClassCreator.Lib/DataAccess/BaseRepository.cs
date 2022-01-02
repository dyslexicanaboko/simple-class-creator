using SimpleClassCreator.Lib.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SimpleClassCreator.Lib.DataAccess
{
    /// <summary>
    ///     The base Data Access Layer
    ///     All Data Access Layers should inherit from this base class
    /// </summary>
    public abstract class BaseRepository
        : IBaseRepository
    {
        private string _connectionString;

        public void ChangeConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SchemaRaw GetFullSchemaInformation(string sql)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    con.Open();

                    var rs = new SchemaRaw();

                    using (var dr = cmd.ExecuteReader())
                    {
                        var tblSqlServer = dr.GetSchemaTable();

                        var tblGeneric = new DataTable();
                        tblGeneric.Load(dr);
    
                        rs.SqlServerSchema = tblSqlServer;
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var tblGeneric = new DataTable();
                        
                        da.FillSchema(tblGeneric, SchemaType.Source);

                        rs.GenericSchema = tblGeneric;
                    }

                    return rs;
                }
            }
        }

        protected IDataReader ExecuteStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
        {
            var con = new SqlConnection(_connectionString);

            con.Open();

            var cmd = new SqlCommand(storedProcedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            if (parameters.Any()) cmd.Parameters.AddRange(parameters);

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

        protected virtual DataTable ExecuteDataTable(string sql)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandTimeout = 0;

                    var dt = new DataTable("Table1");

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    return dt;
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