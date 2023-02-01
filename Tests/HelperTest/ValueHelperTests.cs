using InfoTecs.Api.Exceptions;
using InfoTecs.Api.Helpers;
using InfoTecs.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfoTecs.Api.Tests.HelperTest.TestData;
using Moq;
using SystemWrapper.IO;
using SystemWrapper.IO.Compression;
using System.IO.Abstractions;

namespace InfoTecs.Api.Tests.HelperTest
{
    public class ValueHelperTests
    {
        private readonly ValueHelper _valueHelper;
        private readonly ValueHelperTestData _testData;

        public ValueHelperTests()
        {
            _valueHelper = new ValueHelper();
            _testData = new ValueHelperTestData();
        }

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("2002-03-04_13-08-10;16;42,44", 1)]
        [TestCase("2000-01-01_00-00-00;0;0", 2)]
        [TestCase("2000-01-01_00-00-00;228;228", 0)]
        public void GetValueFromString_Test(string line, int numberCase)
        {
            //given
            var expected = _testData.GetValueModelsForTest(numberCase);

            //when
            var actual = _valueHelper.GetValueFromString(line, numberCase);

            //then
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(ValueModel), actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("2000-00-00_00-00-00;228;228,228", 1)]
        [TestCase("", 1)]
        [TestCase("444", 1)]
        [TestCase("2000-01-01;228;228,228", 1)]
        [TestCase("2000-01-01_00-00-00;228,00;228,228", 1)]
        [TestCase("2000-01-01_00-00-00;22800;228.228", 1)]
        [TestCase("2000/01/01 10:10:10;22800;228.228", 1)]
        [TestCase("1999-01-01_00-00-00;228;228,228", 2)]
        [TestCase("2009-01-01_00-00-00;-228;-228,228", 2)]
        [TestCase("2006-01-01_00-00-00;-228;228,228", 2)]
        [TestCase("2019-01-01_00-00-00;228;-228,228", 2)]
        //[TestCase("2019-01-01_00-00-00;228;0008,228", 0)]
        public void GetValueFromString_NegativeTest(string line, int numberCase)
        {
            //given
            var numberLine = 228;
            var expectedMessage = _testData.GetExceptionMessage(numberCase, numberLine);
            var typeException = _testData.GetTypeExceptionForNegativeTest(numberCase);

            //when
            Exception ex = Assert.Throws(typeException, () => _valueHelper.GetValueFromString(line, numberLine));

            //then
            Assert.That(expectedMessage, Is.EqualTo(ex.Message));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void ReadValuesFromLines_Test(int numberCase)
        {
            //given
            var lines = _testData.GetReadLinesForTest(numberCase);
            var expected = _testData.GetValueModelsForExpectedResult(numberCase);

            //when
            var actual =_valueHelper.ReadValuesFromLines(lines);

            //than
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(1,1,1)]
        [TestCase(2,0, 1)]
        [TestCase(3, 2, 4)]
        [TestCase(0,3,0)]
        public void ReadValuesFromLines_NegativeTest(int numberCase, int numberExceptionCase, int numberLine)
        {
            //given
            var lines = _testData.GetReadLinesForNegativeTest(numberCase);
            var typeException = _testData.GetTypeExceptionForNegativeTest(numberExceptionCase);
            var expectedMessage = _testData.GetExceptionMessage(numberExceptionCase, numberLine);
            //when
            Exception ex = Assert.Throws(typeException, () => _valueHelper.ReadValuesFromLines(lines));
            

            //than
            Assert.IsInstanceOf(typeException, ex);
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}
