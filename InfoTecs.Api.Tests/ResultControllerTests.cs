using AutoMapper;
using InfoTecs.Api.Controllers;
using InfoTecs.Api.Services;
using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Mappers;
using InfoTecs.BLL.Models;
using InfoTecs.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InfoTecs.Api.Tests;

public class ResultControllerTests
{
    private Mock<IResultService> _resultService;
    private Mock<IFileProcessinger> _fileProcessingService;
    private readonly IMapper _mapper;
    private readonly ApiTestData _testData;

    public ResultControllerTests()
    {
        _mapper = new Mapper(
            new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>()));
        _testData = new ApiTestData();
    }

    [SetUp]
    public void Setup()
    {
        _fileProcessingService = new Mock<IFileProcessinger>();
        _resultService = new Mock<IResultService>();
    }

    [Test]
    public void UploadCsvFileAndGetResult_ShouldReturnResultOutputModel()
    {
        //given
        var file = new Mock<IFormFile>();
        var meta = _testData.GetMeta();
        var resultModel = _testData.GetResultModel();
        _fileProcessingService.Setup(x => x.CheckFileAndGetMetaAsync(file.Object)).Returns(meta);
        _resultService.Setup(x => x.ProcessingDataToResult(meta.Result)).Returns(resultModel);
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when
        var expected = sut.UploadCsvFileAndGetResult(file.Object);

        //than
        _fileProcessingService.Verify(x => x.CheckFileAndGetMetaAsync(file.Object), Times.Once());
        _resultService.Verify(x => x.ProcessingDataToResult(meta.Result), Times.Once());
        Assert.IsNotNull(expected);
        Assert.IsInstanceOf(typeof(ResultOutputModel), expected.Result);
    }

    [TestCase]
    public void UploadCsvFileAndGetResult_ShouldThrowsArgumentException()
    {
        //given
        var file = new Mock<IFormFile>();
        var meta = _testData.GetMeta();
        var resultModel = _testData.GetResultModel();

        _fileProcessingService.Setup(x => x.CheckFileAndGetMetaAsync(file.Object)).Throws(new ArgumentException());
        _resultService.Setup(x => x.ProcessingDataToResult(meta.Result)).Returns(resultModel);
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when


        //than
        Assert.ThrowsAsync<ArgumentException>(async () => await sut.UploadCsvFileAndGetResult(file.Object));
        _fileProcessingService.Verify(x => x.CheckFileAndGetMetaAsync(file.Object), Times.Once());
        _resultService.Verify(x => x.ProcessingDataToResult(meta.Result), Times.Never());

    }

    [TestCase]
    public void UploadCsvFileAndGetResult_ShouldThrowsArgumentNullException()
    {
        //given
        var file = new Mock<IFormFile>();
        var meta = _testData.GetMeta();
        var resultModel = _testData.GetResultModel();

        _fileProcessingService.Setup(x => x.CheckFileAndGetMetaAsync(file.Object)).Returns(meta);
        _resultService.Setup(x => x.ProcessingDataToResult(meta.Result)).Throws(new ArgumentNullException());
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when

        //than
        Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UploadCsvFileAndGetResult(file.Object));
        _fileProcessingService.Verify(x => x.CheckFileAndGetMetaAsync(file.Object), Times.Once());
        _resultService.Verify(x => x.ProcessingDataToResult(meta.Result), Times.Once());

    }

    [Test]
    public void GetResults_ShouldReturnListResultOutputModels()
    {
        //given
        var requestModel = _testData.GetResultRequest();
        var results = _testData.GetResultModels();

        _resultService.Setup(x => x.GetResultsByRequestAsync(requestModel.Result)).Returns(results);
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when
        var expected = sut.GetResults(requestModel.Result);

        //than
        _resultService.Verify(x => x.GetResultsByRequestAsync(requestModel.Result), Times.Once());
    }

    [Test]
    public void GetResults_ShouldThrowsNotFoundException()
    {
        //given
        _resultService.Setup(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequestModel>())).Throws(new NotFoundException());
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when

        //than
        Assert.ThrowsAsync<NotFoundException>(async () => await sut.GetResults(It.IsAny<ResultRequestModel>()));
        _resultService.Verify(x => x.GetResultsByRequestAsync(It.IsAny<ResultRequestModel>()), Times.Once());
    }

    [Test]
    public void GetValuesByName_ShouldReturnListValueModels()
    {
        //given
        var fileName = "file";
        var values = _testData.GetValueModels(0);

        _resultService.Setup(x => x.GetValuesByFileNameAsync(fileName)).Returns(values!);
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when
        var expected = sut.GetValuesByName(fileName);

        //than
        _resultService.Verify(x => x.GetValuesByFileNameAsync(fileName), Times.Once());
    }

    [Test]
    public void GetValuesByName_ThrowsArgumentException()
    {
        //given
        var fileName = " ";

        _resultService.Setup(x => x.GetValuesByFileNameAsync(fileName)).Throws(new ArgumentException("Filename cannot be empty"));
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when

        //than
        Assert.ThrowsAsync<ArgumentException>(async () => await sut.GetValuesByName(fileName));
        _resultService.Verify(x => x.GetValuesByFileNameAsync(fileName), Times.Once());
    }

    [Test]
    public void GetValuesByName_ThrowsNotFoundException()
    {
        //given
        var fileName = "file";

        _resultService.Setup(x => x.GetValuesByFileNameAsync(fileName)).Throws(new NotFoundException());
        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when

        //than
        Assert.ThrowsAsync<NotFoundException>(async () => await sut.GetValuesByName(fileName));
        _resultService.Verify(x => x.GetValuesByFileNameAsync(fileName), Times.Once());
    }

    [Test]
    public void DownloadValuesByName_ShouldReturnFileResult()
    {
        //given
        var fileName = "file";
        var values = _testData.GetValueModels(0);
        var bytes = new byte[] { };
        var file = new Mock<FileContentResult>();

        _resultService.Setup(x => x.GetValuesByFileNameAsync(fileName)).Returns(values!);
        _fileProcessingService.Setup(x => x.WriteBytesValuesInJson(values.Result!));
        _fileProcessingService.Setup(x => x.GetJsonFileFromBytes(bytes, fileName)).Returns(new FileContentResult(bytes, "text/json"));

        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when
        var expected = sut.DownloadValuesByName(fileName);

        //than
        _resultService.Verify(x => x.GetValuesByFileNameAsync(fileName), Times.Once());
        _fileProcessingService.Verify(x => x.WriteBytesValuesInJson(values.Result!), Times.Once());
        _fileProcessingService.Verify(x => x.GetJsonFileFromBytes(bytes, fileName), Times.Once());
        Assert.IsNotNull(expected);
        Assert.IsInstanceOf(typeof(FileResult), expected.Result);
    }

    [Test]
    public void DownloadValuesByName_ThrowsProcessFailedExceptionException()
    {
        //given
        var fileName = "file";
        var values = _testData.GetValueModels(1);
        var bytes = new byte[] { };
        var file = new Mock<FileContentResult>();

        _resultService.Setup(x => x.GetValuesByFileNameAsync(fileName)).Returns(values!);
        _fileProcessingService.Setup(x => x.WriteBytesValuesInJson(values.Result!)).Throws(new ProcessFailedException("Received an empty list of values"));
        _fileProcessingService.Setup(x => x.GetJsonFileFromBytes(bytes, fileName)).Returns(It.IsAny<FileContentResult>);

        var sut = new ResultController(_resultService.Object, _fileProcessingService.Object, _mapper);

        //when

        //than
        Assert.ThrowsAsync<ProcessFailedException>(async () => await sut.DownloadValuesByName(fileName));
        _resultService.Verify(x => x.GetValuesByFileNameAsync(fileName), Times.Once());
        _fileProcessingService.Verify(x => x.WriteBytesValuesInJson(values.Result!), Times.Once());
        _fileProcessingService.Verify(x => x.GetJsonFileFromBytes(bytes, fileName), Times.Never());
    }

}