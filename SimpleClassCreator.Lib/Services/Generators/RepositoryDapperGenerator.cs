﻿using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
	public class RepositoryDapperGenerator
		: GeneratorBase
	{
		public RepositoryDapperGenerator(ClassInstructions instructions)
			: base(instructions, "RepositoryDapper.cs")
		{
		}

		public override GeneratedResult FillTemplate()
		{
			var strTemplate = GetTemplate(TemplateName);

			var template = new StringBuilder(strTemplate);

			template.Replace("{{Namespace}}", Instructions.Namespace);
			template.Replace("{{ClassName}}", Instructions.EntityName); //Prefix of the repository class
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
			}

			t = t.Replace("{{Schema}}", Instructions.TableQuery.Schema);
			t = t.Replace("{{Table}}", Instructions.TableQuery.Table);
			t = t.Replace("{{SelectAllList}}", FormatSelectList(Instructions.Properties));
			t = t.Replace("{{InsertColumnList}}", FormatSelectList(lstNoPk));
			t = t.Replace("{{InsertValuesList}}", FormatSelectList(lstNoPk, "@"));
			t = t.Replace("{{UpdateParameters}}", FormatUpdateList(lstNoPk));
			t = t.Replace("{{DynamicParameters}}", FormatDynamicParameterList(Instructions.Properties));

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

		private string FormatDynamicParameterList(IList<ClassMemberStrings> properties)
		{
			var content = GetTextBlock(properties, 
				p => $"{FormatDynamicParameter(p)}",
				Environment.NewLine);

			return content;
		}

		private string FormatDynamicParameter(ClassMemberStrings properties)
		{
			var t = properties.DatabaseType;
			var dbType = TypesService.MapSqlDbTypeToDbTypeLoose[t];

			var lst = new List<string>
			{
				$"name: \"@{properties.Property}\"",
				$"dbType: DbType.{dbType}",
				$"value: entity.{properties.Property}"
			};

			//TODO: Need to work through every type to see what the combinations are
			if (t == SqlDbType.DateTime2)
			{
				lst.Add($"Scale: {properties.Scale}");
			}

			if (t == SqlDbType.Decimal)
			{
				lst.Add($"Precision: {properties.Precision}, Scale: {properties.Scale}");
			}

			if (t == SqlDbType.VarChar ||
				t == SqlDbType.NVarChar ||
				t == SqlDbType.Char ||
				t == SqlDbType.NChar)
			{
				lst.Add($"Size: {properties.Size}");
			}

			var content = $"				p.Add({string.Join(", ", lst)});";

			return content;
		}
	}
}