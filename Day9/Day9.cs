namespace Day9;

class Program
{
    private static readonly string inputFilepathExample = "input_example.txt";
    private static readonly string inputFilepath = "input.txt";

    static void Main(string[] args)
    {
        List<int[]> inputs = ReadInput(inputFilepath);

        PartOne(inputs);
        PartTwo(inputs);
    }

    private static List<int[]> ReadInput(string inputFilepath)
    {
        List<string> lines = File.ReadLines(inputFilepath).ToList();

        List<int[]> inputs = new List<int[]>();
        foreach (string line in lines)
        {
            inputs.Add(line.Split().Select(int.Parse).ToArray());
        }

        return inputs;
    }

    private static int PredictSequence(int[] sequence, bool isExtrapolatingForward = true)
    {
        if (sequence.All(x => x == 0))
        {
            return 0;
        }

        int[] diffs = new int[sequence.Length - 1];
        for (int i = 0; i < sequence.Length - 1; i++)
        {
            diffs[i] = sequence[i + 1] - sequence[i];
        }

        if (isExtrapolatingForward)
        {
            return sequence[sequence.Length - 1] + PredictSequence(diffs);
        }
        else
        {
            return sequence[0] - PredictSequence(diffs, false);
        }
        
    }

    private static void PartOne(List<int[]> inputs)
    {
        int totalPrediction = 0;
        foreach (int[] sequence in inputs)
        {
            int prediction = PredictSequence(sequence);
            totalPrediction += prediction;
        }

        Console.WriteLine($"Part one: Total prediction sum = {totalPrediction}");
    }

    private static void PartTwo(List<int[]> inputs)
    {
        int totalPrediction = 0;
        foreach (int[] sequence in inputs)
        {
            int prediction = PredictSequence(sequence, false);
            totalPrediction += prediction;
        }

        Console.WriteLine($"Part two: Total prediction sum = {totalPrediction}");
    }
}

