using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SimpleClassCreator.Lib.Services.Generators
{
    public abstract class GeneratorBase
    {
        private readonly string _templatesPath;
        
        protected ClassInstructions Instructions { get; set; }

        protected readonly Regex _reBlankSpace = new Regex(@"^\s+$^[\r\n]", RegexOptions.Multiline);
        protected readonly Regex _reBlankLines = new Regex(@"^\s+$[\r\n]*", RegexOptions.Multiline);

        protected GeneratorBase(ClassInstructions instructions, string templateName)
        {
            _templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            
            TemplateName = templateName;

            Instructions = instructions;
        }

        protected GeneratedResult GetResult()
        {
            var r = new GeneratedResult
            {
                Filename = Instructions.ClassName + ".cs"
            };

            return r;
        }

        public string TemplateName { get; protected set; }

        protected virtual string GetTemplate(string templateName)
        {
            var file = Path.Combine(_templatesPath, templateName);

            var str = File.ReadAllText(file);

            return str;
        }

        protected virtual string GetTextBlock<T>(IList<T> items, Func<T, string> formatting, string separator = null)
        {
            if (items.Count == 0) return string.Empty;

            var lst = new List<string>(items.Count);

            foreach (var item in items)
            {
                var formatted = formatting(item);

                lst.Add(formatted);
            }

            if (separator == null)
            {
                separator = Environment.NewLine;
            }

            var content = string.Join(separator, lst);

            return content;
        }

        protected virtual string RemoveBlankLines(string content)
        {
            var replacement = _reBlankLines.Replace(content, string.Empty);

            return replacement;
        }

        protected virtual string RemoveExcessBlankSpace(string content)
        {
            var replacement = _reBlankSpace.Replace(content, string.Empty);

            return replacement;
        }

        public abstract GeneratedResult FillTemplate();

        /* Need to move what can be reused to the base class
         * Need to make sure this is backwards compatible with VB or maybe not, I am not sure.
         * Need to account for using clauses that need to be imported as a result of which properties are being generated.
         * Create unit tests for each part of the ModelGenerator.
         */
        protected virtual string FormatClassAttributes(IList<string> classAttributes)
        {
            var content = GetTextBlock(classAttributes, (ca) => $"[{ca}]");

            return content;
        }

        protected virtual string FormatNamespaces(IList<string> namespaces)
        {
            var content = GetTextBlock(namespaces, (ns) => $"using {ns};");

            return content;
        }

        protected virtual string FormatInterface(string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName)) return string.Empty;

            //This is showing on one line for now. In the future I might format it properly on the next line.
            var content = " : " + interfaceName;

            return content;
        }

        protected virtual string FormatProperties(IList<ClassMemberStrings> properties)
        {
            var content = GetTextBlock(properties,
                (p) => $"        public {p.SystemTypeName} {p.Property} {{ get; set; }}",
                separator: Environment.NewLine + Environment.NewLine);

            return content;
        }

        protected string GetNotImplementedException(string exceptionMessage = null)
        {
            if (exceptionMessage == null)
                exceptionMessage = string.Empty;
            else
                exceptionMessage = $"\"{exceptionMessage}\""; 

            return $"throw new NotImplementedException({exceptionMessage});";
        }
    }
}
