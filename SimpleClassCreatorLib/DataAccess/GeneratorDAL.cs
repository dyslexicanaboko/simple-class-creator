using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.DataAccess
{
    /// <summary>
    /// The Data Access layer for the Code Generator
    /// </summary>
    public class GeneratorDAL : DAL
    { 
        public GeneratorDAL(string connectionString) : base(connectionString)
        {

        }

        public DataTable GetSchema(string sqlQuery)
        {
            try
            {
                SQLText = sqlQuery;

                return ExecuteDataTable();
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);

                throw;
            }
        }

        public string GetPrimaryKeyColumn(string tableName)
        {
            string pkColumn = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    SQLText = "EXEC sp_pkeys '" + tableName + "';";

                    DataTable dt = ExecuteDataTable();

                    if (dt != null && dt.Rows.Count > 0)
                        pkColumn = Convert.ToString(dt.Rows[0]["COLUMN_NAME"]);
                }
                else
                    pkColumn = "PK";
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);

                throw;
            }

            return pkColumn;
        }
    }
}
