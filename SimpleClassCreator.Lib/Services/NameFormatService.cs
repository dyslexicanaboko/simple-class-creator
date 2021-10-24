using SimpleClassCreator.Lib.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleClassCreator.Lib.Services
{
    public class NameFormatService
        : INameFormatService
    {
        private const string DefaultSchema = "dbo";
        private readonly Regex _whiteSpace = new Regex(@"\s+");

        public TableQuery ParseTableName(string tableNameQuery)
        {
            //Regex.Replace(tableNameQuery, @"\s+", string.Empty)
            
            //Remove all square brackets so they can just be reapplied again later
            //Skip the guess work on which segment does or does not have them
            var q = tableNameQuery
                .Replace("[", string.Empty)
                .Replace("]", string.Empty);

            var arr = q.Split('.');

            var tbl = new TableQuery();

            switch (arr.Length)
            {
                //Table
                case 1:
                    tbl.Schema = DefaultSchema;
                    tbl.Table = arr[0];
                    break;
                //Schema.Table
                case 2:
                    tbl.Schema = arr[0];
                    tbl.Table = arr[1];
                    break;
                //Database.Schema.Table
                case 3:
                    tbl.Database = arr[0];
                    tbl.Schema = arr[1];
                    tbl.Table = arr[2];
                    break;
                //LinkedServer.Database.Schema.Table
                case 4:
                    tbl.LinkedServer = arr[0];
                    tbl.Database = arr[1];
                    tbl.Schema = arr[2];
                    tbl.Table = arr[3];
                    break;
            }

            //Copy the unqualified version before it is qualified
            tbl.TableUnqualified = tbl.Table;

            return tbl;
        }

        public string GetClassName(TableQuery tableQuery)
        {
            var c = _whiteSpace.Replace(tableQuery.TableUnqualified, string.Empty);

            return c;
        }

        public string FormatTableQuery(string tableQuery, TableQueryQualifiers qualifiers = TableQueryQualifiers.Schema | TableQueryQualifiers.Table)
        {
            var tq = ParseTableName(tableQuery);

            var str = FormatTableQuery(tq, qualifiers);

            return str;
        }

        public string FormatTableQuery(TableQuery tableQuery, TableQueryQualifiers qualifiers = TableQueryQualifiers.Schema | TableQueryQualifiers.Table)
        {
            if (qualifiers == TableQueryQualifiers.None) throw new ArgumentException("Qualifier cannot be none.", nameof(qualifiers));

            var t = tableQuery;

            var lst = new List<string>();

            if (qualifiers.HasFlag(TableQueryQualifiers.LinkedServer))
            {
                Qualify(lst, TableQueryQualifiers.LinkedServer, t.LinkedServer);
            }

            if (qualifiers.HasFlag(TableQueryQualifiers.Database))
            {
                Qualify(lst, TableQueryQualifiers.Database, t.Database);
            }

            if (qualifiers.HasFlag(TableQueryQualifiers.Schema))
            {
                var schema = t.Schema;

                if (string.IsNullOrWhiteSpace(schema))
                {
                    schema = DefaultSchema;
                }

                Qualify(lst, TableQueryQualifiers.Schema, schema);
            }

            if (qualifiers.HasFlag(TableQueryQualifiers.Table))
            {
                Qualify(lst, TableQueryQualifiers.Table, t.Table);
            }

            var strTableQuery = string.Join(".", lst);

            return strTableQuery;
        }

        private void Qualify(IList<string> segments, TableQueryQualifiers qualifier, string segment)
        {
            if (string.IsNullOrWhiteSpace(segment)) throw new ArgumentException($"{qualifier} cannot be null or whitespace.");

            var qualified = $"[{segment}]";

            segments.Add(qualified);
        }
    }
}
