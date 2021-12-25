using System;

namespace SimpleClassCreator.Lib.Exceptions
{
    public class NonUniqueColumnException
    : Exception
    {
        public NonUniqueColumnException(string nonUniqueColumnName)
            : base(GetMessage(nonUniqueColumnName))
        {

        }

        private static string GetMessage(string nonUniqueColumnName)
        {
            var str = $"The column named \"{nonUniqueColumnName}\" shows up more than once. Please make the column names unique.";

            return str;
        }
    }
}
