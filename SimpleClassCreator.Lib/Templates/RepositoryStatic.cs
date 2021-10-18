using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
{{Namespaces}}

namespace {{Namespace}}
{
	//Should include interface?
    public class {{ClassName}}Repository
    {
		public {{ClassName}} Select({{PrimaryKeyType}} {{PrimaryKey}})
		{
			var sql = @"
			SELECT
{{SelectAllList}}
			FROM {{Schema}}.{{Table}}
			WHERE {{PrimaryKey}} = @{{PrimaryKey}}";

			var p = GetPrimaryKeyParameter({{PrimaryKey}});

			using (var dr = ExecuteReaderText(sql, p))
			{
				var lst = ToList(dr, ToEntity);

				if (!lst.Any()) return null;

				var entity = lst.Single();

				return entity;
			}
		}

		public IEnumerable<{{ClassName}}> SelectAll()
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
		public int Insert({{ClassName}} entity)
		{
			var sql = @"INSERT INTO {{Schema}}.{{Table}} (
{{InsertColumnList}}) VALUES (
{{InsertValuesList}});

			 SELECT SCOPE_IDENTITY() AS PK;";

			var lst = GetParameters(entity);

			using (var dr = ExecuteReaderText(sql, lst.ToArray()))
			{
				return Convert.ToInt32(GetScalar(dr, "PK"));
			}
		}

		public void Update({{ClassName}} entity)
		{
			var sql = @"UPDATE {{Schema}}.{{Table}} SET 
{{UpdateParameters}}
					WHERE {{PrimaryKey}} = @{{PrimaryKey}}";

			var lst = GetParameters(entity);

			var p = GetPrimaryKeyParameter(entity.{{PrimaryKey}});

			lst.Add(p);

			ExecuteNonQuery(sql, lst.ToArray());
		}
		
		private SqlParameter GetPrimaryKeyParameter({{PrimaryKeyType}} {{PrimaryKey}})
		{
			var p = new SqlParameter();
			
			{{PrimaryKeySqlParameter}}

			return p;
		}

		private List<SqlParameter> GetParameters({{ClassName}} entity)
		{
			SqlParameter p = null;

			var lst = new List<SqlParameter>();
			
{{SqlParameters}}
			
			return lst;
		}
		
		private {{ClassName}} ToEntity(IDataReader reader)
		{
			var r = reader;

			var e = new {{ClassName}}();
{{SetProperties}}

			return e;
		}
	}
}
