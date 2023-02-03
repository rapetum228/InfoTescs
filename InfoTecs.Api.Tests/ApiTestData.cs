using InfoTecs.BLL.Models;

namespace InfoTecs.Api.Tests;

public class ApiTestData
{
    public async Task<MetaModel> GetMeta()
    {

        return new MetaModel
        {
            StartDateTime = DateTime.Now,
            FileName = "file",
            Data = new List<string?>
                    {
                        "ejpojfjwp",
                        "seflskfl"
                    }

        };
    }

    public async Task<ResultModel> GetResultModel()
    {

        return new ResultModel
        {
            FileName = "file",
            AverageDiscretTime = 19,
            CountLines = 12,
            AverageParameters = 2,

        };
    }

    public async Task<ResultRequestModel> GetResultRequest()
    {
        return new ResultRequestModel
        {
            FileName = "file",
            EndAverageParameter = 100,
            StartAverageParameter = 0,
            EndAverageTime = 34

        };
    }

    public async Task<List<ResultModel>?> GetResultModels()
    {
        return new List<ResultModel>
                {
                    new ResultModel
                    {
                        FileName = "file"
                    },

                    new ResultModel
                    {
                        FileName = "file228"
                    }
                };
    }

    public async Task<List<ValueModel>?> GetValueModels(int numberCase)
    {
        return numberCase switch
        {
            1 => new List<ValueModel>
            {
            },
            _ => new List<ValueModel>
            {
                new ValueModel{ DateTime = DateTime.Now, DiscretTime = 10, Parameter = 10},
                new ValueModel{ DateTime = DateTime.Now, DiscretTime = 10, Parameter = 10}
            }
        };
    }
}
