namespace Day4
{
    internal class Day4
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Input(List<int> WinningNumbers, List<int> ChosenNumbers);

        static void Main(string[] args)
        {
            List<Input> inputs = ReadInput(inputFilepath);

            PartOne(inputs);
        }

        private static List<Input> ReadInput(string inputFilepath)
        {
            List<Input> inputs = new List<Input>();
            using (StreamReader streamReader = new StreamReader(inputFilepath))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] numbers = line.Split(": ")[1].Split(" | ");
                    List<int> winningNumbers = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                    List<int> chosenNumbers = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                    inputs.Add(new Input(winningNumbers, chosenNumbers));
                }
            }

            return inputs;
        }

        private static void PartOne(List<Input> inputs)
        {
            int totalPoints = 0;

            foreach (Input input in inputs)
            {
                int winningNumberCount = 0;
                foreach (int winningNumber in input.WinningNumbers)
                {
                    if (input.ChosenNumbers.Contains(winningNumber))
                    {
                        winningNumberCount++;
                    }
                }

                if (winningNumberCount > 0)
                {
                    double x = Math.Max(0, winningNumberCount - 1);
                    int points = (int)Math.Pow(2.0, Math.Max(0, winningNumberCount - 1));

                    totalPoints += points;
                }
            }

            Console.WriteLine($"Part one: Total points = {totalPoints}");
        }
    }
}