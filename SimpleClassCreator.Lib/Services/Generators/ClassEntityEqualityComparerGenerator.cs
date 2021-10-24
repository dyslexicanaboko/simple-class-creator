using System;
using System.Collections.Generic;
using SimpleClassCreator.Lib.Models;
using System.Text;
using SimpleClassCreator.Lib.Services.CodeFactory;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ClassEntityEqualityComparerGenerator
        : GeneratorBase
    {
        public ClassEntityEqualityComparerGenerator(ClassInstructions instructions)
            : base(instructions, "EntityEqualityComparer.cs")
        {

        }

        public override GeneratedResult FillTemplate()
        {
            var strTemplate = GetTemplate(TemplateName);

            var template = new StringBuilder(strTemplate);

            template.Replace("{{Namespace}}", Instructions.Namespace);
            template.Replace("{{ClassName}}", Instructions.ClassEntityName);
            template.Replace("{{Namespaces}}", FormatNamespaces(Instructions.Namespaces));

            var t = template.ToString();

            t = RemoveExcessBlankSpace(t);

            t = t.Replace("{{PropertiesEquals}}", FormatForEquals(Instructions.Properties));
            t = t.Replace("{{PropertiesHashCode}}", FormatForHashCode(Instructions.Properties));

            var r = GetResult();
            r.Filename = Instructions.ClassEntityName + "EqualityComparer.cs";
            r.Contents = t;

            return r;
        }

        private string FormatForEquals(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties,
                (p) => $"                x.{p.Property} == y.{p.Property}",
                separator: " && " + Environment.NewLine);

            return content;
        }

        private string FormatForHashCode(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties,
                (p) => $"                obj.{p.Property}",
                separator: "," + Environment.NewLine);

            return content;
        }
    }
}
