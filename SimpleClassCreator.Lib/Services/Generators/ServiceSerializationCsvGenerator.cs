using SimpleClassCreator.Lib.Models;
using System.Text;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public class ServiceSerializationCsvGenerator
        : GeneratorBase
    {
        public ServiceSerializationCsvGenerator(ClassInstructions instructions)
            : base(instructions, "ServiceSerializationCsv.cs")
        {

        }

        public override GeneratedResult FillTemplate()
        {
            var strTemplate = GetTemplate(TemplateName);

            var template = new StringBuilder(strTemplate);

            template.Replace("{{Namespace}}", Instructions.Namespace);
            template.Replace("{{ClassName}}", Instructions.ClassEntityName);

            var t = template.ToString();

            t = RemoveExcessBlankSpace(t);

            var r = GetResult();
            r.Filename = "SerializationService_Csv.cs";
            r.Contents = t;

            return r;
        }
    }
}
