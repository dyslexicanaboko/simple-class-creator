using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SimpleClassCreator.Lib.Models;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleClassCreator.Lib.Services.CodeFactory
{
	public class CSharpCompilerService : ICSharpCompilerService
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
			var assemblyName = $"VirtualAssembly_{Path.GetRandomFileName()}.dll";

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

				cr.VirtualAssembly = Assembly.Load(ms.ToArray());
				cr.AssemblyPath = Path.Combine(Path.GetTempPath(), assemblyName);

				return cr;
			}
		}
	}
}
