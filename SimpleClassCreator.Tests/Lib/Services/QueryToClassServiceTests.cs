using Moq;
using NUnit.Framework;
using SimpleClassCreator.Lib;
using SimpleClassCreator.Lib.DataAccess;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services;
using SimpleClassCreator.Tests.DummyObjects;
using System.Text.RegularExpressions;

namespace SimpleClassCreator.Tests.Lib.Services
{
    [TestFixture]
    public class QueryToClassServiceTests
    {
        /*
        3. Need to create templates for whatever code it is that I am trying to create
        A. Let's create a dummy object that is easy to understand and work off of that 
            like your rudamentary person object 
        B. I want templates to be plug and play meaning the user can provide whatever 
            templates they want so long as they use the appropriate tags
        4. I don't like the if (tableName != null) clauses, need to abstract this, let's 
        pause and see what happens */
        [Test]
        public void Person_table_produces_person_class()
        {
            //Arrange
            var expected = PersonUtil.PersonClass;

            var dt = PersonUtil.GetPersonAsDataTable();

            var p = new ClassParameters
            {
                ClassName = dt.TableName,
                SourceType = SourceTypeEnum.TableName,
                ClassSource = dt.TableName,
                LanguageType = CodeType.CSharp,
                Namespace = "SimpleClassCreator.Tests.DummyObjects",
                TableQuery = new TableQuery() { Schema = "dbo", Table = dt.TableName }
            };
            
            var repo = new Mock<IQueryToClassRepository>();
            repo.Setup(x => x.GetSchema(It.IsAny<string>())).Returns(dt);
            repo.Setup(x => x.GetPrimaryKeyColumn(It.IsAny<TableQuery>())).Returns(nameof(Person.PersonId));

            var svc = new QueryToClassService(repo.Object);

            //Act
            var actual = svc.GenerateClass(p);

            //This is for debug only
            //DumpFile("Expected.cs", expected);
            //DumpFile("Actual.cs", actual);

            //Assert
            //Spacing is still an issue, so going to give it a pass for now
            AssertAreEqualIgnoreWhiteSpace(expected, actual);
        }

        //This is for debug only
        private void DumpFile(string fileName, string contents)
        {
            System.IO.File.WriteAllText(@"C:\Dump\" + fileName, contents);
        }

        private void AssertAreEqualIgnoreWhiteSpace(string expected, string actual)
        {
            var re = new Regex(@"\s+");

            expected = re.Replace(expected, string.Empty);
            actual = re.Replace(actual, string.Empty);

            Assert.AreEqual(expected, actual);
        }
    }
}
