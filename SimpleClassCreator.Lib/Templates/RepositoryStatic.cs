using Microsoft.Data.SqlClient;
using System;
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

			var p = GetPrimaryKeyParameter({{PrimaryKeyParameter}});

			using (var dr = ExecuteReaderText(sql, p))
			{
				var lst = ToList(dr, ToEntity);

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

			using (var dr = ExecuteReaderText(sql))
			{
				var lst = ToList(dr, ToEntity);

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

			var lst = GetParameters(entity);

{{PrimaryKeyInsertExecution}}
		}

		public void Update({{EntityName}} entity)
		{
			var sql = @"UPDATE {{Schema}}.{{Table}} SET 
{{UpdateParameters}}
					WHERE {{PrimaryKeyColumn}} = @{{PrimaryKeyProperty}}";

			var lst = GetParameters(entity);

			var p = GetPrimaryKeyParameter(entity.{{PrimaryKeyProperty}});

			lst.Add(p);

			ExecuteNonQuery(sql, lst.ToArray());
		}
		
		private SqlParameter GetPrimaryKeyParameter({{PrimaryKeyType}} {{PrimaryKeyParameter}})
		{
			var p = new SqlParameter();
			p.ParameterName = ""@{{PrimaryKeyProperty}}"";
			p.SqlDbType = SqlDbType.{{PrimaryKeySqlDbType}};
			p.Value = {{PrimaryKeyParameter}};
			
			return p;
		}

		private List<SqlParameter> GetParameters({{EntityName}} entity)
		{
			SqlParameter p = null;

			var lst = new List<SqlParameter>();
			
{{SqlParameters}}
			
			return lst;
		}
		
		private {{EntityName}} ToEntity(IDataReader reader)
		{
			var r = reader;

			var e = new {{EntityName}}();
{{SetProperties}}

			return e;
		}
	}
}
