using InfoTecs.Api.Services;
using System.Text;

namespace InfoTecs.Api.Tests
{
    public class FileProcessingerTests
    {
        private readonly IFileProcessinger _fileProcessinger;
        private readonly ApiTestData _testData;
        public FileProcessingerTests()
        {
            _fileProcessinger = new FileProcessinger();
            _testData = new ApiTestData();
        }

        [Test]
        public async Task WriteBytesValuesInJson_Test()
        {
            //given
            var values = await _testData.GetValueModels(0);

            //when
            var actual = _fileProcessinger.WriteBytesValuesInJson(values!);

            //than
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual);
            Assert.IsInstanceOf(typeof(byte[]), actual);
        }

        [Test]
        public async Task WriteBytesValuesInJson_NegativeTest()
        {
            //given
            var values = await _testData.GetValueModels(1);
            var expectedMessage = "Received an empty list of values";
            //when
            Exception ex = Assert.Throws(typeof(ArgumentException), () => _fileProcessinger.WriteBytesValuesInJson(values!));

            //than
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetJsonFileFromBytes_Test()
        {
            //given
            var fileName = "file";
            var str = "String for test";
            var bytes = Encoding.UTF8.GetBytes(str);

            //when
            var actual = _fileProcessinger.GetJsonFileFromBytes(bytes, fileName);

            //than
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ContentType == "txt/json");
            Assert.IsTrue(actual.FileDownloadName == fileName + ".json");

        }

        [Test]
        public void GetJsonFileFromBytes_NegativeTestByFileName()
        {
            //given
            var fileName = " ";
            var str = "String for test";
            var bytes = Encoding.UTF8.GetBytes(str);
            var expectedMessage = "File name must not be empty";

            //when
            Exception ex = Assert.Throws(typeof(ArgumentException), () => _fileProcessinger.GetJsonFileFromBytes(bytes, fileName));

            //than
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetJsonFileFromBytes_NegativeTestByEmptyBytesBuffer()
        {
            //given
            var fileName = "file";
            var bytes = new byte[] { };
            var expectedMessage = "Received an empty list of bytes";

            //when
            Exception ex = Assert.Throws(typeof(ArgumentException), () => _fileProcessinger.GetJsonFileFromBytes(bytes, fileName));

            //than
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}
