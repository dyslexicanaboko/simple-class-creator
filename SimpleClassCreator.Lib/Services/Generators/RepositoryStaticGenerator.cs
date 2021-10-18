using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
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

			template.Replace("{{Namespace}}", Instructions.Namespace);
			template.Replace("{{ClassName}}", Instructions.ClassName);
			template.Replace("{{Namespaces}}", FormatNamespaces(Instructions.Namespaces));

			var t = template.ToString();

			t = RemoveExcessBlankSpace(t);
			//t = RemoveBlankLines(t);

			var pk = Instructions.Properties.SingleOrDefault(x => x.IsPrimaryKey);
			var lstNoPk = Instructions.Properties.Where(x => !x.IsPrimaryKey).ToList();


			//if (pk == null)
			//{
			//    pk = new ClassMemberStrings();
			//}

			//TODO: What to do when there is no primary key?
			t = t.Replace("{{PrimaryKey}}", pk.ColumnName);
			t = t.Replace("{{PrimaryKeyType}}", pk.DatabaseType);
			t = t.Replace("{{PrimaryKeySqlParameter}}", null);

			t = t.Replace("{{Schema}}", Instructions.TableMeta.Schema);
			t = t.Replace("{{Table}}", Instructions.TableMeta.Table);
			t = t.Replace("{{SelectAllList}}", FormatSelectList(Instructions.Properties));
			t = t.Replace("{{InsertColumnList}}", FormatSelectList(lstNoPk));
			t = t.Replace("{{InsertValuesList}}", FormatSelectList(lstNoPk, "@"));
			t = t.Replace("{{UpdateParameters}}", FormatUpdateList(lstNoPk));
			t = t.Replace("{{SqlParameters}}", null);
			t = t.Replace("{{SetProperties}}", null);

			var r = GetResult();
			r.Filename = Instructions.ClassName + "_IEquatable.cs";
			r.Contents = t;

			return r;
		}

		private string FormatSelectList(IList<ClassMemberStrings> properties, string prefix = null)
		{
			var content = GetTextBlock(properties,
				(p) => $"                {prefix}{p.Property}",
				separator: "," + Environment.NewLine);

			return content;
		}

		private string FormatUpdateList(IList<ClassMemberStrings> properties)
		{
			var content = GetTextBlock(properties,
				(p) => $"                {p.Property} = @{p.Property}",
				separator: "," + Environment.NewLine);

			return content;
		}

		private string FormatSqlParameter(ClassMemberStrings properties)
		{
			var content = $@"p = new SqlParameter();
			p.ParameterName = ""@{properties.Property}"";
			p.SqlDbType = SqlDbType.{properties.DatabaseType};
			p.Value = entity.{properties.Property}; ";

			//TODO: Need to handle Scale, Precision and Size

			return content;
		}
	}
}
