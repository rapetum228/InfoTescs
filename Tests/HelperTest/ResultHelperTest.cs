using Api.Helpers;
using InfoTecs.Api.Exceptions;
using InfoTecs.Api.Helpers;
using InfoTecs.Api.Models;
using InfoTecs.Api.Tests.HelperTest.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTecs.Api.Tests.HelperTest
{
    public class ResultHelperTest
    {
        private readonly ResultHelper _resultHelper;
        private readonly ResultHelperTestData _testData;

        public ResultHelperTest()
        {
            _resultHelper = new ResultHelper();
            _testData = new ResultHelperTestData();
        }


        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void CalculateMediana_Test(int numberCase)
        {
            //given
            var values = _testData.GetDoublesForTest(numberCase);
            var expected = _testData.GetExpectedMediana(numberCase);

            //when
            var actual =_resultHelper.CalculateMediana(values);

            //than
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void CalculateMediana_NegativeTest()
        {
            //given
            var values = new List<double>();
            var expected = "The file must contain at least one line";
            //when
            Exception ex = Assert.Throws(typeof(CountLinesException), () => _resultHelper.CalculateMediana(values));

            //than
            Assert.That(expected, Is.EqualTo(ex.Message));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetPeriodFromTimeSpan_Test(int numberCase)
        {
            //given
            var times = _testData.GetTimespanForTest(numberCase);
            var expected = _testData.GetPeriodModelForTest(numberCase);

            //when
            var actual = _resultHelper.GetPeriodFromTimeSpan(times);

            //than
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetPeriodFromTimeSpan_NegativeTest()
        {
            //given
            var timeSpan = new TimeSpan(-100, 0, 1, -20);
            var expected = "Incorrect TimeSpan value";
            //when
            Exception ex = Assert.Throws(typeof(ArgumentException), () => _resultHelper.GetPeriodFromTimeSpan(timeSpan));

            //than
            Assert.That(expected, Is.EqualTo(ex.Message));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void CalculateResult_Test(int numberCase)
        {
            //given
            var values = _testData.GetValueModelsForTest(numberCase);
            var expected = _testData.GetExpectedResult(numberCase);

            //when
            var actual = _resultHelper.CalculateResult(values);
            actual.AverageParameters = Math.Round(actual.AverageParameters, 2);
            //than
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void CalculateResult_NegativeTest()
        {
            //given
            var values = new List<ValueModel>();
            var expected = "The file must contain at least one line";
            //when
            Exception ex = Assert.Throws(typeof(CountLinesException), () => _resultHelper.CalculateResult(values));

            //than
            Assert.That(expected, Is.EqualTo(ex.Message));
        }
    }
}
