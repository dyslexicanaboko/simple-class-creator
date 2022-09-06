using SimpleClassCreator.Lib.Models;
using System.Collections.Generic;

namespace SimpleClassCreator.Lib.Services
{
    public interface IQueryToClassService
    {
        IList<GeneratedResult> Generate(QueryToClassParameters parameters);

        IList<GeneratedResult> Generate(DtoInstructions parameters);
    }
}