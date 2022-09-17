using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public interface ICSharpCompilerService
    {
        CompilerResult Compile(string classSourceCode);
    }
}