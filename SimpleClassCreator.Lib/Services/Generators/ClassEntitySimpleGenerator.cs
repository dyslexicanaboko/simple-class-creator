using SimpleClassCreator.Lib.Models;
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
    }
}
