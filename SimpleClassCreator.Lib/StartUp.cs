using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleClassCreator.Lib
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                List<string> lst = new List<string>(args);

                //Execute(string tableName, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties)
                //switch (lst.Count)
                //{
                //    case 2:
                //        QueryToClassService.Execute(
                //            lst[0], //Table Name
                //            GetEnum(lst[1])); //Code Type
                //        break;
                //    case 3:
                //        QueryToClassService.Execute(
                //            lst[0], //Table Name
                //            GetEnum(lst[1]), //Code Type
                //            GetMemberPrefix(lst[2])); //Member Prefix
                //        break;
                //    case 4:
                //        QueryToClassService.Execute(
                //            lst[0], //Table Name
                //            GetEnum(lst[1]), //Code Type
                //            GetMemberPrefix(lst[2]), //Member Prefix
                //            Convert.ToBoolean(lst[3])); //Include WCF Tags  
                //        break;
                //    case 5:
                //        QueryToClassService.Execute(
                //            lst[0], //Table Name
                //            GetEnum(lst[1]), //Code Type
                //            GetMemberPrefix(lst[2]), //Member Prefix
                //            Convert.ToBoolean(lst[3]), //Include WCF Tags  
                //            Convert.ToBoolean(lst[4])); //Build Out Properties
                //        break;
                //    default:
                //        PrintCommandLineMenu();
                //        break;
                //}
            }
            else
            {
                //Generator.Execute("Product", CodeType.VBNet, "m", true, true);
                Wizard();
            }
            //Generator.Execute("Product", CodeType.CSharp);
            //Generator.Execute("SELECT TOP 1 * FROM Product", "Product", CodeType.VBNet, string.Empty, true);

            if (args.Length == 0)
            {
                //DO NOT DELETE THIS LINE
                Console.WriteLine("Press Any Key to Continue...");
                Console.ReadLine();
            }
        }

        private static void Wizard()
        {
            bool includeWCFTags = false,
                 buildOutProperties = false,
                 tableBasedSet = false;

            string strTableName = string.Empty,
                   strMemberPrefix = string.Empty,
                   strSqlQuery = string.Empty,
                   strClassName = string.Empty;

            CodeType ct = CodeType.CSharp;

            tableBasedSet = (QuestionAndAnswer("1 - Table, 2 - Query") == "1");

            //Use the table based set of method definitions
            if (tableBasedSet)
            {
                //Execute(string tableName, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties)
                strTableName = QuestionAndAnswer("Table Name?");
            }
            else //Use the query based set of method definitions
            {
                //Execute(string sqlQuery, string className, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties, string tableName = null)
                strSqlQuery = QuestionAndAnswer("Query?");
                strClassName = QuestionAndAnswer("Class Name?");
                strTableName = null;
            }

            ct = GetEnum(QuestionAndAnswer("Code Type? {0 - C#, 1 - VB.Net}"));
            strMemberPrefix = QuestionAndAnswer("Member Prefix? (Optional - just leave blank)");
            includeWCFTags = Convert.ToBoolean(QuestionAndAnswer("Include WCF Tags? {true/false}"));
            buildOutProperties = Convert.ToBoolean(QuestionAndAnswer("Build Out Properties? {true/false}"));

            //if (tableBasedSet)
            //    QueryToClassService.Execute(strTableName, ct, strMemberPrefix, includeWCFTags, buildOutProperties);
            //else
            //    QueryToClassService.Execute(strSqlQuery, strClassName, ct, strMemberPrefix, includeWCFTags, buildOutProperties);
        }

        private static string QuestionAndAnswer(string question)
        {
            Console.WriteLine(question);

            return Console.ReadLine();
        }

        private static string GetMemberPrefix(string prefix)
        {
            if (prefix == "!" || string.IsNullOrWhiteSpace(prefix))
                prefix = string.Empty;

            return prefix;
        }

        private static CodeType GetEnum(string argument)
        {
            return (CodeType)Enum.Parse(typeof(CodeType), argument);
        }

        private static void PrintCommandLineMenu()
        {
            //Execute(string tableName, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties)
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Minimum 2 Arguments, Maximum 5 Arguments");
            sb.AppendLine();
            sb.AppendLine("Command Line Can Only Handle Single Tables:");
            sb.AppendLine("[Table Name]");
            sb.AppendLine("[Code Type] : {0 - C#, 1 - VB.Net}]");
            sb.AppendLine("[Member Prefix] : {use ! for blank} Ex: m_name vs. _name");
            sb.AppendLine("[Include WCF Tags] : {true, false}");
            sb.AppendLine("[Build Out Properties] : {true, false}");

            Console.WriteLine(sb.ToString());
        }
    }
}

                    //Execute(string sqlQuery, string className, CodeType language, string memberPrefix, bool includeWCFTags, bool buildOutProperties, string tableName = null)
                    //switch (lst.Count)
                    //{
                    //    case 3:
                    //        Generator.Execute(
                    //            lst[0], //SQL Query
                    //            lst[1], //Class Name
                    //            GetEnum(lst[2])); //Code Type
                    //        break;
                    //    case 4:
                    //        Generator.Execute(
                    //            lst[0], //SQL Query
                    //            lst[1], //Class Name
                    //            GetEnum(lst[2]), //Code Type
                    //            lst[3]); //Member Prefix
                    //        break;
                    //    case 5:
                    //        Generator.Execute(
                    //            lst[0], //SQL Query
                    //            lst[1], //Class Name
                    //            GetEnum(lst[2]), //Code Type
                    //            lst[3], //Member Prefix
                    //            Convert.ToBoolean(lst[4])); //Include WCF Tags 
                    //        break;
                    //    case 6:
                    //        Generator.Execute(
                    //            lst[0], //SQL Query
                    //            lst[1], //Class Name
                    //            GetEnum(lst[2]), //Code Type
                    //            lst[3], //Member Prefix
                    //            Convert.ToBoolean(lst[4]), //Include WCF Tags  
                    //            Convert.ToBoolean(lst[5])); //Build Out Properties
                    //        break;
                    //    default:
                    //        PrintMenu();
                    //        break;