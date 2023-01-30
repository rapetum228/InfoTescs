using Api.Exceptions;
using Api.Models;
using System.Globalization;

namespace Api.Helpers
{
    public class CsvHelper
    {
        private static readonly DateTime _minDate = new DateTime(2000, 1, 1);
        private const int _minCountOfLines = 1;
        private const int _maxCountOfLines = 10000;

        public CsvHelper()
        {

        }

        public List<ValueModel> ReadValuesFromLines(string path)
        {
            int numberLine = 0;
            var lines = GetLinesFromCsv(path);
            //проверки здесь для тестинга
            if (lines.Length < _minCountOfLines)
            {
                throw new CountLinesException("Строк должно бытьне меньше одной");
            }
            if (lines.Length > _maxCountOfLines)
            {
                throw new CountLinesException("Строк должно быть не больше 10000");
            }

            var values = new List<ValueModel>();

            foreach (var line in lines)
            {
                numberLine++;

                var item = GetValueFromString(line, numberLine);
                values.Add(item);
            }
            return values;
        }

        private ValueModel GetValueFromString(string line, int numberLine)
        {
            var cells = line.Split(';');

            if (cells.Length != 3)
            {
                throw new InvalidLineException(numberLine);
            }

            int discretTime;

            if (!Int32.TryParse(cells[1], out discretTime) || discretTime < 0)
            {
                throw new InvalidLineException(numberLine);
            }

            double parameter;

            if (!Double.TryParse(cells[2], out parameter) || parameter < 0)
            {
                throw new InvalidLineException(numberLine);
            }
            var item = new ValueModel
            {
                DateTime = GetDateTimeFromStr(cells[0], numberLine),
                DiscretTime = discretTime,
                Parameter = parameter
            };

            return item;
        }

        private string[] GetLinesFromCsv(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found");
            }
            var lines = File.ReadAllLines(path); //ЧЁ С ОПЕРАТИВОЙ

            return lines;
        }

        private DateTime GetDateTimeFromStr(string dateString, int numberLine)
        {

            string format = "yyyy-MM-dd_HH-mm-ss";
            DateTime dateTime;
            if (!DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dateTime) || dateTime < _minDate)
            {
                throw new InvalidLineException(numberLine);
            }

            return dateTime;
        }
    }
}
