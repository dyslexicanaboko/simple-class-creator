//https://www.nuget.org/packages/Dapper/
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
{{Namespaces}}

namespace {{Namespace}}
{
    public class {{ClassName}}Repository
    {
		public {{EntityName}} Select({{PrimaryKeyType}} {{PrimaryKeyParameter}})
		{
			var sql = @"
			SELECT
{{SelectAllList}}
			FROM {{Schema}}.{{Table}}
			WHERE {{PrimaryKeyColumn}} = @{{PrimaryKeyProperty}}";
			
			using (var connection = new SqlConnection(ConnectionString))
			{
				var lst = connection.Query<{{EntityName}}>(sql, new { {{PrimaryKeyProperty}} = {{PrimaryKeyParameter}}}).ToList();

				if (!lst.Any()) return null;

				var entity = lst.Single();

				return entity;
			}
		}

		public IEnumerable<{{EntityName}}> SelectAll()
		{
			var sql = @"
			SELECT
{{SelectAllList}}
			FROM {{Schema}}.{{Table}}";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var lst = connection.Query<{{EntityName}}>(sql).ToList();

				return lst;
			}
		}

		//Preference on whether or not insert method returns a value is up to the user and the object being inserted
		public {{PrimaryKeyType}} Insert({{EntityName}} entity)
		{
			var sql = @"INSERT INTO {{Schema}}.{{Table}} (
{{InsertColumnList}}
            ) VALUES (
{{InsertValuesList}});
{{ScopeIdentity}}";

			using (var connection = new SqlConnection(ConnectionString))
			{
{{PrimaryKeyInsertExecution}}
			}
		}

		public void Update({{EntityName}} entity)
		{
			var sql = @"UPDATE {{Schema}}.{{Table}} SET 
{{UpdateParameters}}
            WHERE {{PrimaryKeyColumn}} = @{{PrimaryKeyProperty}}";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var p = new DynamicParameters();
{{DynamicParameters}}

				connection.Execute(sql, p);
			}
		}
	}
}
