namespace Day2
{
    internal class Day2
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Input(int GameIndex, List<Draw> Draws);

        private record struct Draw(int Red, int Green, int Blue);

        static void Main(string[] args)
        {
            List<Input> inputs = ReadInput(inputFilepath);

            PartOne(inputs);
            PartTwo(inputs);
        }

        private static List<Input> ReadInput(string inputFilepath)
        {
            List<Input> inputs = new List<Input>();
            using (StreamReader streamReader = new StreamReader(inputFilepath))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] inputComponents = line.Split(": ");

                    int gameIndex = int.Parse(inputComponents[0].Split()[1]);

                    string[] drawsRaw = inputComponents[1].Split("; ");

                    List<Draw> draws = new List<Draw>();
                    for (int i = 0; i < drawsRaw.Length; i++)
                    {
                        string[] colourCounts = drawsRaw[i].Split(", ");

                        int red = 0;
                        int green = 0;
                        int blue = 0;

                        for (int j = 0; j < colourCounts.Length; j++)
                        {
                            string[] colourCountComponents = colourCounts[j].Split();
                            
                            switch (colourCountComponents[1])
                            {
                                case "red":
                                    red = int.Parse(colourCountComponents[0]);
                                    break;
                                case "green":
                                    green = int.Parse(colourCountComponents[0]);
                                    break;
                                case "blue":
                                    blue = int.Parse(colourCountComponents[0]);
                                    break;

                                default:
                                    throw new Exception($"Unrecognised colour: {colourCountComponents[1]}");
                            }
                        }

                        draws.Add(new Draw(red, green, blue));
                    }

                    inputs.Add(new Input(gameIndex, draws));
                }
            }

            return inputs;
        }

        private static void PartOne(List<Input> inputs)
        {
            Draw maxDraw = new Draw(12, 13, 14);

            List<int> validGameIndices = new List<int>();
            foreach (Input input in inputs)
            {
                bool isGameValid = true;
                foreach (Draw draw in input.Draws)
                {
                    isGameValid &= IsDrawValid(draw, maxDraw);
                }

                if (isGameValid)
                {
                    validGameIndices.Add(input.GameIndex);
                }
            }

            int totalValidGameIndices = validGameIndices.Sum();

            Console.WriteLine($"Part one: Total valid game indices = {totalValidGameIndices}");
        }

        private static bool IsDrawValid(Draw draw, Draw maxDraw)
        {
            return draw.Red <= maxDraw.Red && draw.Green <= maxDraw.Green && draw.Blue <= maxDraw.Blue;
        }

        private static void PartTwo(List<Input> inputs)
        {

        }
    }
}