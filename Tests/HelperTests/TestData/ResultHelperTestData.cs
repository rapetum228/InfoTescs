using InfoTecs.BLL.Models;

namespace InfoTecs.BLL.Tests.HelperTests.TestData
{
    public class ResultHelperTestData
    {
        public List<double> GetDoublesForTest(int numberCase)
        {
            return numberCase switch
            {
                1 => new List<double>
                {
                    16,17,18
                },
                2 => new List<double>
                {
                    16,17,18,19
                },
                3 => new List<double>
                {
                    16.7, 17, 18,19,16,17,18.7,119,
                },
                4 => new List<double>
                {
                    1,1,1,1,1,1,1,1.7,2
                },
                5 => new List<double>
                {
                    1,1,1,1.7,1.7,1,1,1
                },
                6 => new List<double>
                {
                    5, 6
                },
                _ => new List<double>()
                {
                    16
                },


            };
        }

        public double GetExpectedMediana(int numberCase)
        {
            return numberCase switch
            {
                1 => 17,
                2 => 17.5,
                3 => 17.5,
                4 => 1,
                5 => 1,
                6 => 5.5,
                _ => 16
            };
        }

        public TimeSpan GetTimespanForTest(int numberCase)
        {
            return numberCase switch
            {
                1 => new TimeSpan(166, 13, 12, 11),
                2 => new TimeSpan(0, 136, 0, 0),
                _ => new TimeSpan(0, 0, 0, 0)
            };
        }

        public PeriodModel GetPeriodModelForTest(int numberCase)
        {
            return numberCase switch
            {
                1 => new PeriodModel
                {
                    Days = 166,
                    Hours = 13,
                    Minutes = 12,
                    Seconds = 11
                },
                2 => new PeriodModel
                {
                    Days = 5,
                    Hours = 16,
                    Minutes = 0,
                    Seconds = 0
                },
                _ => new PeriodModel
                {
                    Days = 0,
                    Hours = 0,
                    Minutes = 0,
                    Seconds = 0
                }
            };
        }

        public List<ValueModel> GetValueModelsForTest(int numberCase)
        {
            return numberCase switch
            {
                1 => new List<ValueModel>
                {
                    new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 11.7,
                        DiscretTime = 10
                    },new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 12.7,
                        DiscretTime = 10
                    },new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 16.7,
                        DiscretTime = 0
                    },new ValueModel
                    {
                        DateTime = new(2005, 10, 10, 1, 2, 3),
                        Parameter = 12.7,
                        DiscretTime = 14
                    },
                },
                2 => new List<ValueModel>
                {
                    new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 11.6,
                        DiscretTime = 10
                    },new ValueModel
                    {
                        DateTime = new(2000, 10, 10, 1, 2, 3),
                        Parameter = 18.9,
                        DiscretTime = 10
                    },new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 16.7,
                        DiscretTime = 0
                    },new ValueModel
                    {
                        DateTime = new(2001, 10, 10, 10, 20, 30),
                        Parameter = 12.7,
                        DiscretTime = 14
                    },new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 14.7,
                        DiscretTime = 14
                    },
                },
                3 => new List<ValueModel>
                {
                    new ValueModel{ DateTime = DateTime.Today, DiscretTime = 0, Parameter = 0},
                    new ValueModel{ DateTime = DateTime.Today, DiscretTime = 0, Parameter = 0}
                },
                _ => new List<ValueModel>
                {
                    new ValueModel
                    {
                        DateTime = new(2002, 10, 10, 1, 2, 3),
                        Parameter = 11.7,
                        DiscretTime = 10
                    }
                },
            };
        }

        public ResultModel GetExpectedResult(int numberCase)
        {
            return numberCase switch
            {
                1 => new ResultModel
                {
                    MinimalParameter = 11.7,
                    MaximalParameter = 16.7,
                    AverageDiscretTime = 8.5,
                    AverageParameters = 13.45,
                    CountLines = 4,
                    DateTimePeriod = new PeriodModel { Days = 1096, Hours = 0, Minutes = 0, Seconds = 0 },
                    MedianaByParameters = 12.7,
                },
                2 => new ResultModel
                {
                    MinimalParameter = 11.6,
                    MaximalParameter = 18.9,
                    AverageDiscretTime = 9.6,
                    AverageParameters = 14.92,
                    CountLines = 5,
                    DateTimePeriod = new PeriodModel { Days = 730, Hours = 0, Minutes = 0, Seconds = 0 },
                    MedianaByParameters = 14.7,
                },
                3 => new ResultModel
                {
                    MinimalParameter = 0,
                    MaximalParameter = 0,
                    AverageDiscretTime = 0,
                    AverageParameters = 0,
                    CountLines = 2,
                    DateTimePeriod = new PeriodModel { Days = 0, Hours = 0, Minutes = 0, Seconds = 0 },
                    MedianaByParameters = 0,
                },
                _ => new ResultModel
                {
                    MinimalParameter = 11.7,
                    MaximalParameter = 11.7,
                    AverageDiscretTime = 10,
                    AverageParameters = 11.7,
                    CountLines = 1,
                    DateTimePeriod = new PeriodModel { Days = 0, Hours = 0, Minutes = 0, Seconds = 0 },
                    MedianaByParameters = 11.7,
                }
            };
        }
    }
}
