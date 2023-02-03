using InfoTecs.BLL.Models;
using InfoTecs.DAL.Entities;

namespace InfoTecs.BLL.Tests.ServiceTests;

public class ResultServiceTestData
{
    public async Task<List<Result>?> GetResultsForTest(int numberCase = 0)
    {
        return numberCase switch
        {
            1 => new List<Result>
            {

            },
            2 => null,
            _ => new List<Result>
                {
                    new Result
                    {
                        AverageDiscretTime = 14,
                        AverageParameters = 3.14,
                        DateTimePeriod = new Period { Days = 19, Hours = 4, Minutes = 8, Seconds = 9 },
                        CountLines = 2,
                        MaximalParameter = 43,
                        MedianaByParameters = 10,
                        MinimalParameter = 0,
                        Values = new List<Value>()
                                {
                                     new Value{ DateTime = DateTime.Now, DiscretTime = 18, Parameter = 22.8 },
                                     new Value{ DateTime = DateTime.Now, DiscretTime = 1, Parameter = 0.1 }
                                }
                    }
                }
        };
    }

    public ResultModel GetResultModelForTest(int numberCase)
    {
        return numberCase switch
        {
            4 => new ResultModel(),
            _ => new ResultModel
            {
                AverageDiscretTime = 14,
                AverageParameters = 3.14,
                DateTimePeriod = new PeriodModel { Days = 19, Hours = 4, Minutes = 8, Seconds = 9 },
                CountLines = 2,
                MaximalParameter = 43,
                MedianaByParameters = 10,
                MinimalParameter = 0,
                Values = new List<ValueModel>()
                        {
                             new ValueModel{ DateTime = DateTime.Now, DiscretTime = 18, Parameter = 22.8 },
                             new ValueModel{ DateTime = DateTime.Now, DiscretTime = 1, Parameter = 0.1 }
                        }
            }
        };
    }

    public MetaModel GetMetaForTest(int numberCase)
    {
        return numberCase switch
        {
            1 => new MetaModel
            {
                FileName = "data",
                StartDateTime = DateTime.Now,
                Data = new List<string?>
                {
                }

            },
            2 => new MetaModel
            {
                FileName = " ",
                StartDateTime = DateTime.Now,
                Data = new List<string?>
                {
                }

            },
            _ => new MetaModel
            {
                FileName = "data",
                StartDateTime = DateTime.Now,
                Data = new List<string?>
                {
                    "2023-02-03_09-09-09;18;19,21",
                    "2023-02-03_09-09-09;18;19,21"
                }

            }
        };
    }

    public List<ValueModel> GetValueModelsForTest(int numberCase)
    {
        return numberCase switch
        {
            3 => new List<ValueModel>(),
            _ => new List<ValueModel>
                {
                    new ValueModel{ DateTime = DateTime.Now, DiscretTime = 18, Parameter = 22.8 },
                    new ValueModel{ DateTime = DateTime.Now, DiscretTime = 1, Parameter = 0.1 }
                }
        };
    }

    public async Task<ICollection<Value>?> GetTaskValuesForTest(int numberCase)
    {
        return numberCase switch
        {
            1 => new List<Value>(),
            2 => null,
            _ => new List<Value>
                {
                    new Value { DateTime = DateTime.Now, DiscretTime = 18, Parameter = 22.8 },
                    new Value { DateTime = DateTime.Now, DiscretTime = 1, Parameter = 0.1 }
                }
        };
    }
}