using Api.Exceptions;
using Api.Helpers;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tests.HelperTest.TestData;
using Moq;
using SystemWrapper.IO;
using SystemWrapper.IO.Compression;
using SystemInterface.IO;

namespace Tests.HelperTest
{
    public class ValueHelperTests
    {
        private readonly ValueHelper _valueHelper;
        private readonly ValueHelperTestData _testData;
        private Mock<IFile> _fileMock;
        FileWrap fileWrapRepository;
        public ValueHelperTests()
        {
            _valueHelper = new ValueHelper();
            _testData = new ValueHelperTestData();
        }

        [SetUp]
        public void Setup()
        {
            _fileMock = new Mock<IFile>();
            fileWrapRepository = Mock.Of<FileWrap>();
        }

        [TestCase("2002-03-04_13-08-10;16;42,44", 1)]
        [TestCase("2000-01-01_00-00-00;228;228,228", 228)]
        public void GetValueFromString_Test(string line, int number)
        {
            //given
            var expected = _testData.GetValueModelsForTest(number);

            //when
            var actual = _valueHelper.GetValueFromString(line, number);

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
        [TestCase("1999-01-01_00-00-00;228;228,228", 0)]
        [TestCase("2009-01-01_00-00-00;-228;-228,228", 0)]
        [TestCase("2006-01-01_00-00-00;-228;228,228", 0)]
        [TestCase("2019-01-01_00-00-00;228;-228,228", 0)]
        //[TestCase("2019-01-01_00-00-00;228;0008,228", 0)]
        public void GetValueFromString_NegativeTest(string line, int numberMessage)
        {
            //given
            var expectedMessage = _testData.GetException(numberMessage);
            var type = numberMessage == 0 ? typeof(ValueIsNotInRangeException) : typeof(InvalidLineException);
            //when
            Exception ex = Assert.Throws(type, () => _valueHelper.GetValueFromString(line, 228));

            //then
            Assert.That(expectedMessage, Is.EqualTo(ex.Message));
        }

        [TestCase("data.csv")]
        public void GetLinesFromCsvTest(string fakePath)
        {
            //given
            var lines = _testData.GetReadLinesForTest(fakePath);
            _fileMock.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(lines);
            _fileMock.Setup(x=>x.Exists(fakePath)).Returns(true);
            //fileWrapRepository.
            var sut = new ValueHelper();
            var expected = lines;

            //when
            var actual = sut.GetLinesFromCsv(fakePath);

            //than
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
