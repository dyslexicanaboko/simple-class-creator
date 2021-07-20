using SimpleClassCreator.Models;
using SimpleClassCreator.Services.CodeFactory;
using System.Collections.Generic;
using System.Text;

namespace SimpleClassCreator.Services.Generators
{
    public class ModelGenerator
        : GeneratorBase
    {
        public ModelGenerator(ClassInstructions instructions)
            : base(instructions, "Model.cs")
        {
            
        }

        public override StringBuilder FillTemplate()
        {
            var strTemplate = GetTemplate(TemplateName);

            Template = new StringBuilder(strTemplate);

            Template.Replace("{{Namespace}}", Instructions.Namespace);
            Template.Replace("{{ClassName}}", Instructions.ClassName);
            Template.Replace("{{ClassAttributes}}", FormatClassAttributes(Instructions.ClassAttributes));
            Template.Replace("{{Namespaces}}", FormatNamespaces(Instructions.Namespaces));
            Template.Replace("{{Properties}}", FormatProperties(Instructions.Properties));

            return Template;
        }

        /* Need to move what can be reused to the base class
         * Need to make sure this is backwards compatible with VB or maybe not, I am not sure.
         * Need to account for using clauses that need to be imported as a result of which properties are being generated.
         * Create unit tests for each part of the ModelGenerator.
         */
        private string FormatClassAttributes(IList<string> classAttributes)
        {
            var sb = new StringBuilder();

            foreach (var ca in classAttributes)
            {
                sb.Append("[").Append(ca).Append("]").AppendLine();
            }

            var content = sb.ToString();

            return content;
        }

        private string FormatNamespaces(IList<string> namespaces)
        {
            var sb = new StringBuilder();

            foreach (var ns in namespaces)
            {
                sb.Append("using ").Append(ns).Append(";").AppendLine();
            }

            var content = sb.ToString();

            return content;
        }

        private string FormatProperties(IList<ClassMemberStrings> properties)
        {
            var sb = new StringBuilder();

            foreach (var p in properties)
            {
                sb.Append($"public {p.SystemType} {p.Property} {{ get; set; }}").AppendLine();
            }

            var content = sb.ToString();

            return content;
        }
    }
}
