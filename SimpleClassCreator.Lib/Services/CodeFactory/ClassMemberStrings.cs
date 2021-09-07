using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;

namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public class ClassMemberStrings
    {
        private readonly CodeDomProvider _provider;

        public ClassMemberStrings(DataColumn dc, CodeType type, string fieldPrefix)
        {
            var _type = type;

            if (_type == CodeType.CSharp)
                _provider = new CSharpCodeProvider();
            else
                _provider = new VBCodeProvider();

            IsDbNullable = dc.AllowDBNull;

            IsImplicitlyNullable = 
                dc.DataType == typeof(string) || 
                dc.DataType.BaseType == typeof(Array);

            //Remove unnecessary extra padding if it shows up
            ColumnName = dc.ColumnName.Trim();

            //Qualifying the column name for SQL
            if(ColumnName.Contains(" ")) ColumnName = "[" + ColumnName + "]";

            //Removing any whitespace
            Property = ColumnName.Replace(" ", string.Empty);

            var firstChar = Property.Substring(0, 1);
            var remainder = Property.Substring(1, Property.Length - 1);

            //Pascal Case the property name
            Property = firstChar.ToUpper() + remainder;

            //Camel case the field name
            Field = fieldPrefix + "_" + firstChar.ToLower() + remainder;

            //Getting the base type
            SystemType = GetTypeAsString(dc.DataType);

            //If this column is nullable then mark it with a question mark
            if (!IsImplicitlyNullable && IsDbNullable)
                SystemType = SystemType + "?";

            //These statements are a matter of preference
            StringValue = dc.DataType == typeof(string) || dc.DataType == typeof(DateTime) ? "AddString(" + Property + ")" : Property; //it is important to filter strings for SQL Injection, hence the AddString method
            
            ConvertTo = "Convert.To" + (dc.DataType == typeof(byte) ? "Int32" : dc.DataType.Name) + "("; //A byte can fit inside of an Int32
        }

        public string ColumnName { get; }
        
        public bool InSystemNamespace { get; private set; }
        
        public bool IsDbNullable { get; }
        
        public bool IsImplicitlyNullable { get; }
        
        public string Field { get; }
        
        public string Property { get; }
        
        public string SystemType { get; }
        
        public string ConvertTo { get; }
        
        public string StringValue { get; }

        private string GetTypeAsString(Type target)
        {
            var str = _provider.GetTypeOutput(new CodeTypeReference(target));

            if (!str.StartsWith("System.")) return str;

            //This is a side-effect, but for now I will let it pass
            InSystemNamespace = true;

            str = str.Replace("System.", string.Empty);

            return str;
        }
    }
}
