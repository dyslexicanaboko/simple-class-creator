using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
	public class ClassCompiler
	{
		public CompilerResult Compile(string classSourceCode)
		{
			var syntaxTree = CSharpSyntaxTree.ParseText($@"
				using System;

				namespace Namespace1
				{{
					{classSourceCode}
				}}");

			//Location of System.Object
			var objectAssemblyPath = typeof(object).Assembly.Location;

			//Get path of System.Object in order to load DLLs of the same version.
			var assemblyPath = Path.GetDirectoryName(objectAssemblyPath);

			MetadataReference[] references = 
			{
				MetadataReference.CreateFromFile(objectAssemblyPath),
				MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
				MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
			};

			//Generate random DLL name as a temporary container
			var assemblyName = Path.GetRandomFileName();

			var compilation = CSharpCompilation.Create(
				assemblyName,
				syntaxTrees: new[] { syntaxTree },
				references: references,
				options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			using (var ms = new MemoryStream())
			{
				// write IL code into memory
				var result = compilation.Emit(ms);

                var cr = new CompilerResult { CompiledSuccessfully = result.Success };

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

				AppDomain.CurrentDomain.DefineDynamicAssembly() //<-- need to figure out how to save this dynamically.
			}
		}
	}
}
