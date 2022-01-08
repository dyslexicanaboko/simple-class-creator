using System;

namespace SimpleClassCreator.Lib.Models
{
    public class SchemaColumn
    {
        public SchemaColumn()
        {

        }

        public SchemaColumn(System.Reflection.PropertyInfo property)
        {
            IsDbNullable = !property.PropertyType.IsValueType;

            ColumnName = property.Name;

            SystemType = property.PropertyType;

            SqlType = System.Data.SqlDbType.Int.ToString(); //This is to satisfy the ClassMemberStrings class, it won't be used
        }

        public bool IsPrimaryKey { get; set; }

        public bool IsIdentity { get; set; }

        public bool IsDbNullable { get; set; }

        public string ColumnName { get; set; }
        
        public Type SystemType { get; set; }
        
        public string SqlType { get; set; }
        
        public int Size { get; set; }
        
        public int Precision { get; set; }
        
        public int Scale { get; set; } 
    }
}
