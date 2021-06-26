using System;
using System.Data;
using System.Data.SqlClient;
using SimpleClassCreator.Models;

namespace SimpleClassCreator.DataAccess
{
    /// <summary>
    /// The Data Access layer for the Code Generator
    /// </summary>
    public class QueryToClassRepository
        : BaseRepository, IQueryToClassRepository
    {
        public DataTable GetSchema(string query)
        {
            try
            {
                return ExecuteDataTable(query);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);

                throw;
            }
        }

        public string GetPrimaryKeyColumn(TableQuery tableQuery)
        {
            try
            {
                var pSchema = new SqlParameter("@table_owner", tableQuery.Schema);
                pSchema.SqlDbType = SqlDbType.NVarChar;
                pSchema.Size = 256;

                var pTable = new SqlParameter("@table_name", tableQuery.Table);
                pTable.SqlDbType = SqlDbType.NVarChar;
                pTable.Size = 256;

                using (var dr = ExecuteStoredProcedure("sys.sp_pkeys", pTable, pSchema))
                {
                    dr.Read();

                    var pk = Convert.ToString(dr["COLUMN_NAME"]);

                    return pk;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);

                throw;
            }
        }
    }
}
