using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
	public class RepositoryStaticGenerator
		: GeneratorBase
	{
		public RepositoryStaticGenerator(ClassInstructions instructions)
			: base(instructions, "RepositoryStatic.cs")
		{
		}

		public override GeneratedResult FillTemplate()
		{
			var strTemplate = GetTemplate(TemplateName);

			var template = new StringBuilder(strTemplate);

			/* Context
			 * ClassName: Refers to the name of THIS class that is being generated "Table1Repository.cs"
			 * EntityName: Refers to the existing source Entity Class assumed to have been generated already "Table1Entity.cs"
			 * ModelName: Refers to the existing Model Class that compliments the Entity Class "Table1Model.cs" */

			template.Replace("{{Namespace}}", Instructions.Namespace);
			template.Replace("{{ClassName}}", Instructions.EntityName); //Prefix of the repository class name
			template.Replace("{{EntityName}}", Instructions.ClassEntityName); //Class entity name
			template.Replace("{{Namespaces}}", FormatNamespaces(Instructions.Namespaces));

			var t = template.ToString();

			t = RemoveExcessBlankSpace(t);
			//t = RemoveBlankLines(t);

			var pk = Instructions.Properties.SingleOrDefault(x => x.IsPrimaryKey);
			var lstNoPk = Instructions.Properties.Where(x => !x.IsPrimaryKey).ToList();

			//TODO: What to do when there is no primary key?
			if (pk != null)
			{
				t = t.Replace("{{PrimaryKey}}", pk.ColumnName);
				t = t.Replace("{{PrimaryKeyType}}", pk.SystemTypeAlias);
				t = t.Replace("{{PrimaryKeySqlParameter}}", FormatSqlParameter(pk));
			}

			t = t.Replace("{{Schema}}", Instructions.TableQuery.Schema);
			t = t.Replace("{{Table}}", Instructions.TableQuery.Table);
			t = t.Replace("{{SelectAllList}}", FormatSelectList(Instructions.Properties));
			t = t.Replace("{{InsertColumnList}}", FormatSelectList(lstNoPk));
			t = t.Replace("{{InsertValuesList}}", FormatSelectList(lstNoPk, "@"));
			t = t.Replace("{{UpdateParameters}}", FormatUpdateList(lstNoPk));
			t = t.Replace("{{SqlParameters}}", FormatSqlParameterList(lstNoPk));
			t = t.Replace("{{SetProperties}}", FormatSetProperties(Instructions.Properties));

			var r = GetResult();
			r.Filename = Instructions.EntityName + "Repository.cs";
			r.Contents = t;

			return r;
		}

		private string FormatSelectList(IList<ClassMemberStrings> properties, string prefix = null)
		{
			var content = GetTextBlock(properties,
				p => $"                {prefix}{p.Property}",
				"," + Environment.NewLine);

			return content;
		}

		private string FormatUpdateList(IList<ClassMemberStrings> properties)
		{
			var content = GetTextBlock(properties,
				p => $"                {p.Property} = @{p.Property}",
				"," + Environment.NewLine);

			return content;
		}

		private string FormatSqlParameterList(IList<ClassMemberStrings> properties)
		{
			var content = GetTextBlock(properties,
				p => $@"{FormatSqlParameter(p)}
								  
			lst.Add(p);",
				Environment.NewLine);

			return content;
		}

		private string FormatSqlParameter(ClassMemberStrings properties)
		{
			var t = properties.DatabaseType;

			var content = $@"p = new SqlParameter();
			p.ParameterName = ""@{properties.Property}"";
			p.SqlDbType = SqlDbType.{t};
			p.Value = entity.{properties.Property};";

			//TODO: Need to work through every type to see what the combinations are
			if (t == SqlDbType.DateTime2)
			{
				content += Environment.NewLine + $"            p.Scale = {properties.Scale};";
			}

			if (t == SqlDbType.Decimal)
			{
				content += Environment.NewLine +
$@"            p.Scale = {properties.Scale};
			p.Precision = {properties.Precision};";
			}

			if (t == SqlDbType.VarChar ||
				t == SqlDbType.NVarChar ||
				t == SqlDbType.Char ||
				t == SqlDbType.NChar)
			{
				content += Environment.NewLine + $"            p.Size = {properties.Size}";
			}

			return content;
		}

		private string FormatSetProperties(IList<ClassMemberStrings> properties)
		{
			var content = GetTextBlock(properties,
				p => $"            {FormatSetProperty(p)}",
				Environment.NewLine);

			return content;
		}

		private string FormatSetProperty(ClassMemberStrings properties)
		{
			var p = properties;

			//Examples
			//r["IntValue"] = Convert.ToInt32(r["IntValue"];
			//r["NullableIntValue"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["NullableIntValue"]);
			//r["GuidValue"] = Guid.Parse(Convert.ToString(r["GuidValue")];
			//r["NullableGuidValue"] == DBNull.Value ? null : (Guid?)Guid.Parse(Convert.ToString(r["NullableGuidValue"]));

			var dr = $"r[\"{p.ColumnName}\"]";
			
			var method = string.Format(p.ConversionMethodSignature, dr);

			var content = $"e.{p.Property} = ";

			if (p.IsDbNullable)
			{
				//The Alias already has the question mark suffix if nullable
				content += $"{dr} == DBNull.Value ? null : ({p.SystemTypeAlias})";
			}
				
			content += $"{method};";

			return content;
		}
	}
}