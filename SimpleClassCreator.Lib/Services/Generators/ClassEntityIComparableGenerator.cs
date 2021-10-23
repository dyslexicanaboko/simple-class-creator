using SimpleClassCreator.Lib.Models;
using System.Linq;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ClassEntityIComparableGenerator
        : GeneratorBase
    {
        public ClassEntityIComparableGenerator(ClassInstructions instructions)
            : base(instructions, "EntityIComparable.cs")
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

            t = t.Replace("{{Property1}}", Instructions.Properties.First().Property);

            var r = GetResult();
            r.Filename = Instructions.ClassName + "_IComparable.cs";
            r.Contents = t;

            return r;
        }
    }
}
