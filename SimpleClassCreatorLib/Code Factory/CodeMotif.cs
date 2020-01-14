using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SimpleClassCreator
{
    

    /*
    public class CodeMotif
    {
        private CodeType _codeType;
        private string _targetClassName;
        private string _memberPrefix;
        private string _indexOpen;
        private string _indexClose;
        private string _vbFunction;
        private string _vbEndFunction;
        private string _vbEndSub;
        private string _vbNext;

        public CodeMotif(string className, CodeType type, string memberPrefix = "")
        {
            _codeType = type;
            _targetClassName = className;
            _memberPrefix = memberPrefix;

            SetMotif();
        }

        private void SetMotif()
        {
            if (_codeType == CodeType.CSharp)
            {
                _indexOpen = "[";
                _indexClose = "]";
                _vbFunction = string.Empty;
                _vbEndFunction = "}" + Environment.NewLine + Environment.NewLine;
                _vbEndSub = "}" + Environment.NewLine + Environment.NewLine;
                _vbNext = "}" + Environment.NewLine;
                m_using = "using";
                m_class = "class";
                m_public = "public";
                m_openClass = m_public + " " + m_class + " " + _targetClassName + Environment.NewLine + "{" + Environment.NewLine;
                m_closeClass = "}" + Environment.NewLine;
                m_private = "private";
                m_void = "void";
                m_static = "static";
                m_new = "new";
                m_null = "null";
                m_emptyConstructor = "public " + _targetClassName + "()" + Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine + Environment.NewLine;
                m_listOfTarget = "List<" + _targetClassName + ">";
                m_lineTerminator = ";" + Environment.NewLine;
                _startRegion = "#region";
                m_endRegion = "#endregion";
                m_forEach = "foreach";
                m_in = "in";
                m_return = "return";
                m_fileExtension = "cs";
            }
            else
            {
                _indexOpen = "(";
                _indexClose = ")";
                _vbFunction = "Function";
                _vbEndFunction = "End Function" + Environment.NewLine + Environment.NewLine;
                _vbEndSub = "End Sub" + Environment.NewLine + Environment.NewLine;
                _vbNext = "Next" + Environment.NewLine;
                m_using = "Imports";
                m_class = "Class";
                m_public = "Public";
                m_openClass = m_public + " " + m_class + " " + _targetClassName + Environment.NewLine;
                m_closeClass = "End Class" + Environment.NewLine;
                m_private = "Private";
                m_void = "Sub";
                m_static = "Shared";
                m_new = "New";
                m_null = "Nothing"; 
                m_emptyConstructor = "Public Sub New()" + Environment.NewLine + Environment.NewLine + "End Sub" + Environment.NewLine + Environment.NewLine;
                m_listOfTarget = "List(Of " + _targetClassName + ")";
                m_lineTerminator = Environment.NewLine;
                _startRegion = "#Region";
                m_endRegion = "#End Region";
                m_forEach = "For Each";
                m_in = "In";
                m_return = "Return";
                m_fileExtension = "vb";
            }
        }

        public string CreateRegion(string regionName)
        {
            string strRegion = _startRegion + " ";

            if (_codeType == CodeType.CSharp)
                strRegion += regionName;
            else
                strRegion += WrapInQuotes(regionName);

            return strRegion + Environment.NewLine;
        }

        /// <summary>
        /// Create a Public Property
        /// </summary>
        /// <param name="sb">The StringBuilder to append to</param>
        /// <param name="info">All necessary information about a Member in order to create the Property</param>
        public void CreateProperty(StringBuilder sb, MemberInfo info)
        {
            if (_codeType == CodeType.CSharp)
            {
                //Private Member
                sb.Append("private ").Append(info.SystemType).Append(" ").Append(info.Member).Append(m_lineTerminator);

                //Public Property
                sb.Append("public ").Append(info.SystemType).Append(" ").Append(info.Property).Append(Environment.NewLine);
                sb.Append("{").Append(Environment.NewLine);
                //Getter
                sb.Append("get { return ").Append(info.Member).Append("; }").Append(Environment.NewLine);

                //Setter
                sb.Append("set { ").Append(info.Member).Append(" = value; }").Append(Environment.NewLine);

                //Close Property
                sb.Append("}");
            }
            else
            {
                //Private Member
                sb.Append("Private ").Append(info.Member).Append(" As ").Append(info.SystemType).Append(m_lineTerminator);

                //Public Property
                sb.Append("Public Property ").Append(info.Property).Append(" As ").Append(info.SystemType).Append(Environment.NewLine);

                //Getter
                sb.Append("Get").Append(Environment.NewLine);
                sb.Append("Return ").Append(info.Member).Append(Environment.NewLine);
                sb.Append("End Get").Append(Environment.NewLine);

                //Setter
                sb.Append("Set(value As ").Append(info.SystemType).Append(")").Append(Environment.NewLine);
                sb.Append(info.Member).Append(" = value").Append(Environment.NewLine);
                sb.Append("End Get").Append(Environment.NewLine);

                //Close Property
                sb.Append("End Property");
            }

            sb.Append(Environment.NewLine).Append(Environment.NewLine);
        }

        public void CreateObjectGenerationMethod(StringBuilder sb, string body)
        {
            string csRT, vbRT;

            if (_codeType == CodeType.CSharp)
            {
                csRT = m_listOfTarget;
                vbRT = Environment.NewLine + "{";
            }
            else
            {
                csRT = string.Empty;
                vbRT = " As " + m_listOfTarget;
            }

            sb.Append(m_public).Append(" ").Append(m_static).Append(" ").Append(csRT).Append(_vbFunction).Append(" ObjectGeneration(").Append(GetParameter("dt", "DataTable")).Append(")").Append(vbRT).Append(Environment.NewLine);
            sb.Append(GetVariable("obj", _targetClassName, m_null)).Append(Environment.NewLine);
            sb.Append(GetVariable("lst", m_listOfTarget, m_new + " " + m_listOfTarget + "()")).Append(Environment.NewLine).Append(Environment.NewLine);
            
            CreateForEach(sb, "dr", "DataRow", "dt.Rows", 
                (
                    "obj = " + m_new + " " + _targetClassName + "()" + m_lineTerminator +
                    body +
                    Environment.NewLine + "lst.Add(obj)" + (_codeType == CodeType.CSharp ? ";" : string.Empty)));
            
            sb.Append(Environment.NewLine);
            sb.Append(m_return).Append(" lst").Append(m_lineTerminator);

            sb.Append(_vbEndFunction);
        }

        private void CreateForEach(StringBuilder sb, string name, string dataType, string collection, string body)
        {
            string csO = string.Empty, 
                   csC = string.Empty;

            if(_codeType == CodeType.CSharp)
            {
                csO = "(";
                csC = ")" + Environment.NewLine + "{";
            }

            sb.Append(m_forEach).Append(csO).Append(" ").Append(GetParameter(name, dataType)).Append(" ").Append(m_in).Append(" ").Append(collection).Append(csC).Append(Environment.NewLine);
            sb.Append(body).Append(Environment.NewLine);
            sb.Append(_vbNext);
        }

        public void CreateUpdateMethod(StringBuilder sb, string updateStatement, MemberInfo primaryKey)
        {
            string csC = Environment.NewLine,
                   pkValue = string.Empty;

            if (_codeType == CodeType.CSharp)
                csC += "{" + Environment.NewLine;

            sb.Append(m_public).Append(" ").Append(m_void).Append(" Update").Append(_targetClassName).Append("(").Append(GetParameter("obj", _targetClassName)).Append(")").Append(csC);
            sb.Append(GetVariable("sb", "StringBuilder", (m_new + " StringBuilder()"))).Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append("sb.Append(\"UPDATE ").Append(_targetClassName).Append(" SET \")").Append(m_lineTerminator);
            sb.Append(updateStatement).Append(m_lineTerminator);
            sb.Append("sb.Append(\"WHERE ").Append(primaryKey.ColumnName).Append(" = \").Append(obj.").Append(primaryKey.Property).Append(").Append(\";\")").Append(m_lineTerminator);
            sb.Append(_vbEndSub);
        }

        public void CreateInsertMethod(StringBuilder sb, string columns, string insertStatement)
        {
            string csC = Environment.NewLine;

            if (_codeType == CodeType.CSharp)
                csC += "{" + Environment.NewLine;

            sb.Append(m_public).Append(" ").Append(m_void).Append(" Insert").Append(_targetClassName).Append("(").Append(GetParameter("obj", _targetClassName)).Append(")").Append(csC);
            sb.Append(GetVariable("sb", "StringBuilder", (m_new + " StringBuilder()"))).Append(Environment.NewLine).Append(Environment.NewLine);
            sb.Append("sb.Append(\"INSERT INTO ").Append(_targetClassName).Append("(").Append(columns).Append(") VALUES ( \")").Append(m_lineTerminator);
            sb.Append(insertStatement).Append(m_lineTerminator);
            sb.Append("sb.Append(\");\")").Append(m_lineTerminator);
            sb.Append(_vbEndSub);
        }

        public void CreateAddStringMethods(StringBuilder sb)
        {
            string csC,
                   csRT,
                   vbRT;

            if (_codeType == CodeType.CSharp)
            {
                csC = Environment.NewLine + "{";
                csRT = "string";
                vbRT = string.Empty;
            }
            else
            {
                csC = string.Empty;
                csRT = string.Empty;
                vbRT = " As String";
            }

            sb.Append(m_public).Append(" ").Append(_vbFunction).Append(csRT).Append(" ").Append(" AddString(").Append(GetParameter("target", "String")).Append(")").Append(vbRT).Append(csC).Append(Environment.NewLine);
            sb.Append(m_return).Append(" \"'\" + target + \"'\"").Append(m_lineTerminator);
            sb.Append(_vbEndFunction);

            sb.Append(m_public).Append(" ").Append(_vbFunction).Append(csRT).Append(" ").Append(" AddString(").Append(GetParameter("target", "DateTime")).Append(")").Append(vbRT).Append(csC).Append(Environment.NewLine);
            sb.Append(m_return).Append(" \"'\" + AddString(target.ToString(\"u\")) + \"'\"").Append(m_lineTerminator);
            sb.Append(_vbEndFunction);
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

        private string GetParameter(string name, string dataType)
        {
            string strParameter;

            if (_codeType == CodeType.CSharp)
                strParameter = dataType + " " + name;
            else
                strParameter = name + " As " + dataType;

            return strParameter;    
        }

        private string GetVariable(string name, string dataType, string setValue = "")
        {
            string strVariable = GetParameter(name, dataType);

            if (_codeType == CodeType.VBNet)
                strVariable = "Dim " + strVariable;
           
            strVariable += (setValue == string.Empty ? "" : " = " + setValue) + (_codeType == CodeType.CSharp ? ";" : "");

            return strVariable;
        }

#region Properties
        private string m_using;
        public string Using
        {
            get { return m_using; }
        }

        private string m_openClass;
        public string OpenClass
        {
            get { return m_openClass; }
        }

        private string m_closeClass;
        public string CloseClass
        {
            get { return m_closeClass; }
        }

        //No public property provided for this member because there is a method 
        //called "CreateRegion()" that is utilized for generating this piece of code
        private string _startRegion;

        private string m_endRegion;
        public string EndRegion
        {
            get { return m_endRegion + Environment.NewLine + Environment.NewLine; }
        }

        private string m_class;
        public string Class
        {
            get { return m_class; }
        }

        private string m_emptyConstructor;
        public string EmptyConstructor
        {
            get { return m_emptyConstructor; }
        }

        private string m_public;
        public string Public 
        {
            get { return m_public; }
        }

        private string m_private;
        public string Private
        {
            get { return m_private; }
        }

        private string m_void;
        public string Void
        {
            get { return m_void; }
        }

        private string m_static;
        public string Static
        {
            get { return m_static; }
        }

        private string m_new;
        public string New
        {
            get { return m_new; }
        }

        private string m_null;
        public string Null
        {
            get { return m_null; }
        }

        private string m_listOfTarget;
        public string ListOfTarget
        {
            get { return m_listOfTarget; }
        }

        private string m_lineTerminator;
        public string LineTerminator
        {
            get { return m_lineTerminator; }
        }

        private string m_forEach;
        public string ForEach
        {
            get { return m_forEach; }
        }

        private string m_in;
        public string In
        {
            get { return m_in; }
        }

        private string m_return;
        public string Return
        {
            get { return m_return; }
        }

        private string m_fileExtension;
        public string FileExtension 
        { 
            get { return m_fileExtension; } 
        }
#endregion

        public class MemberInfo
        {
            private CodeType _type;

            public MemberInfo(DataColumn dc, CodeType type, string memberPrefix)
            {
                _type = type;

                ColumnName = dc.ColumnName.Contains(" ") ? "[" + dc.ColumnName + "]" : dc.ColumnName;
                Property = dc.ColumnName.Trim().Replace(" ", "");
                Property = Property.Substring(0, 1).ToUpper() + Property.Substring(1, Property.Length - 1);
                Member = memberPrefix + "_" + Property.Substring(0, 1).ToLower() + Property.Substring(1, Property.Length - 1);
                SystemType = SystemTypeToString(dc.DataType);
  
                //These statements are a matter of preference
                StringValue = dc.DataType.Equals(typeof(string)) || dc.DataType.Equals(typeof(DateTime)) ? "AddString(" + Member + ")" : Member; //it is important to filter strings for SQL Injection, hence the AddString method
                ConvertTo = "Convert.To" + (dc.DataType.Equals(typeof(Byte)) ? "Int32" : dc.DataType.Name) + "("; //A byte can fit inside of an Int32
            }

            public string ColumnName { get; set; }
            public string Member { get; set; }
            public string Property { get; set; }
            public string SystemType { get; set; }
            public string ConvertTo { get; set; }
            public string StringValue { get; set; }

            private string SystemTypeToString(Type t)
            {
                string strType = t.Name;

                //This is a matter of preference
                if (t.Equals(typeof(Int32)) || t.Equals(typeof(Byte)))
                    strType = _type == CodeType.CSharp ? "int" : "Integer";
                else if (t.Equals(typeof(Boolean)))
                    strType = _type == CodeType.CSharp ? "bool" : "Boolean";
                else if (_type == CodeType.CSharp)
                {
                    if (t.Equals(typeof(DateTime)))
                        strType = t.Name;
                    else if (t.IsValueType || t.Equals(typeof(string)))
                        strType = t.Name.ToLower();
                }
                else
                    strType = t.Name;

                return strType;
            }
        }
    }
     * */
}
