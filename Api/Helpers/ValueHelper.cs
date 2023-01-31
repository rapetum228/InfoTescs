﻿using Api.Exceptions;
using Api.Models;
using System.Globalization;

namespace Api.Helpers
{
    public class ValueHelper
    {
        private static readonly DateTime _minDate = new (2000, 1, 1);
        private const int MinCountOfLines = 1;
        private const int MaxCountOfLines = 10000;

        public ValueHelper()
        {

        }

        public List<ValueModel> ReadValuesFromLines(string path)
        {
            int numberLine = 0;
            var lines = GetLinesFromCsv(path);
            //проверки здесь для тестинга
            if (lines.Length < MinCountOfLines)
            {
                throw new CountLinesException("The file must contain at least one line");
            }
            if (lines.Length > MaxCountOfLines)
            {
                throw new CountLinesException("The file must contain no more than 10000 lines");
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