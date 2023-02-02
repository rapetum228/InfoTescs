using InfoTecs.BLL.Exceptions;
using InfoTecs.BLL.Helpers;
using InfoTecs.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTecs.BLL.Tests.HelperTests.TestData
{
    public class ValueHelperTestData
    {
        public ValueModel GetValueModelsForTest(int number)
        {
            switch (number)
            {
                case 1:
                    return new ValueModel
                    {
                        DateTime = new DateTime(2002, 3, 4, 13, 8, 10),
                        DiscretTime = 16,
                        Parameter = 42.44
                    };
                case 2:
                    return new ValueModel
                    {
                        DateTime = new DateTime(2000, 1, 1, 0, 0, 0),
                        DiscretTime = 0,
                        Parameter = 0
                    };

                default:
                    return new ValueModel
                    {
                        DateTime = new DateTime(2000, 1, 1, 0, 0, 0),
                        DiscretTime = 228,
                        Parameter = 228
                    };
            }
        }

        public ICollection<string?> GetReadLinesForTest(int number)
        {
            return number switch
            {
                1 => new string?[] { "2002-03-04_13-08-10;16;42,44", "2000-01-01_00-00-00;228;228,228" },
                _ => FillCillectionForTest<string?>("2012-03-04_13-08-10;16;42,44", 3000)
            };
        }

        public ICollection<string?> GetReadLinesForNegativeTest(int number)
        {
            return number switch
            {
                1 => new string?[] { "2002-03-04_13-08-10;1642,44", "2000-01-01_00-00-00;228;228,228" },
                2 => new string?[] { },
                3 => new string?[]
                {
                    "2002-03-04_13-08-10;16;4244",
                    "2002-03-04_13-08-10;16;42,44",
                    "2000-01-01_00-00-00;0;0",
                    "2000-01-01_00-00-00;-1;0"
                },
                _ => FillCillectionForTest<string?>("2012-03-04_13-08-10;16;42,44", 10001)
            };
        }

        public Type GetTypeExceptionForNegativeTest(int number)
        {
            return number switch
            {
                1 => typeof(InvalidLineException),
                2 => typeof(ValueIsNotInRangeException),
                _ => typeof(CountLinesException),
            };
        }

        public string GetExceptionMessage(int numberCase, int numberLine)
        {
            return numberCase switch
            {
                1 => $"Incorrect data in line number {numberLine}",
                2 => $"Data in line {numberLine} does not belong to the range of valid values",
                3 => "The file must contain no more than 10000 lines",
                _ => "The file must contain at least one line",
            };
        }

        public ICollection<ValueModel> GetValueModelsForExpectedResult(int number)
        {
            return number switch
            {
                1 => new List<ValueModel>
                {
                    new ValueModel
                    {
                        DateTime = new DateTime(2002,3,4,13,8,10),
                        DiscretTime = 16,
                        Parameter = 42.44
                    },
                    new ValueModel
                    {
                        DateTime = new DateTime(2000,1,1,0,0,0),
                        DiscretTime = 228,
                        Parameter = 228.228
                    }

                },
                _ => FillCillectionForTest<ValueModel>(
                    new ValueModel
                    {
                        DateTime = new DateTime(2012, 3, 4, 13, 8, 10),
                        DiscretTime = 16,
                        Parameter = 42.44
                    }, 3000)
            };
        }
        private ICollection<T> FillCillectionForTest<T>(T value, int count)
        {
            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                list.Add(value);
            }
            return list;
        }
    }
}
