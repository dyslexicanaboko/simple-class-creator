using System;
using SimpleClassCreator.Lib.Models;
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
            sq.Table = tableQuery;
            sq.IsSingleTableQuery = sq.Table != null;
            sq.HasPrimaryKey = rs.GenericSchema.PrimaryKey.Any();

            sq.ColumnsAll = new List<SchemaColumn>(rs.GenericSchema.Columns.Count);

            foreach (DataColumn dc in rs.GenericSchema.Columns)
            {
                var sqlServerColumn = rs.SqlServerSchema.Select($"ColumnName = '{dc.ColumnName}'").Single();

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
            
            sq.PrimaryKey.IsPrimaryKey = true;

            sq.ColumnsNoPk = sq
                .ColumnsAll
                .Where(x => !x.ColumnName.Equals(pk.ColumnName, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return sq;
        }
    }
}
