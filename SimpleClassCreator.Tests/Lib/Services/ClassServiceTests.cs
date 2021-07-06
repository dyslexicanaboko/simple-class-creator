using System;
using NUnit.Framework;
using SimpleClassCreator.Services;

namespace SimpleClassCreator.Tests.Lib.Services
{
    [TestFixture]
    public class ClassServiceTests
    {
        private IClassService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ClassService();
        }

        [TestCase("a", "[dbo].[a]")]
        [TestCase("a.b", "[a].[b]")]
        [TestCase("[a b]", "[dbo].[a b]")]
        [TestCase("a.[b c]", "[a].[b c]")]
        [TestCase("[a b].[c d]", "[a b].[c d]")]
        [TestCase("[a b].c", "[a b].[c]")]
        public void Parse_different_table_names_successfully(string input, string expected)
        {
            //Arrange


            //Act
            var tq = _service.ParseTableName(input);

            var actual = _service.FormatTableQuery(tq, TableQueryQualifiers.Schema | TableQueryQualifiers.Table);

            //Assert
            Assert.IsTrue(expected.Equals(actual, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
