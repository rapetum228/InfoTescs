﻿using AutoMapper;
using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Mappers;
using InfoTecs.BLL.Models;
using InfoTecs.BLL.Services;
using InfoTecs.DAL.Additions;
using InfoTecs.DAL.Entities;
using InfoTecs.DAL.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTecs.BLL.Tests.ServiceTests
{
    public class ValueServiceTest
    {
        private Mock<IResultRepository> _resultRepository;
        private Mock<IResultHelperService> _resultHelper;
        private Mock<IValueHelperService> _valueHelper;
        private readonly IMapper _mapper;
        private readonly ValueServiceTestData _testData;

        public ValueServiceTest()
        {
            _mapper = new Mapper(
                new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>())); ;
            _testData = new ValueServiceTestData();
        }

        [SetUp]
        public void Setup()
        {
            _resultRepository = new Mock<IResultRepository>();
            _resultHelper = new Mock<IResultHelperService>();
            _valueHelper = new Mock<IValueHelperService>();
        }

        [Test]
        public void ProcessingDataToResult_ShouldReturnResultModel()
        {
            //given
            var resultModel = _testData.GetResultModelForTest(0);
            _resultHelper.Setup(x => x.CalculateResult(It.IsAny<List<ValueModel>>())).Returns(resultModel);

            var valueModels = _testData.GetValueModelsForTest(0);
            _valueHelper.Setup(x => x.ReadValuesFromLines(It.IsAny<List<string?>>())).Returns(valueModels);

            var meta = _testData.GetMetaForTest(0);

            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);
            var expected = resultModel;

            //when
            var actual = sut.ProcessingDataToResult(meta);

            //than
            _resultHelper.Verify(x => x.CalculateResult(It.IsAny<List<ValueModel>>()), Times.Once());
            _valueHelper.Verify(x => x.ReadValuesFromLines(It.IsAny<List<string?>>()), Times.Once());
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(ResultModel), actual);
            Assert.That(expected.FileName, Is.EqualTo(meta.FileName));
            Assert.That(expected.StartDateTime, Is.EqualTo(meta.StartDateTime));
            Assert.That(expected.Values, Is.EqualTo(actual.Values));
        }

        [TestCase(1, 0, 0)] //1 - meta with empty Data, correct ResultModel, correct list of ValueModel
        [TestCase(2, 0, 0)] //2 - meta without FileName, correct ResultModel, correct list of ValueModel
        [TestCase(3, 0, 1)] //3 - correct meta, correct ResultModel, empty list of ValueModel
        [TestCase(4, 1, 1)] //4 -correct meta, ResultModel with empty properties
        public void ProcessingDataToResult_ShouldThrowArgumentNullException(int numberCase,
                                    int countExactlyResultHelper, int countExactlyValueHelper)
        {
            //given
            var meta = _testData.GetMetaForTest(numberCase);
            var resultModel = _testData.GetResultModelForTest(numberCase);
            var valueModels = _testData.GetValueModelsForTest(numberCase);

            _resultHelper.Setup(x => x.CalculateResult(valueModels)).Returns(resultModel);
            _valueHelper.Setup(x => x.ReadValuesFromLines(meta.Data)).Returns(valueModels);

            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);
            //var actual = sut.ProcessingDataToResult(meta);

            //than
            Assert.Throws<ArgumentNullException>(() => sut.ProcessingDataToResult(meta));
            _valueHelper.Verify(x => x.ReadValuesFromLines(meta.Data), Times.Exactly(countExactlyValueHelper));
            _resultHelper.Verify(x => x.CalculateResult(valueModels), Times.Exactly(countExactlyResultHelper));
        }

        [Test]
        public void AddResultAsync_ShouldReturnVoid()
        {
            //given
            _resultRepository.Setup(x => x.AddResultAsync(It.IsAny<Result>()));
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when
            sut.AddResultAsync(It.IsAny<ResultModel>()).Wait();

            //than
            _resultRepository.Verify(x => x.AddResultAsync(It.IsAny<Result>()), Times.Once());
        }

        [Test]
        public void GetResultsByRequestAsync_ShouldReturnListOfResultModels()
        {
            //given
            var results = _testData.GetResultsForTest();
            _resultRepository.Setup(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequest>())).Returns(results);
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when
            var resultModels = sut.GetResultsByRequestAsync(It.IsAny<ResultRequestModel>());

            //than
            _resultRepository.Verify(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequest>()), Times.Once());
        }

        [TestCase(1)] //empty list
        [TestCase(2)] //null
        public void GetResultsByRequestAsync_ShouldReturnNotFoundException(int numberCase)
        {
            //given
            var results = _testData.GetResultsForTest(numberCase);
            _resultRepository.Setup(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequest>())).Returns(results);
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when

            //than
            Assert.ThrowsAsync<NotFoundException>(() =>
                    
             sut.GetResultsByRequestAsync(It.IsAny<ResultRequestModel>())
            
            );
            _resultRepository.Verify(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequest>()), Times.Once());
        }


        [Test]
        public async Task GetValuesByFileNameAsync_ShouldReturnListOfResultModelsAsync()
        {
            //given
            var values = _testData.GetTaskValuesForTest(0);

            _resultRepository.Setup(x => x.GetValuesByFileNameAsync("file")).Returns(values);
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when
            var valueModels = await sut.GetValuesByFileNameAsync("file");

            //than
            _resultRepository.Verify(x => x.GetValuesByFileNameAsync("file"), Times.Once());
            Assert.IsNotNull(valueModels);
            Assert.IsInstanceOf(typeof(List<ValueModel>), valueModels);
        }

        [TestCase(1)] //empty list
        [TestCase(2)] //null
        public void GetValuesByFileNameAsync_ShouldReturnNotFoundException(int numberCase)
        {
            //given
            var values = _testData.GetTaskValuesForTest(numberCase);
            _resultRepository.Setup(x => x.GetValuesByFileNameAsync("file")).Returns(values);
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when

            //than
            Assert.ThrowsAsync<NotFoundException>(() =>

             sut.GetValuesByFileNameAsync("file")

            );
            _resultRepository.Verify(x => x.GetValuesByFileNameAsync("file"), Times.Once());
        }

        [Test]
        public void GetValuesByFileNameAsync_ShouldThrowArgumentNullException()
        {
            //given
            var values = _testData.GetTaskValuesForTest(0);
            _resultRepository.Setup(x => x.GetValuesByFileNameAsync(" ")).Returns(values);
            var sut = new ValueService(_mapper, _resultHelper.Object, _valueHelper.Object, _resultRepository.Object);

            //when

            //than
            Assert.ThrowsAsync<ArgumentNullException>(() =>

             sut.GetValuesByFileNameAsync(" ")

            );
            _resultRepository.Verify(x => x.GetValuesByFileNameAsync(" "), Times.Never());
        }
    }
}
