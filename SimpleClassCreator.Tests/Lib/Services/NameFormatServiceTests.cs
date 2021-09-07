using NUnit.Framework;
using SimpleClassCreator.Lib.Services;
using System;
using SimpleClassCreator.Lib;

namespace SimpleClassCreator.Tests.Lib.Services
{
    [TestFixture]
    public class NameFormatServiceTests
    {
        private INameFormatService _service;

        [SetUp]
        public void Setup()
        {
            _service = new NameFormatService();
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

        [TestCase("TableName", "TableName")]
        [TestCase("tablename", "tablename")]
        [TestCase("dbo.TableName", "TableName")]
        [TestCase("dbo.[T a b l e N a m e]", "TableName")]
        [TestCase("dbo.[T A B L E N A M E]", "TABLENAME")]
        [TestCase("[T   a   b   l   e   N   a   m   e]", "TableName")]
        public void Get_class_name(string input, string expected)
        {
            //Arrange
            var tq = _service.ParseTableName(input);

            //Act
            var actual = _service.GetClassName(tq);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
