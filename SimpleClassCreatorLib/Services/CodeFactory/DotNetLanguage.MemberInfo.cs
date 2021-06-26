using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;

namespace SimpleClassCreator.Services.CodeFactory
{
    public abstract partial class DotNetLanguage
    {
        public class MemberInfo
        {
            private CodeType _type;
            private CodeDomProvider _provider;

            public MemberInfo(DataColumn dc, CodeType type, string memberPrefix)
            {
                _type = type;

                if (_type == CodeType.CSharp)
                    _provider = new CSharpCodeProvider();
                else
                    _provider = new VBCodeProvider();

                IsNullable = dc.AllowDBNull && dc.DataType != typeof(string);

                ColumnName = dc.ColumnName.Contains(" ") ? "[" + dc.ColumnName + "]" : dc.ColumnName;

                //Removing any whitespace
                Property = dc.ColumnName.Trim().Replace(" ", string.Empty);

                //Camel Casing the property name
                Property = Property.Substring(0, 1).ToUpper() + Property.Substring(1, Property.Length - 1);

                Member = memberPrefix + "_" + Property.Substring(0, 1).ToLower() + Property.Substring(1, Property.Length - 1);

                //Getting the base type
                SystemType = GetTypeAsString(dc.DataType);

                //If this column is nullable then mark it with a question mark
                if (IsNullable)
                    SystemType = SystemType + "?";

                //These statements are a matter of preference
                StringValue = dc.DataType.Equals(typeof(string)) || dc.DataType.Equals(typeof(DateTime)) ? "AddString(" + Property + ")" : Property; //it is important to filter strings for SQL Injection, hence the AddString method
                ConvertTo = "Convert.To" + (dc.DataType.Equals(typeof(Byte)) ? "Int32" : dc.DataType.Name) + "("; //A byte can fit inside of an Int32
            }

            public string ColumnName { get; set; }
            public bool IsNullable { get; set; }
            public string Member { get; set; }
            public string Property { get; set; }
            public string SystemType { get; set; }
            public string ConvertTo { get; set; }
            public string StringValue { get; set; }

            public string GetTypeAsString(Type target)
            {
                return _provider.GetTypeOutput(new CodeTypeReference(target));
            }
        }
    }
}
