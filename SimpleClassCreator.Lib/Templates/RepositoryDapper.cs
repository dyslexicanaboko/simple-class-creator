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
		public {{EntityName}} Select({{PrimaryKeyType}} {{PrimaryKey}})
		{
			var sql = @"
			SELECT
{{SelectAllList}}
			FROM {{Schema}}.{{Table}}
			WHERE {{PrimaryKey}} = @{{PrimaryKey}}";
			
			using (var connection = new SqlConnection(ConnectionString))
			{
				var lst = connection.Query<{{EntityName}}>(sql, new { {{PrimaryKey}} = {{PrimaryKey}}}).ToList();

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

			SELECT SCOPE_IDENTITY() AS PK;"; //This assumes int for now

			using (var connection = new SqlConnection(ConnectionString))
			{
				return connection.ExecuteScalar<{{PrimaryKeyType}}>(sql, entity);
			}
		}

		public void Update({{EntityName}} entity)
		{
			var sql = @"UPDATE {{Schema}}.{{Table}} SET 
{{UpdateParameters}}
            WHERE {{PrimaryKey}} = @{{PrimaryKey}}";

			using (var connection = new SqlConnection(ConnectionString))
			{
				var p = new DynamicParameters();
{{DynamicParameters}}

				connection.Execute(sql, p);
			}
		}
	}
}
