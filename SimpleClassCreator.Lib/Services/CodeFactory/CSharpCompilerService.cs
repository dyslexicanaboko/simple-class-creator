using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SimpleClassCreator.Lib.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public class CSharpCompilerService
    {
        private const string UserDynamicNamespace = "SimpleClassCreator.Lib.Services.CodeFactory.UserDynamic";

        public CompilerResult GetType(string cSharpCodeClassOnly)
        {
            //Take user inputted C# class and dynamically build it to parse the class properties
            var syntaxTree = CSharpSyntaxTree.ParseText($@"
                using System;

                namespace {UserDynamicNamespace}
                {{
                    {cSharpCodeClassOnly}
                }}");

            var objectAssemblyPath = typeof(object).Assembly.Location;

            //Get the current path of where the "Object" class was loaded from. This is how other DLLs of the same version can be loaded.
            var assemblyPath = Path.GetDirectoryName(objectAssemblyPath);

            //Adding bare minimum references that are needed to compile and run the code
            MetadataReference[] references = {
                MetadataReference.CreateFromFile(objectAssemblyPath),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };

            //Randomly naming the assembly because it's always going to be throw away
            var assemblyName = Path.GetRandomFileName();

            //Analyze and generate IL code from syntax tree
            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                //Write IL code into memory
                var result = compilation.Emit(ms);

                var cr = new CompilerResult {CompiledSuccessfully = result.Success};

                if (!result.Success)
                {
                    //Handle exceptions
                    cr.Errors = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(x => $"{x.Id} {x.GetMessage()}")
                        .ToList();

                    return cr;
                }

                // load this 'virtual' DLL so that we can use
                ms.Seek(0, SeekOrigin.Begin);

                var assembly = Assembly.Load(ms.ToArray());

                //There should only be one class in this assembly, might have to plan for more than one class
                var className = assembly.GetTypes().Single();

                // create instance of the desired class and call the desired function
                cr.Type = assembly.GetType($"{UserDynamicNamespace}.{className}");

                cr.Instance = Activator.CreateInstance(cr.Type);

                return cr;
            }
        }
    }
}
