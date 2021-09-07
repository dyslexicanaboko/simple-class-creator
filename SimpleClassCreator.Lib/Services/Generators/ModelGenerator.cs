using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ModelGenerator
        : GeneratorBase
    {
        public ModelGenerator(ClassInstructions instructions)
            : base(instructions, "Model.cs")
        {

        }

        public override string FillTemplate()
        {
            var strTemplate = GetTemplate(TemplateName);

            var template = new StringBuilder(strTemplate);

            template.Replace("{{Namespace}}", Instructions.Namespace);
            template.Replace("{{ClassName}}", Instructions.ClassName);
            template.Replace("{{ClassAttributes}}", FormatClassAttributes(Instructions.ClassAttributes));
            template.Replace("{{Namespaces}}", FormatNamespaces(Instructions.Namespaces));

            var t = template.ToString();

            t = RemoveExcessBlankSpace(t);
            //t = RemoveBlankLines(t);

            t = t.Replace("{{Properties}}", FormatProperties(Instructions.Properties));

            return t;
        }

        /* Need to move what can be reused to the base class
         * Need to make sure this is backwards compatible with VB or maybe not, I am not sure.
         * Need to account for using clauses that need to be imported as a result of which properties are being generated.
         * Create unit tests for each part of the ModelGenerator.
         */
        private string FormatClassAttributes(IList<string> classAttributes)
        {
            var content = GetTextBlock(classAttributes, (ca) => $"[{ca}]" );

            return content;
        }

        private string FormatNamespaces(IList<string> namespaces)
        {
            var content = GetTextBlock(namespaces, (ns) => $"using {ns};");

            return content;
        }

        private string FormatProperties(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties, 
                (p) => $"        public {p.SystemType} {p.Property} {{ get; set; }}", 
                separator: Environment.NewLine + Environment.NewLine);

            return content;
        }
    }
}
