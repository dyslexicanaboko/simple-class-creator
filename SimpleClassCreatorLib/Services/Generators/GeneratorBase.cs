using SimpleClassCreator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleClassCreator.Services.Generators
{
    public abstract class GeneratorBase
    {
        private readonly string _templatesPath;

        protected StringBuilder Template { get; set; }

        protected ClassInstructions Instructions { get; set; }

        public GeneratorBase(ClassInstructions instructions, string templateName)
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

        public abstract StringBuilder FillTemplate();
    }
}
