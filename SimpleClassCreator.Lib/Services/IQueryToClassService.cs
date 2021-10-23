using System.Collections.Generic;
using SimpleClassCreator.Lib.Models;

namespace SimpleClassCreator.Lib.Services
{
    public interface IQueryToClassService
    {
        IList<GeneratedResult> Generate(QueryToClassParameters parameters);
    }
}