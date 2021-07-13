﻿using NUnit.Framework;
using SimpleClassCreator.Services.CodeFactory;
using System;
using System.Data;

namespace SimpleClassCreator.Tests.Lib.Services.CodeFactory
{
    [TestFixture]
    public class ClassMemberStringsTests
    {
        [TestCase(typeof(DateTime), ExpectedResult = "DateTime")]
        [TestCase(typeof(DateTimeOffset), ExpectedResult = "DateTimeOffset")]
        [TestCase(typeof(Guid), ExpectedResult = "Guid")]
        [TestCase(typeof(TimeSpan), ExpectedResult = "TimeSpan")]
        [TestCase(typeof(bool), ExpectedResult = "bool")]
        [TestCase(typeof(byte), ExpectedResult = "byte")]
        [TestCase(typeof(byte[]), ExpectedResult = "byte[]")]
        [TestCase(typeof(decimal), ExpectedResult = "decimal")]
        [TestCase(typeof(double), ExpectedResult = "double")]
        [TestCase(typeof(float), ExpectedResult = "float")]
        [TestCase(typeof(int), ExpectedResult = "int")]
        [TestCase(typeof(long), ExpectedResult = "long")]
        [TestCase(typeof(short), ExpectedResult = "short")]
        [TestCase(typeof(string), ExpectedResult = "string")]
        public string Types_are_formatted_as_expected(Type type)
        {
            //Arrange
            var c = new DataColumn("DoesNotMatter", type);
            c.AllowDBNull = false;

            //Act
            var m = new ClassMemberStrings(c, CodeType.CSharp, string.Empty);

            return m.SystemType;
        }

        [TestCase(typeof(DateTime), ExpectedResult = "DateTime?")]
        [TestCase(typeof(DateTimeOffset), ExpectedResult = "DateTimeOffset?")]
        [TestCase(typeof(Guid), ExpectedResult = "Guid?")]
        [TestCase(typeof(TimeSpan), ExpectedResult = "TimeSpan?")]
        [TestCase(typeof(bool), ExpectedResult = "bool?")]
        [TestCase(typeof(byte), ExpectedResult = "byte?")]
        [TestCase(typeof(byte[]), ExpectedResult = "byte[]")]
        [TestCase(typeof(decimal), ExpectedResult = "decimal?")]
        [TestCase(typeof(double), ExpectedResult = "double?")]
        [TestCase(typeof(float), ExpectedResult = "float?")]
        [TestCase(typeof(int), ExpectedResult = "int?")]
        [TestCase(typeof(long), ExpectedResult = "long?")]
        [TestCase(typeof(short), ExpectedResult = "short?")]
        [TestCase(typeof(string), ExpectedResult = "string")]
        public string Nullable_types_are_formatted_as_expected(Type type)
        {
            //Arrange
            var c = new DataColumn("DoesNotMatter", type);
            c.AllowDBNull = true;

            //Act
            var m = new ClassMemberStrings(c, CodeType.CSharp, string.Empty);

            return m.SystemType;
        }
    }
}
