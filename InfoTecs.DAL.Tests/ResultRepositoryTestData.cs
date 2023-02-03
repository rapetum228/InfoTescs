using InfoTecs.DAL.Additions;
using InfoTecs.DAL.Entities;

namespace InfoTecs.DAL.Tests
{
    public class ResultRepositoryTestData
    {
        public Result GetResultForTest()
        {
            return new Result
            {
                FileName = "file",
                AverageDiscretTime = 13.5,
                DateTimePeriod = new Period { Days = 18, Hours = 0, Minutes = 9, Seconds = 10 },
                StartDateTime = DateTime.Now,
                AverageParameters = 14.6,
                CountLines = 5,
                MaximalParameter = 20,
                MedianaByParameters = 12,
                MinimalParameter = 1,
                Values = new List<Value>
                {
                    new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                    new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                    new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                }
            };
        }

        public List<Result> GetListResultsForFillDb()
        {
            return new List<Result>
                    {
                        new Result
                        {
                            FileName = "file",
                            AverageDiscretTime = 13.5,
                            DateTimePeriod = new Period { Days = 18, Hours = 0, Minutes = 9, Seconds = 10 },
                            StartDateTime = DateTime.Now,
                            AverageParameters = 4.6,
                            CountLines = 5,
                            MaximalParameter = 20,
                            MedianaByParameters = 12,
                            MinimalParameter = 1,
                            Values =new List<Value>
                            {
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                            }
                        },
                        new Result
                        {
                            FileName = "file1",
                            AverageDiscretTime = 13.5,
                            DateTimePeriod = new Period { Days = 18, Hours = 0, Minutes = 9, Seconds = 10 },
                            StartDateTime = DateTime.Now,
                            AverageParameters = 14.6,
                            CountLines = 5,
                            MaximalParameter = 20,
                            MedianaByParameters = 12,
                            MinimalParameter = 1,
                            Values =new List<Value>
                            {
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                            }
                        },
                        new Result
                        {
                            FileName = "data",
                            AverageDiscretTime = 113.5,
                            DateTimePeriod = new Period { Days = 18, Hours = 0, Minutes = 9, Seconds = 10 },
                            StartDateTime = DateTime.Now,
                            AverageParameters = 114.6,
                            CountLines = 5,
                            MaximalParameter = 20,
                            MedianaByParameters = 12,
                            MinimalParameter = 1,
                            Values =new List<Value>
                            {
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                                new Value { DateTime = DateTime.Now, DiscretTime = 10, Parameter = 1},
                            }
                        }

            };
        }

        public ResultRequest GetRequest(int numberCase)
        {
            return numberCase switch
            {
                1 => new ResultRequest
                {
                    FileName = "il",
                    EndAverageParameter = 15,
                    StartPeriod = DateTime.Today
                },
                2 => new ResultRequest
                {
                    FileName = "il",
                    EndAverageParameter = 15,
                    StartPeriod = DateTime.Today,
                    StartAverageTime = 10,
                    EndAverageTime = 100,
                    EndPeriod = DateTime.Today.AddDays(10),
                    StartAverageParameter = 0
                },
                3 => new ResultRequest
                {
                    StartAverageTime = 10,
                    EndAverageTime = 100,
                    EndPeriod = DateTime.Today.AddDays(10),
                    StartAverageParameter = 0
                },
                _ => new ResultRequest
                {

                }
            };
        }
    }
}
