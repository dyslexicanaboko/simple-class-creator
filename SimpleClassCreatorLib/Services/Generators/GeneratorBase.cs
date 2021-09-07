using SimpleClassCreator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SimpleClassCreator.Services.Generators
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

        public abstract string FillTemplate();
    }
}
