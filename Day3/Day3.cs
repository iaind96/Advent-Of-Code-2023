﻿using System.Data;

namespace Day3
{
    internal class Day3
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Input(List<int> WinningNumbers, List<int> ChosenNumbers);

        static void Main(string[] args)
        {
            List<string> inputs = ReadInput(inputFilepath);

            PartOne(inputs);
            PartTwo(inputs);
        }

        private static List<string> ReadInput(string inputFilepath)
        {
            List<string> input = new List<string>();
            using (StreamReader streamReader = new StreamReader(inputFilepath))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                     input.Add(line);
                }
            }

            return input;
        }

        private static bool IsDigitSurroundedBySymbol(int rowIndex, int columnIndex, List<string> input)
        {
            bool isDigitSurroundedBySymbol = false;

            int rowStart = Math.Max(0, rowIndex - 1);
            int rowEnd = Math.Min(input.Count - 1, rowIndex + 1);
            int columnStart = Math.Max(0, columnIndex - 1);
            int columnEnd = Math.Min(input[0].Length - 1, columnIndex + 1);

            for (int i = rowStart; i <= rowEnd; i++)
            {
                for (int j = columnStart; j <= columnEnd; j++)
                {
                    isDigitSurroundedBySymbol |= (!char.IsDigit(input[i][j]) && input[i][j] != '.');
                }
            }

            return isDigitSurroundedBySymbol;
        }

        private static void PartOne(List<string> input)
        {
            List<int> partNumbers = new List<int>();

            for (int rowIndex = 0; rowIndex < input.Count; rowIndex++)
            {
                string row = input[rowIndex];

                List<char> digits = new List<char>();
                bool isNumberPartNumber = false;

                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    if (char.IsDigit(row[columnIndex]))
                    {
                        digits.Add(row[columnIndex]);

                        // check is number is valid
                        isNumberPartNumber |= IsDigitSurroundedBySymbol(rowIndex, columnIndex, input);
                    }
                    else
                    {
                        if (isNumberPartNumber)
                        {
                            // parse number and add to total
                            int partNumber = int.Parse(new string(digits.ToArray()));
                            partNumbers.Add(partNumber);
                        }

                        // reset search for this line
                        digits.Clear();
                        isNumberPartNumber = false;
                    }
                }

                if (isNumberPartNumber)
                {
                    // parse number and add to total
                    int partNumber = int.Parse(new string(digits.ToArray()));
                    partNumbers.Add(partNumber);
                }
            }

            int partNumbersSum = partNumbers.Sum();

            Console.WriteLine($"Part one: Part number sum = {partNumbersSum}");
        }

        private static void PartTwo(List<string> input)
        {

        }
    }
}