using SimpleClassCreator.Lib.Events;
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
    public delegate void RowProcessedHandler(object sender, RowProcessedEventArgs e);

    public ClassEntitySimpleGenerator(ClassInstructions instructions)
      : base(instructions, "EntitySimple.cs")
    {
    }

    public event RowProcessedHandler RowProcessed;

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

      var count = 0;

      foreach (DataRow r in dataTable.Rows)
      {
        var sb = new StringBuilder();

        sb.AppendLine($"new {cn}")
          .AppendLine("{");

        foreach (DataColumn c in dataTable.Columns)
        {
          var p = Instructions.Properties.Single(
            x =>
              x.Property.Equals(c.ColumnName, StringComparison.OrdinalIgnoreCase));

          var value = GetValueString(p, r[c]);

          sb.Append(p.Property).Append(" = ").Append(Convert.ToString(value)).AppendLine(",");
        }

        sb.AppendLine("}");

        lst.Add(sb.ToString());

        RaiseRowProcessedEvent(new RowProcessedEventArgs { Count = ++count, Total = dataTable.Rows.Count });
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

    private static string GetValueString(ClassMemberStrings property, object value)
    {
      if (value == DBNull.Value) return "null";

      var strValue = Convert.ToString(value);

      if (property.SystemType == typeof(string))
        return string.IsNullOrEmpty(strValue) ? "string.Empty" : $"\"{strValue}\"";

      if (property.SystemType == typeof(bool)) return strValue.ToLower();

      if (property.SystemType == typeof(decimal)) return $"{strValue}M";

      if (property.SystemType == typeof(double)) return $"{strValue}D";

      if (property.SystemType == typeof(DateTime)) return $"DateTime.Parse(\"{strValue}\")";

      if (property.SystemType == typeof(Guid)) return $"Guid.Parse(\"{strValue}\")";

      return strValue;
    }

    private void RaiseRowProcessedEvent(RowProcessedEventArgs e) => RowProcessed?.Invoke(this, e);
  }
}
