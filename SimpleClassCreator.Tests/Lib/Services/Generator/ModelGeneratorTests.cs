﻿using NUnit.Framework;
using SimpleClassCreator.Lib.Models;
using SimpleClassCreator.Lib.Services.CodeFactory;
using SimpleClassCreator.Lib.Services.Generators;
using System.Collections.Generic;
using System.Data;
using SimpleClassCreator.Lib;

namespace SimpleClassCreator.Tests.Lib.Services.Generator
{
    [TestFixture]
    public class ModelGeneratorTests
        : TestBase
    {
        private readonly ModelGenerator _generator;

        public ModelGeneratorTests()
        {
            var instructions = new ClassInstructions();

            _generator = new ModelGenerator(instructions);
        }

        [Test]
        public void Empty_list_creates_no_attributes()
        {
            //Arrange
            var lst = new List<string>();

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatClassAttributes", lst);

            //Assert
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void Empty_list_creates_no_namespaces()
        {
            //Arrange
            var lst = new List<string>();

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatNamespaces", lst);

            //Assert
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void Empty_list_creates_no_properties()
        {
            //Arrange
            var lst = new List<ClassMemberStrings>();

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatProperties", lst);

            //Assert
            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void One_attribute_creates_expected_string()
        {
            //Arrange
            var item = "FakeAttribute";

            var lst = new List<string>
            {
                item
            };

            var expected = "[" + item + "]";

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatClassAttributes", lst);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void One_namespace_creates_expected_string()
        {
            //Arrange
            var item = "Fake.NameSpace";

            var lst = new List<string>
            {
                item
            };

            var expected = "using " + item + ";";

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatNamespaces", lst);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void One_ClassMemberString_creates_expected_string()
        {
            //Arrange
            var dc = new DataColumn("FakeColumn", typeof(int));
            dc.AllowDBNull = false;
            
            var item = new ClassMemberStrings(dc, CodeType.CSharp, null);

            var lst = new List<ClassMemberStrings>
            {
                item
            };

            var expected = "        public int FakeColumn { get; set; }";

            //Act
            var actual = InvokePrivateMethod<ModelGenerator, string>(_generator, "FormatProperties", lst);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
