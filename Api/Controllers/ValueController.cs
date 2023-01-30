using Api.Models;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly ValueService _valueService;
        private readonly IMapper _mapper;
        public ValueController(ValueService valueService, IMapper mapper)
        {
            _valueService = valueService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ResultOutputModel> UploadFile(IFormFile file)
        {
            MetaModel resultShortModel = new()
            {
                StartDateTime = DateTime.Now,
                FileName = file.FileName,
            };

            if (file.ContentType != "text/csv")
            {
                throw new FormatException("Only csv format file");
            }

            resultShortModel.CurrentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var fileinfo = new FileInfo(resultShortModel.CurrentPath);

            using (var stream = System.IO.File.Create(resultShortModel.CurrentPath))
            {
                await file.CopyToAsync(stream);
            }

            var result = await _valueService.FileProcessAsync(resultShortModel);
            return result;
        }

        [HttpPost]
        public async Task<List<ResultOutputModel>> GetResults([FromForm] ResultRequestModel requestModel)
        {
            var results = await _valueService.GetResultsByRequest(requestModel);

            return _mapper.Map<List<ResultOutputModel>>(results);
        }

        [HttpGet]
        public async Task<List<ValueModel>> GetValuesByName(string fileName)
        {
            var values = await _valueService.GetValuesByFileName(fileName);
            return values;
        }

        [HttpGet]
        public async Task<FileResult> DownloadValues(string fileName)
        {
            var values = await _valueService.GetValuesByFileName(fileName);

            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            using (FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<List<ValueModel>>(fs, values);
            }

            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(tempPath), "txt/json")
            {
                FileDownloadName = fileName
            };

            return result;
        }
    }
}
