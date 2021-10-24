using System;
using System.Collections.Generic;
using System.Data;

namespace SimpleClassCreator.Lib.Services
{
    public class TypesService
    {
        //FYI:The enumerations below have a proper set of mappings going from Sql to System
        /*  System.Data.SqlDbType
                SQL Server specific
                C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Data.dll
            
            System.Data.DbType
                ANSI SQL (Generic)
                C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Data.Common.dll */

        /// <summary>
        /// Loose mapping going from System type to Sql Server database type.
        /// </summary>
        public static readonly Dictionary<Type, SqlDbType> MapSystemToSqlLoose = new Dictionary<Type, SqlDbType>
        {
            {typeof(bool), SqlDbType.Bit},
            {typeof(byte), SqlDbType.TinyInt},
            {typeof(short), SqlDbType.SmallInt},
            {typeof(int), SqlDbType.Int},
            {typeof(long), SqlDbType.BigInt},
            {typeof(string), SqlDbType.NVarChar}, //Could be Char, NChar or VarChar
            {typeof(char[]), SqlDbType.NVarChar}, //Could be Char, NChar or VarChar
            {typeof(byte[]), SqlDbType.VarBinary}, //Could be Binary
            {typeof(decimal), SqlDbType.Decimal},
            {typeof(float), SqlDbType.Real}, //System.Single -> float -> SqlDbType.Real
            {typeof(double), SqlDbType.Float}, //Do not confuse with System.float
            {typeof(TimeSpan), SqlDbType.Time},
            {typeof(DateTime), SqlDbType.DateTime2},
            {typeof(DateTimeOffset), SqlDbType.DateTimeOffset},
            {typeof(Guid), SqlDbType.UniqueIdentifier}
        };

        /// <summary>
        /// Loose mapping going from SQL Server database type to Database type. Does not account for all types!
        /// </summary>
        public static readonly Dictionary<SqlDbType, DbType> MapSqlDbTypeToDbTypeLoose = new Dictionary<SqlDbType, DbType>
        {
            { SqlDbType.BigInt, DbType.Int64 },
            { SqlDbType.Binary, DbType.Binary },
            { SqlDbType.Bit, DbType.Boolean },
            { SqlDbType.Char, DbType.AnsiStringFixedLength },
            { SqlDbType.Date, DbType.Date },
            { SqlDbType.DateTime, DbType.DateTime },
            { SqlDbType.DateTime2, DbType.DateTime2 },
            { SqlDbType.DateTimeOffset, DbType.DateTimeOffset },
            { SqlDbType.Decimal, DbType.Decimal },
            { SqlDbType.Float, DbType.Double },
            { SqlDbType.Int, DbType.Int32 },
            { SqlDbType.Money, DbType.Currency },
            { SqlDbType.NChar, DbType.StringFixedLength },
            { SqlDbType.NVarChar, DbType.String },
            { SqlDbType.Real, DbType.Single },
            { SqlDbType.SmallInt, DbType.Int16 },
            { SqlDbType.Time, DbType.Time },
            { SqlDbType.UniqueIdentifier, DbType.Guid },
            { SqlDbType.VarChar, DbType.AnsiString },
            { SqlDbType.Xml, DbType.Xml }
        };

        //I got the base of this list from here: https://stackoverflow.com/a/1362899/603807
        //I am not using this right now because I realized later that the CodeProvider DOES provide the aliases. Going to keep it around for now.
        /// <summary>
        /// Strong mapping of System types and their aliases. List has been extended to include
        /// structures that are not primitive types but are used as such.
        /// </summary>
        public static readonly Dictionary<Type, string> MapSystemToAliases = new Dictionary<Type, string>
        {
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(float), "float" }, //Single
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(object), "object" },
            { typeof(bool), "bool" },
            { typeof(char), "char" },
            { typeof(string), "string" },
            { typeof(void), "void" },
            //These don't have aliases because they are not primitives, however they are used as such
            { typeof(TimeSpan), "TimeSpan" },
            { typeof(DateTime), "DateTime" },
            { typeof(DateTimeOffset), "DateTimeOffset" },
            { typeof(Guid), "Guid" },
        };

        /// <summary>
        /// Strong mapping of Sql Server Database type lower case names to their equivalent Enumeration.
        /// </summary>
        public static readonly Dictionary<string, SqlDbType> SqlTypes = Utils.GetEnumDictionary<SqlDbType>(true);
    }
}
