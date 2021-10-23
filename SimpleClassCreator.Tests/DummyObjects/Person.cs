using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;

namespace SimpleClassCreator.Tests.DummyObjects
{
    public class Person
    {
        public int PersonId { get; set; }

        public int Age { get; set; }
        
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }

    public static class PersonUtil
    {
        public static SchemaQuery GetPersonAsSchemaQuery()
        {
            var sq = new SchemaQuery
            {
                TableQuery = new TableQuery { Schema = "dbo", Table = nameof(Person) },
                IsSolitaryTableQuery = true,
                HasPrimaryKey = true
            };

            sq.PrimaryKey = GetSchemaColumn(nameof(Person.PersonId), typeof(int), false);
            sq.ColumnsNoPk = new List<SchemaColumn>
            {
                GetSchemaColumn(nameof(Person.Age), typeof(int), true),
                GetSchemaColumn(nameof(Person.FirstName), typeof(string)),
                GetSchemaColumn(nameof(Person.MiddleName), typeof(string), true),
                GetSchemaColumn(nameof(Person.LastName), typeof(string)),
                GetSchemaColumn(nameof(Person.BirthDate), typeof(DateTime), true)
            };

            //Order matters
            var lst = new List<SchemaColumn>();
            lst.Add(sq.PrimaryKey);
            lst.AddRange(sq.ColumnsNoPk);
            
            sq.ColumnsAll = lst;

            return sq;
        }

        private static SchemaColumn GetSchemaColumn(string columnName, Type type, bool isNullable = false)
        {
            var c = new SchemaColumn
            {
                ColumnName = columnName,
                SystemType = type,
                IsDbNullable = isNullable,
                SqlType = TypesService.MapSystemToSqlLoose[type].ToString()
            };

            return c;
        }

        private static DataColumn GetNonNullColumn(string columnName, Type type)
        {
            var dc = new DataColumn(columnName, type);
            dc.AllowDBNull = false;

            return dc;
        }

        public const string PersonClass = 
@"using System;

namespace SimpleClassCreator.Tests.DummyObjects
{

    public class Person
    {
        public int PersonId { get; set; }

        public int? Age { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
";
    }

    /*
        This is not used by anything directly.
        I am hand crafting an object here to copy and paste it as a string for unit testing output comparison.
     */
    public class PersonService
    {
        
    }

    public class PersonRepository
    {

    }
}
