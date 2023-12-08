namespace Day8
{
    internal class Day8
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepathExample2 = "input_example2.txt";
        private static readonly string inputFilepathExample3 = "input_example3.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Input(string Directions, Dictionary<string, Node> Network);

        private record struct Node(string Left, string Right);

        static void Main(string[] args)
        {
            Input input = ReadInput(inputFilepath);

            //PartOne(input);
            //PartTwoNaive(input);
            PartTwo(input);
        }

        private static Input ReadInput(string inputFilepath)
        {
            List<string> lines = File.ReadLines(inputFilepath).ToList();

            string directions = lines[0];

            Dictionary<string, Node> network = new Dictionary<string, Node>();
            for (int i = 2; i < lines.Count; i++)
            {
                string[] nodeComponents = lines[i].Split(" = ");

                string[] children = nodeComponents[1].Split(", ");

                network[nodeComponents[0]] = new Node(children[0].Replace("(", ""), children[1].Replace(")", ""));
            }

            Input input = new Input(directions, network);

            return input;
        }

        private static void PartOne(Input input)
        {
            string currentNode = "AAA";
            string endNode = "ZZZ";

            int stepCount = 0;
            while (currentNode != endNode)
            {
                for (int i = 0; i < input.Directions.Length & currentNode != endNode; i++)
                {
                    switch (input.Directions[i])
                    {
                        case 'L':
                            currentNode = input.Network[currentNode].Left;
                            break;
                        case 'R':
                            currentNode = input.Network[currentNode].Right;
                            break;
                    }

                    stepCount++;
                }
            }

            Console.WriteLine($"Part one: Total steps = {stepCount}");
        }

        private static bool AreNodesFinal(string[] nodes)
        {
            return nodes.All(x => x[2] == 'Z');
        }

        private static void PartTwoNaive(Input input)
        {
            string[] currentNodes = input.Network.Keys.Where(x => x[2] == 'A').ToArray();

            int stepCount = 0;
            while (!AreNodesFinal(currentNodes))
            {
                for (int i = 0; i < input.Directions.Length & !AreNodesFinal(currentNodes); i++)
                {
                    for (int j = 0; j < currentNodes.Length; j++)
                    {
                        switch (input.Directions[i])
                        {
                            case 'L':
                                currentNodes[j] = input.Network[currentNodes[j]].Left;
                                break;
                            case 'R':
                                currentNodes[j] = input.Network[currentNodes[j]].Right;
                                break;
                        }
                    }

                    stepCount++;
                }
            }

            Console.WriteLine($"Part two: Total steps = {stepCount}");
        }

        private static long HighestCommonFactor(long a, long b)
        {
            if (a == b)
            {
                return a;
            }
            else if (b > a)
            {
                (a, b) = (b, a);
            }

            while (b != 0)
            {
                long bPrevious = b;
                b = a % b;
                a = bPrevious;
            }

            return a;
        }

        private static long LowestCommonMultiple(long a, long b)
        {
            return (a / HighestCommonFactor(a, b)) * b;
        }

        private static void PartTwo(Input input)
        {
            string[] startEndNodes = input.Network.Keys.Where(x => x[2] == 'A' | x[2] == 'Z').ToArray();

            Dictionary<string, Tuple<string, long>> nodeMapping = new Dictionary<string, Tuple<string, long>>();

            for (int j = 0; j < startEndNodes.Length; j++)
            {
                string currentNode = startEndNodes[j];

                long stepCount = 0;
                while (currentNode[2] != 'Z' | stepCount == 0)
                {
                    for (int i = 0; (i < input.Directions.Length & currentNode[2] != 'Z') | stepCount == 0; i++)
                    {
                        switch (input.Directions[i])
                        {
                            case 'L':
                                currentNode = input.Network[currentNode].Left;
                                break;
                            case 'R':
                                currentNode = input.Network[currentNode].Right;
                                break;
                        }

                        stepCount++;
                    }
                }

                Console.WriteLine($"Part one: Total steps = {stepCount}, {stepCount % input.Directions.Length}");

                nodeMapping[startEndNodes[j]] = new Tuple<string, long>(currentNode, stepCount);
            }

            string[] currentNodes = input.Network.Keys.Where(x => x[2] == 'A').ToArray();

            long[] transitionTimes = currentNodes.Select(x => nodeMapping[x].Item2).ToArray();

            long transistionTimesLCM = transitionTimes[0];
            for (int i = 1; i < transitionTimes.Length; i++)
            {
                transistionTimesLCM = LowestCommonMultiple(transistionTimesLCM, transitionTimes[i]);
            }

            Console.WriteLine($"Part two: Total steps {transistionTimesLCM}");

            //long[] stepCounts = new long[currentNodes.Length];

            //currentNodes[0] = nodeMapping[currentNodes[0]].Item1;
            //stepCounts[0] += nodeMapping[currentNodes[0]].Item2;

            //long maxStepCount = stepCounts.Max();

            //while (!stepCounts.All(x => x == stepCounts[0]))
            //{
            //    for (int i = 0; i < currentNodes.Length & !stepCounts.All(x => x == stepCounts[0]); i++)
            //    {
            //        if (stepCounts[i] < maxStepCount)
            //        {
            //            currentNodes[i] = nodeMapping[currentNodes[i]].Item1;
            //            stepCounts[i] += nodeMapping[currentNodes[i]].Item2;

            //            maxStepCount = stepCounts.Max();
            //        }
            //    }
            //}

            //Console.WriteLine($"Part two: Total steps {stepCounts[0]}");
        }
    }
}