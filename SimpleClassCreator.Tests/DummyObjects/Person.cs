using System;
using System.Data;

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
        public static DataTable GetPersonAsDataTable()
        {
            var dt = new DataTable(nameof(Person));

            var dc = GetNonNullColumn(nameof(Person.PersonId), typeof(int));
            dt.Columns.Add(dc);

            dc = new DataColumn(nameof(Person.Age), typeof(int));
            dt.Columns.Add(dc);

            dc = GetNonNullColumn(nameof(Person.FirstName), typeof(string));
            dt.Columns.Add(dc);

            dc = new DataColumn(nameof(Person.MiddleName), typeof(string));
            dt.Columns.Add(dc);

            dc = GetNonNullColumn(nameof(Person.LastName), typeof(string));
            dt.Columns.Add(dc);

            dc = new DataColumn(nameof(Person.BirthDate), typeof(DateTime));
            dt.Columns.Add(dc);

            return dt;
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
