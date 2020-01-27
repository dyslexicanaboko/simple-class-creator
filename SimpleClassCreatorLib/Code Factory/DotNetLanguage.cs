using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.Text;

namespace SimpleClassCreator
{
    public enum CodeType
    {
        CSharp = 0,
        VBNet = 1
    }

    public abstract class DotNetLanguage
    {
        protected string _listOfTarget = string.Empty;
        protected string _memberPrefix;
        protected string _indexOpen;
        protected string _indexClose;
        protected string _vbFunction;
        protected string _vbEndFunction;
        protected string _vbEndSub;
        protected string _vbNext;
        
        abstract public void InitializeMotifValues();
        
        public virtual string CreateRegion(string regionName)
        {
            return string.Format("{0} {1}" + Environment.NewLine, StartRegion, regionName);
        }

        abstract public void CreateProperty(StringBuilder sb, MemberInfo info);
        abstract public void CreateObjectGenerationMethod(StringBuilder sb, string body);
        abstract protected void CreateForEach(StringBuilder sb, string name, string dataType, string collection, string body);
        abstract public void CreateUpdateMethod(StringBuilder sb, string updateStatement, MemberInfo primaryKey);
        abstract public void CreateInsertMethod(StringBuilder sb, string columns, string insertStatement);
        abstract public void CreateAddStringMethods(StringBuilder sb);
        abstract protected string GetParameter(string name, string dataType);

        protected string CapitalizeFirstLetter(string target)
        {
            return target.Substring(0, 1).ToUpper() + target.Substring(1, target.Length - 1);
        }

        protected virtual string GetVariable(string name, string dataType, string setValue = "")
        {
            return string.Format("{0}{1}", GetParameter(name, dataType), (setValue == string.Empty ? "" : " = " + setValue));
        }

        public string DataRowGet(string dataRow, string columnName)
        { 
            return dataRow + WrapInIndex(WrapInQuotes(columnName));
        }

        public static string WrapInQuotes(string target)
        {
            return "\"" + target + "\"";
        }

        public string WrapInIndex(string target)
        {
            return _indexOpen + target + _indexClose;
        }

#region Properties
        public bool IncludeWCFTags { get; protected set; }
        public string Using { get; protected set; }

        public string OpenNamespace { get; protected set; }
        public string CloseNamespace { get; protected set; }
        public string OpenClass { get; protected set; }
        public string CloseClass { get; protected set; }
        public string StartRegion { get; protected set; }

        private string m_endRegion;
        public string EndRegion
        {
            get { return m_endRegion + Environment.NewLine + Environment.NewLine; }
            protected set { m_endRegion = value; }
        }

        public string ClassName { get; set; }
        public string NamespaceName { get; set; }
        public string Namespace { get; protected set; }
        public string Class { get; protected set; }
        public string EmptyConstructor { get; protected set; }
        public string Public  { get; protected set; }
        public string Private { get; protected set; }
        public string Void { get; protected set; }
        public string Static { get; protected set; }
        public string New { get; protected set; }
        public string Null { get; protected set; }
        public string ListOfTarget { get; protected set; }
        public string LineTerminator { get; protected set; }
        public string ForEach { get; protected set; }
        public string In { get; protected set; }
        public string Return { get; protected set; }
        public string FileExtension  { get; protected set; }
        public string DataContract { get; protected set; }
        public string DataMember { get; protected set; }
#endregion

        public class MemberInfo
        {
            private CodeType _type;
            private CodeDomProvider _provider;

            public MemberInfo(DataColumn dc, CodeType type, string memberPrefix)
            {
                _type = type;

                if(_type == CodeType.CSharp)
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
