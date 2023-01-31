using Api.Exceptions;
using Api.Helpers;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.HelperTest.TestData
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

                default:
                    return new ValueModel
                    {
                        DateTime = new DateTime(2000, 1, 1, 0, 0, 0),
                        DiscretTime = 228,
                        Parameter = 228.228
                    };
            }
        }

        public string GetException(int number)
        {
            return number switch
            {
                1 => "Incorrect data in line number 228",
                _ => "Data in line 228 does not belong to the range of valid values",
            };
        }

        public string[] GetReadLinesForTest(string fakePath)
        {
            return fakePath switch
            {
                "data.csv" => new string[] { "2002-03-04_13-08-10;16;42,44", "2000-01-01_00-00-00;228;228,228" },
                _ => new string[] {""}
            };
        }
    }
}
