using System;
using System.Text;

/* This is from a very old version one of the simple class creator when I
 * was trying to support C# and VB.NET simultaneously.
 * I will eventually pull this out of here and put it in a graveyard folder. */
namespace SimpleClassCreator.Lib.Services.CodeFactory
{
  public class CSharpLanguage : DotNetLanguage
    {
        private bool _buildOutProperties = false;

        public CSharpLanguage(string className)
        {
            ClassName = CapitalizeFirstLetter(className);
        }

        public override void InitializeMotifValues()
        {
            _indexOpen = "[";
            _indexClose = "]";
            Using = "using";
            Namespace = "namespace";
            Class = "class";
            Public = "public";
            OpenNamespace = Namespace + " " + NamespaceName + Environment.NewLine + "{" + Environment.NewLine;
            CloseNamespace = "}" + Environment.NewLine;
            OpenClass = Public + " " + Class + " " + ClassName + Environment.NewLine + "{" + Environment.NewLine;
            CloseClass = "}" + Environment.NewLine;
            Private = "private";
            Void = "void";
            Static = "static";
            New = "new";
            Null = "null";
            EmptyConstructor = "public " + ClassName + "()" + Environment.NewLine + "{" + Environment.NewLine + Environment.NewLine + "}" + Environment.NewLine + Environment.NewLine;
            ListOfTarget = "List<" + ClassName + ">";
            LineTerminator = ";" + Environment.NewLine;
            StartRegion = "#region";
            EndRegion = "#endregion";
            ForEach = "foreach";
            In = "in";
            Return = "return";
            FileExtension = "cs";
            DataContract = "[DataContract]" + Environment.NewLine;
            DataMember = "[DataMember]" + Environment.NewLine;
        }

        public override void CreateProperty(StringBuilder sb, ClassMemberStrings info)
        {
            if (_buildOutProperties)
            {
                //Private Member
                sb.AppendFormat("private {0} {1}{2}", info.SystemTypeAlias, info.Field, LineTerminator);

                if (IncludeSerializableAttribute)
                    sb.Append(DataMember);

                //Public Property
                sb.AppendFormat("public {0} {1}{3}{{{3} get {{ return {2}; }}{3} set {{ {2} = value; }}{3}}} ", info.SystemTypeAlias, info.Property, info.Field, Environment.NewLine);
            }
            else
            {
                if (IncludeSerializableAttribute)
                    sb.Append(DataMember);

                sb.AppendFormat("{0} {1} {2} {{ get; set; }}", Public, info.SystemTypeAlias, info.Property);
            }

            sb.AppendLine().AppendLine();
        }

        public override void CreateObjectGenerationMethod(StringBuilder sb, string body)
        {
            var n = Environment.NewLine;

            sb.Append(
                $"public static {ClassName} ObjectGeneration(IDataReader dr){n}{{{n}var obj = new {ClassName}();{n}{n}");

            sb.Append(body);
            //CreateForEach(sb, "dr", "DataRow", "dt.Rows", string.Format("{3}obj = new {0}(){1}{2}{3}lst.Add(obj);", ClassName, LineTerminator, body, Environment.NewLine));

            sb.AppendFormat("{0}return obj;{0}}}{0}{0}", Environment.NewLine);
        }

        protected override void CreateForEach(StringBuilder sb, string name, string dataType, string collection, string body)
        {
            sb.AppendFormat("foreach({0} in {1}){3}{{ {2}{3} }}{3}", GetParameter(name, dataType), collection, body, Environment.NewLine);
        }

        public override void CreateUpdateMethod(StringBuilder sb, string updateStatement, ClassMemberStrings primaryKey)
        {
            sb.AppendFormat("public void Update{0}(){4}{{{4}StringBuilder sb = new StringBuilder();{4}{4}sb.Append(\"UPDATE {0} SET \");{4}{1};{4}sb.Append(\" WHERE {2} = \").Append(obj.{3}).Append(\";\");{4}}}{4}{4}", 
                ClassName, //0
                updateStatement, //1
                primaryKey.ColumnName, //2
                primaryKey.Property, //3
                Environment.NewLine); //4
        }

        public override void CreateInsertMethod(StringBuilder sb, string columns, string insertStatement)
        {
            sb.AppendFormat("public void Insert{0}(){3}{{{3}StringBuilder sb = new StringBuilder();{3}{3}sb.Append(\"INSERT INTO {0} ({1}) VALUES ( \");{3}{2};{3}sb.Append(\");\");{3}}}{3}{3}",
                ClassName, //0
                columns, //1
                insertStatement, //2
                Environment.NewLine); //3
        }

        protected override string GetParameter(string name, string dataType)
        {
            return string.Format("{0} {1}", dataType, name);
        }

        protected override string GetVariable(string name, string dataType, string setValue = "")
        {
            return base.GetVariable(name, dataType, setValue) + ";";
        }
    }
}
