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
		public {{EntityName}} Select({{PrimaryKeyType}} {{PrimaryKey}})
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
		public int Insert({{EntityName}} entity)
		{
			var sql = @"INSERT INTO {{Schema}}.{{Table}} (
{{InsertColumnList}}) VALUES (
{{InsertValuesList}});

			 SELECT SCOPE_IDENTITY() AS PK;"; //This assumes int for now

			var lst = GetParameters(entity);

			using (var dr = ExecuteReaderText(sql, lst.ToArray()))
			{
				//This assumes int for now
				return Convert.ToInt32(GetScalar(dr, "PK"));
			}
		}

		public void Update({{EntityName}} entity)
		{
			var sql = @"UPDATE {{Schema}}.{{Table}} SET 
{{UpdateParameters}}
					WHERE {{PrimaryKey}} = @{{PrimaryKey}}";

			var lst = GetParameters(entity);

			var p = GetPrimaryKeyParameter(entity.{{PrimaryKey}});

			lst.Add(p);

			ExecuteNonQuery(sql, lst.ToArray());
		}
		
		private SqlParameter GetPrimaryKeyParameter({{EntityName}} entity)
		{
			SqlParameter p = null;
			
			{{PrimaryKeySqlParameter}}

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
