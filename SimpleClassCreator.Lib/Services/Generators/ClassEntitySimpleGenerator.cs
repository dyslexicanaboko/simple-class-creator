using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ClassEntitySimpleGenerator
        : GeneratorBase
    {
        public ClassEntitySimpleGenerator(ClassInstructions instructions)
            : base(instructions, "EntitySimple.cs")
        {

        }

        public override GeneratedResult FillTemplate()
        {
            var strTemplate = GetTemplate(TemplateName);

            var template = new StringBuilder(strTemplate);

            template.Replace("{{ClassName}}", Instructions.ClassEntityName);

            var t = template.ToString();

            t = RemoveExcessBlankSpace(t);

            t = t.Replace("{{Properties}}", FormatProperties(Instructions.Properties));

            var r = GetResult();
            r.Contents = t;

            return r;
        }

        public GeneratedResult FillMockDataTemplate(DataTable dataTable)
        {
            var lst = new List<string>(dataTable.Rows.Count);

            var cn = Instructions.ClassEntityName;

            foreach (DataRow r in dataTable.Rows)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"new {cn}")
                    .AppendLine("{");

                foreach (DataColumn c in dataTable.Columns)
                {
                    var p = Instructions.Properties.Single(x =>
                        x.Property.Equals(c.ColumnName, StringComparison.OrdinalIgnoreCase));

                    var value = GetValueString(p, r[c]);

                    sb.Append(p.Property).Append(" = ").Append(Convert.ToString(value)).AppendLine(",");
                }
                
                sb.AppendLine("}");

                lst.Add(sb.ToString());
            }

            var sbFinal = new StringBuilder();

            sbFinal.AppendLine($"var lst = new List<{cn}>")
                .AppendLine("{")
                .Append(string.Join("," + Environment.NewLine, lst))
                .AppendLine("};");

            var result = GetResult();
            result.Contents = sbFinal.ToString();

            return result;
        }

        private string GetValueString(ClassMemberStrings property, object value)
        {
            if (value == DBNull.Value) return "null";

            var strValue = Convert.ToString(value);

            if (property.SystemType == typeof(string))
            {
                strValue = string.IsNullOrEmpty(strValue) ? "string.Empty" : $"\"{strValue}\"";
            }

            if (property.SystemType == typeof(DateTime))
            {
                strValue = $"DateTime.Parse(\"{strValue}\")";
            }

            if (property.SystemType == typeof(Guid))
            {
                strValue = $"Guid.Parse(\"{strValue}\")";
            }

            return strValue;
        }
    }
}
