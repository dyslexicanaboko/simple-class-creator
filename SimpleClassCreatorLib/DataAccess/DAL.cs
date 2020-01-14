using System;
using System.Configuration;
using System.Data;
//If you don't use SQL Server then change this to your flavor of DB Driver
using System.Data.SqlClient;
using SimpleClassCreator.DTO;

namespace SimpleClassCreator.DataAccess
{
    /// <summary>
    /// The base Data Access Layer (Object)
    /// All Data Access Layers should inherit from this base class
    /// </summary>
	public class DAL
	{
		#region Members
        private IDbConnection _serverConnection;

		private string _sqltext;
		protected string SQLText 
		{
			set 
			{	
				if(value != "")
					_sqltext = value;
				else
					throw new InvalidOperationException("Invalid Query");
			}
			//this is in for testing only
			get	{	return _sqltext;	}
		}

        private string _connectionString = string.Empty;
        
        //This property is for reference ONLY - it will not include the password
        private string _connectionStringRefOnly;
        public string ConnectionString 
        {
            get { return _connectionStringRefOnly; }
        }
		
		private void EstablishConnection()
		{
            _serverConnection = new SqlConnection(_connectionString);

            _serverConnection.Open();

            //Grab this while the connection is still open
            _connectionStringRefOnly = _serverConnection.ConnectionString;
		}

		private void CleanUpConnection()
		{
            _serverConnection.Close();
            _serverConnection.Dispose();
            _serverConnection = null;
		}
		#endregion

        //private string _connectPropertyDataBase;
        //protected string ConnectPropertyDataBase
        //{
        //    get { return _connectPropertyDataBase; }
        //    set { _connectPropertyDataBase = value; }
        //}

        public DAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public DAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbCommand SQLCommand(string sqlString, IDbConnection dbConnection)
        {
            //If you don't use SQL Server then change this to your flavor of DB Driver
            IDbCommand genericCommand = new SqlCommand(sqlString, (SqlConnection)dbConnection);

            return genericCommand;
        }

        private DataTable GetDataTable(IDbCommand dbCommand)
        {
            DataTable dt = new DataTable(); //Cannot be null

            //If you don't use SQL Server then change this to your flavor of DB Driver
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)dbCommand;
            adapter.FillSchema(dt, SchemaType.Source);
            adapter.Fill(dt);
            adapter.Dispose();

            return dt;
        }

        protected DataTable ExecuteDataTable()
        {
            string strQueryCommand;

            DataTable dt = new DataTable();

            strQueryCommand = SQLText;

            if(string.IsNullOrWhiteSpace(strQueryCommand))
                throw new Exception("invalid SQL Command");
            
            try
            {
                EstablishConnection();

                IDbCommand cmd = SQLCommand(strQueryCommand, _serverConnection);

                cmd.CommandTimeout = 0;

                dt = GetDataTable(cmd);

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                ex.Data.Add("Query", strQueryCommand);

                throw;
            }
            finally
            {
                CleanUpConnection();
            }

            return dt;
        }

        public static ConnectionResult TestConnectionString(string connectionString)
        {
            ConnectionResult obj = new ConnectionResult();

            try
            {
                DAL dal = new DAL(connectionString);

                dal.SQLText = "SELECT 1;";

                DataTable dt = dal.ExecuteDataTable();

                obj.Success = (dt != null && dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                obj.Success = false;
                obj.ReturnedException = ex; 
            }

            return obj;
        }
	}
}
