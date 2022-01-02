using SimpleClassCreator.Lib.Exceptions;
using SimpleClassCreator.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SimpleClassCreator.Lib.DataAccess
{
    /// <summary>
    /// The Data Access layer for the Code Generator
    /// </summary>
    public class QueryToClassRepository
        : BaseRepository, IQueryToClassRepository
    {
        public SchemaQuery GetSchema(TableQuery tableQuery, string query)
        {
            var rs = GetFullSchemaInformation(query);

            var sq = new SchemaQuery();
            sq.Query = query;
            sq.TableQuery = tableQuery;
            sq.IsSolitaryTableQuery = sq.TableQuery != null;
            sq.HasPrimaryKey = rs.GenericSchema.PrimaryKey.Any();

            sq.ColumnsAll = new List<SchemaColumn>(rs.GenericSchema.Columns.Count);

            foreach (DataColumn dc in rs.GenericSchema.Columns)
            {
                var arr = rs.SqlServerSchema.Select($"ColumnName = '{dc.ColumnName}'");

                //If the query provided contains repeat column names, this won't work. Let the user know they have to change their query.
                if (arr.Length > 1)
                {
                    throw new NonUniqueColumnException(dc.ColumnName);
                }

                var sqlServerColumn = arr.Single();

                var sc = new SchemaColumn
                {
                    ColumnName = dc.ColumnName,
                    IsDbNullable = dc.AllowDBNull,
                    SystemType = dc.DataType,
                    SqlType = sqlServerColumn.Field<string>("DataTypeName"),
                    Size = sqlServerColumn.Field<int>("ColumnSize"),
                    Precision = sqlServerColumn.Field<short>("NumericPrecision"),
                    Scale = sqlServerColumn.Field<short>("NumericScale")
                };

                sq.ColumnsAll.Add(sc);
            }

            if (!sq.HasPrimaryKey) return sq;
            
            //TODO: This is assuming a single column is the primary key which is a bad idea, but okay for now
            var pk = rs.GenericSchema.PrimaryKey.First();

            sq.PrimaryKey = sq
                .ColumnsAll
                .Single(x => x.ColumnName.Equals(pk.ColumnName, StringComparison.InvariantCultureIgnoreCase));

            sq.PrimaryKey.IsIdentity = pk.AutoIncrement;
            sq.PrimaryKey.IsPrimaryKey = true;

            sq.ColumnsNoPk = sq
                .ColumnsAll
                .Where(x => !x.ColumnName.Equals(pk.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return sq;
        }
    }
}
