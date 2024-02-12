using System;
using System.Text;

/* This is from a very old version one of the simple class creator when I
 * was trying to support C# and VB.NET simultaneously.
 * I will eventually pull this out of here and put it in a graveyard folder. */
namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public abstract partial class DotNetLanguage
    {
        protected string _listOfTarget = string.Empty;
        protected string _memberPrefix;
        protected string _indexOpen;
        protected string _indexClose;
        protected string _vbFunction;
        protected string _vbEndFunction;
        protected string _vbEndSub;
        protected string _vbNext;
        
        public abstract void InitializeMotifValues();
        
        public virtual string CreateRegion(string regionName)
        {
            return string.Format("{0} {1}" + Environment.NewLine, StartRegion, regionName);
        }

        public abstract void CreateProperty(StringBuilder sb, ClassMemberStrings info);
        
        public abstract void CreateObjectGenerationMethod(StringBuilder sb, string body);
        
        protected abstract void CreateForEach(StringBuilder sb, string name, string dataType, string collection, string body);
        
        public abstract void CreateUpdateMethod(StringBuilder sb, string updateStatement, ClassMemberStrings primaryKey);
        
        public abstract void CreateInsertMethod(StringBuilder sb, string columns, string insertStatement);
        
        protected abstract string GetParameter(string name, string dataType);

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

        public bool IncludeSerializableAttribute { get; protected set; }
        
        public string Using { get; protected set; }

        public string OpenNamespace { get; protected set; }
        
        public string CloseNamespace { get; protected set; }
        
        public string OpenClass { get; protected set; }
        
        public string CloseClass { get; protected set; }
        
        public string StartRegion { get; protected set; }

        private string _endRegion;
        public string EndRegion
        {
            get { return _endRegion + Environment.NewLine + Environment.NewLine; }
            protected set { _endRegion = value; }
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
    }
}
