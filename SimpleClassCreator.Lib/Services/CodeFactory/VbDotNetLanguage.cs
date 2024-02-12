using System;
using System.Text;

/* This is from a very old version one of the simple class creator when I
 * was trying to support C# and VB.NET simultaneously.
 * I will eventually pull this out of here and put it in a graveyard folder. */
namespace SimpleClassCreator.Lib.Services.CodeFactory
{
    public class VbDotNetLanguage : DotNetLanguage
    {
        public VbDotNetLanguage(string className)
        {
            ClassName = CapitalizeFirstLetter(className);
        }

        public override void InitializeMotifValues()
        {
            _indexOpen = "(";
            _indexClose = ")";
            _vbFunction = "Function";
            _vbEndFunction = "End Function" + Environment.NewLine + Environment.NewLine;
            _vbEndSub = "End Sub" + Environment.NewLine + Environment.NewLine;
            _vbNext = "Next" + Environment.NewLine;
            Using = "Imports";
            Namespace = "Namespace";
            Class = "Class";
            Public = "Public";
            OpenNamespace = Namespace + " " + NamespaceName + Environment.NewLine;
            CloseNamespace = "End Namespace" + Environment.NewLine;
            OpenClass = Public + " " + Class + " " + ClassName + Environment.NewLine;
            CloseClass = "End Class" + Environment.NewLine;
            Private = "Private";
            Void = "Sub";
            Static = "Shared";
            New = "New";
            Null = "Nothing";
            EmptyConstructor = "Public Sub New()" + Environment.NewLine + Environment.NewLine + "End Sub" + Environment.NewLine + Environment.NewLine;
            ListOfTarget = "List(Of " + ClassName + ")";
            LineTerminator = Environment.NewLine;
            StartRegion = "#Region";
            EndRegion = "#End Region";
            ForEach = "For Each";
            In = "In";
            Return = "Return";
            FileExtension = "vb";
            DataContract = "<DataContract()> _" + Environment.NewLine;
            DataMember = "<DataMember()> _" + Environment.NewLine;
        }

        public override string CreateRegion(string regionName)
        {
            return base.CreateRegion(WrapInQuotes(regionName));
        }

        public override void CreateProperty(StringBuilder sb, ClassMemberStrings info)
        {
            //Private Member
            sb.AppendFormat("Private {0} As {1}{2}", info.Field, info.SystemTypeAlias, LineTerminator);

            if (IncludeSerializableAttribute)
                sb.Append(DataMember);

            //Public Property
            sb.AppendFormat("Public Property {0} As {1}{3}Get{3}Return {2}{3}End Get{3}Set(value As {1}){3}{2} = value{3}End Get{3}End Property", info.Property, info.SystemTypeAlias, info.Field, Environment.NewLine);
            sb.AppendLine().AppendLine();
        }

        public override void CreateObjectGenerationMethod(StringBuilder sb, string body)
        {
            //Public Shared Function ObjectGeneration(dt As DataTable) As List(Of Product)
            sb.AppendFormat("Public Shared Function ObjectGeneration(dr As IDataReader) As {0}{1}Dim obj As {0} = Nothing{1}{1}",
                ClassName, //0
                Environment.NewLine); //1

            //CreateForEach(sb, "dr", "DataRow", "dt.Rows", string.Format("obj = New {0}(){1}{2}{3}lst.Add(obj)", ClassName, LineTerminator, body, Environment.NewLine));

            sb.AppendFormat("{0}{0}Return obj{0}End Function{0}{0}", Environment.NewLine);
        }

        protected override void CreateForEach(StringBuilder sb, string name, string dataType, string collection, string body)
        {
            sb.AppendFormat("For Each {0} In {1}{3} {2}{3} Next", GetParameter(name, dataType), collection, body, Environment.NewLine);
        }

        public override void CreateUpdateMethod(StringBuilder sb, string updateStatement, ClassMemberStrings primaryKey)
        {
            sb.AppendFormat("Public Sub Update{0}(){4}Dim sb As New StringBuilder(){4}{4}sb.Append(\"UPDATE {0} SET \"){4}{1}{4}sb.Append(\" WHERE {2} = \").Append(obj.{3}).Append(\";\"){4}End Sub{4}{4}",
                ClassName, //0
                updateStatement, //1
                primaryKey.ColumnName, //2
                primaryKey.Property, //3
                Environment.NewLine); //4
        }

        public override void CreateInsertMethod(StringBuilder sb, string columns, string insertStatement)
        {
            sb.AppendFormat("Public Sub Insert{0}(){3}Dim sb As New StringBuilder(){3}{3}sb.Append(\"INSERT INTO {0} ({1}) VALUES ( \"){3}{2}{3}sb.Append(\");\"){3}End Sub{3}{3}",
                ClassName, //0
                columns, //1
                insertStatement, //2
                Environment.NewLine); //3
        }

        protected override string GetParameter(string name, string dataType)
        {
            return string.Format("{0} As {1}", name, dataType);
        }
    }
}
