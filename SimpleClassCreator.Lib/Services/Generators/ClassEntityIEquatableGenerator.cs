using System;
using System.Collections.Generic;
using SimpleClassCreator.Lib.Models;
using System.Text;
using SimpleClassCreator.Lib.Services.CodeFactory;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ClassEntityIEquatableGenerator
        : GeneratorBase
    {
        public ClassEntityIEquatableGenerator(ClassInstructions instructions)
            : base(instructions, "EntityIEquatable.cs")
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
            //t = RemoveBlankLines(t);

            t = t.Replace("{{PropertiesEquals}}", FormatForEquals(Instructions.Properties));
            t = t.Replace("{{PropertiesHashCode}}", FormatForHashCode(Instructions.Properties));

            var r = GetResult();
            r.Filename = Instructions.ClassEntityName + "_IEquatable.cs";
            r.Contents = t;

            return r;
        }

        private string FormatForEquals(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties,
                (p) => $"                {p.Property} == p.{p.Property}",
                separator: " && " + Environment.NewLine);

            return content;
        }

        private string FormatForHashCode(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties,
                (p) => $"                {p.Property}.GetHashCode()",
                separator: " + " + Environment.NewLine);

            return content;
        }
    }
}
