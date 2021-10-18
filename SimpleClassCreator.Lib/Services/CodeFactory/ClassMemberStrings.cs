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

        public ClassMemberStrings(DataColumn dc, CodeType type)
        {
            if (type == CodeType.CSharp)
                _provider = new CSharpCodeProvider();
            else
                _provider = new VBCodeProvider();

            //Best attempt at mapping data column to a SQL type
            //DatabaseType = dc.

            //Determine if this is the primary key for the type
            //IsPrimaryKey = dc.

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
            Field = "_" + firstChar.ToLower() + remainder;

            //Getting the base type
            SystemType = GetTypeAsString(dc.DataType);

            //If this column is nullable then mark it with a question mark
            if (!IsImplicitlyNullable && IsDbNullable)
                SystemType = SystemType + "?";

            //These statements are a matter of preference
            //StringValue = dc.DataType == typeof(string) || dc.DataType == typeof(DateTime) ? "AddString(" + Property + ")" : Property; //it is important to filter strings for SQL Injection, hence the AddString method
            
            ConvertTo = "Convert.To" + (dc.DataType == typeof(byte) ? "Int32" : dc.DataType.Name) + "("; //A byte can fit inside of an Int32
        }

        //For cloning only, bypasses all of the logic and is a straight copy
        private ClassMemberStrings(ClassMemberStrings source, CodeDomProvider provider)
        {
            _provider = provider;

            ColumnName = source.ColumnName;
            InSystemNamespace = source.InSystemNamespace;
            IsDbNullable = source.IsDbNullable;
            IsImplicitlyNullable = source.IsImplicitlyNullable;
            Field = source.Field;
            Property = source.Property;
            SystemType = source.SystemType;
            ConvertTo = source.ConvertTo;
            //StringValue = source.StringValue;
        }

        /// <summary>Qualified SQL Column name</summary>
        public string ColumnName { get; }

        /// <summary>SQL Server database type</summary>
        public string DatabaseType { get; }

        public int? Size { get; set; }
        
        public int? Scale { get; set; }

        public int? Precision { get; set; }

        public bool IsPrimaryKey { get; }

        public bool IsDbNullable { get; }
        
        /// <summary>
        /// Some code types are nullable without having to qualify it with a question mark
        /// in front of the type. In other words don't add a question mark to types that
        /// are already nullable such as strings.
        /// </summary>
        public bool IsImplicitlyNullable { get; }

        public bool InSystemNamespace { get; private set; }

        public string Field { get; }
        
        /// <summary>Property name in code</summary>
        public string Property { get; }
        
        /// <summary>Property's System Type in code</summary>
        public string SystemType { get; }

        public string ConvertTo { get; }
        
        //public string StringValue { get; }

        private string GetTypeAsString(Type target)
        {
            var str = _provider.GetTypeOutput(new CodeTypeReference(target));

            if (!str.StartsWith("System.")) return str;

            //This is a side-effect, but for now I will let it pass
            InSystemNamespace = true;

            str = str.Replace("System.", string.Empty);

            return str;
        }

        public ClassMemberStrings Clone() => new ClassMemberStrings(this, _provider);
    }
}
