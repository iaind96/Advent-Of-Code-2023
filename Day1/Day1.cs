using System.Text.RegularExpressions;

namespace Day1
{
    internal class Day1
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepathExample2 = "input_example2.txt";
        private static readonly string inputFilepath = "input.txt";

        private static readonly Dictionary<string, string> digitMapping = new Dictionary<string, string>
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" },

        };

        static void Main(string[] args)
        {
            List<string> inputs = ReadInput(inputFilepath);

            PartOne(inputs);
            PartTwo(inputs);
        }

        private static List<string> ReadInput(string inputFilepath)
        {
            List<string> inputs = new List<string>();
            using (StreamReader streamReader = new StreamReader(inputFilepath))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    inputs.Add(line);
                }
            }

            return inputs;
        }

        private static void PartOneOld(List<string> inputs)
        {
            int calibrationValueSum = 0;

            foreach(string input in inputs)
            {
                char[] calibrationValues = new char[2];

                foreach(char inputChar in input)
                {
                    if (int.TryParse(inputChar.ToString(), out _))
                    {
                        calibrationValues[0] = inputChar;
                        break;
                    }
                }

                foreach (char inputChar in input.Reverse())
                {
                    if (int.TryParse(inputChar.ToString(), out _))
                    {
                        calibrationValues[1] = inputChar;
                        break;
                    }
                }

                string calibrationValueAsString = new string(calibrationValues);

                int.TryParse(calibrationValueAsString, out int calibrationValueAsInt);

                calibrationValueSum += calibrationValueAsInt;
            }

            Console.WriteLine($"Part One: Calibration value sum = {calibrationValueSum}");
        }

        private static void PartOne(List<string> inputs)
        {
            int calibrationValueSum = 0;

            string singleDigitPattern = @"\d";

            foreach (string input in inputs)
            {
                MatchCollection matches = Regex.Matches(input, singleDigitPattern);

                if (matches.Count > 0)
                {
                    string calibrationValueAsString = matches[0].Value + matches[matches.Count - 1].Value;

                    calibrationValueSum += int.Parse(calibrationValueAsString);
                }
            }

            Console.WriteLine($"Part One: Calibration value sum = {calibrationValueSum}");
        }

        private static void PartTwo(List<string> inputs)
        {
            int calibrationValueSum = 0;

            string writtenDigitPattern = string.Join('|', digitMapping.Keys);
            string singleDigitPattern = @"\d";
            string digitPattern = singleDigitPattern + "|" + writtenDigitPattern;

            foreach (string input in inputs)
            {
                MatchCollection matches = Regex.Matches(input, $"(?=({digitPattern}))");

                if (matches.Count > 0)
                {

                    string calibrationValueAsString = ParseMatchToSingleDigit(matches[0].Groups[1].Value) + ParseMatchToSingleDigit(matches[matches.Count - 1].Groups[1].Value);

                    calibrationValueSum += int.Parse(calibrationValueAsString);
                }
            }

            Console.WriteLine($"Part Two: Calibration value sum = {calibrationValueSum}");
        }

        private static string ParseMatchToSingleDigit(string match)
        {
            if (digitMapping.TryGetValue(match, out string singleDigit)) 
            {
                return singleDigit;
            }

            return match;
        }
    }
}