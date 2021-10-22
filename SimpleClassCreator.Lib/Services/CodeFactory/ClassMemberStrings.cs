using Microsoft.CSharp;
using Microsoft.VisualBasic;
using SimpleClassCreator.Lib.Models;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;

namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public class ClassMemberStrings
    {
        private readonly CodeDomProvider _provider;

        public ClassMemberStrings(SchemaColumn sc, CodeType type)
        {
            if (type == CodeType.CSharp)
                _provider = new CSharpCodeProvider();
            else
                _provider = new VBCodeProvider();

            DatabaseTypeName = sc.SqlType.ToLower(); //Case is inconsistent, so making it lower on purpose

            DatabaseType = TypesService.SqlTypes[DatabaseTypeName];

            Size = sc.Size;

            Precision = sc.Precision;

            Scale = sc.Scale;

            IsPrimaryKey = sc.IsPrimaryKey;
            
            IsDbNullable = sc.IsDbNullable;

            IsImplicitlyNullable = 
                sc.SystemType == typeof(string) || 
                sc.SystemType.BaseType == typeof(Array);

            //Remove unnecessary extra padding if it shows up
            ColumnName = sc.ColumnName.Trim();

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
            SystemTypeName = GetTypeAsString(sc.SystemType);

            SystemTypeAlias = TypesService.MapSystemToAliases[sc.SystemType];

            //TODO: Should I keep doing this? Not sure yet.
            //If this column is nullable then mark it with a question mark
            if (!IsImplicitlyNullable && IsDbNullable)
                SystemTypeName = SystemTypeName + "?";

            ConversionMethodSignature = GetConversionMethodSignature(sc.SystemType, SystemTypeName);
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
            SystemTypeName = source.SystemTypeName;
            ConversionMethodSignature = source.ConversionMethodSignature;
            //StringValue = source.StringValue;
        }

        /// <summary>Qualified SQL Column name</summary>
        public string ColumnName { get; }

        /// <summary>SQL Server database type name in lower case</summary>
        public string DatabaseTypeName { get; }

        /// <summary>SQL Server database type enumeration</summary>
        public SqlDbType DatabaseType { get; }

        /// <summary>Column size for varchar, nvarchar, char, nchar etc...</summary>
        public int Size { get; }

        /// <summary>Column precision for numeric types such as decimal(p,s)</summary>
        public int Precision { get; }

        /// <summary>Column scale for numeric types such as decimal(p,s) and for datetime2(s)</summary>
        public int Scale { get; }

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
        
        /// <summary>
        /// Property's System Type in code from typeof(T). This is the full class name of the type, not the alias.
        /// Does not include the "System." namespace as a prefix.
        /// </summary>
        public string SystemTypeName { get; }

        /// <summary>Property's System Type in code from typeof(T). This is the alias of the type.</summary>
        public string SystemTypeAlias { get; }

        public string ConversionMethodSignature { get; }

        private string GetTypeAsString(Type target)
        {
            var str = _provider.GetTypeOutput(new CodeTypeReference(target));

            if (!str.StartsWith("System.")) return str;

            //This is a side-effect, but for now I will let it pass
            InSystemNamespace = true;

            str = str.Replace("System.", string.Empty);

            return str;
        }

        private string GetConversionMethodSignature(Type type, string systemTypeName)
        {
            //Guid does not have a method in the Convert class
            if (type == typeof(Guid))
            {
                //DataReader column has to be converted to string first
                return "Guid.Parse(Convert.ToString({0}))";
            }

            var typeString = systemTypeName;

            //TODO: I don't know why I had this code, there is a valid Convert.ToByte() method
            //A byte can fit inside of an Int32
            //if (type == typeof(byte))
            //{
            //    typeString = "Int32";
            //}

            var c = "Convert.To" + typeString + "({0})";

            return c;
        }

        public ClassMemberStrings Clone() => new ClassMemberStrings(this, _provider);
    }
}
