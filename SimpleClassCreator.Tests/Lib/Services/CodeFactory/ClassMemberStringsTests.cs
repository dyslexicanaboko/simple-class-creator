using NUnit.Framework;
using SimpleClassCreator.Services.CodeFactory;
using System;
using System.Data;

namespace SimpleClassCreator.Tests.Lib.Services.CodeFactory
{
    [TestFixture]
    public class ClassMemberStringsTests
    {
        [TestCase(typeof(string), ExpectedResult = "string")]
        [TestCase(typeof(int), ExpectedResult = "int")]
        [TestCase(typeof(byte), ExpectedResult = "byte")]
        [TestCase(typeof(DateTime), ExpectedResult = "DateTime")]
        [TestCase(typeof(Guid), ExpectedResult = "Guid")]
        public string Types_are_formatted_as_expected(Type type)
        {
            //Arrange
            var c = new DataColumn("DoesNotMatter", type);
            c.AllowDBNull = false;

            //Act
            var m = new ClassMemberStrings(c, CodeType.CSharp, string.Empty);

            return m.SystemType;
        }
    }
}
