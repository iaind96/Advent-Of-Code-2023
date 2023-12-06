namespace Day6
{
    internal class Day6
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Race(int Time, int RecordDistance);

        private record struct RaceLong(long Time, long RecordDistance);

        static void Main(string[] args)
        {
            List<Race> inputs = ReadInput(inputFilepathExample);
            PartOne(inputs);

            RaceLong input = ReadInputPartTwo(inputFilepath);
            PartTwo(input);
        }

        private static List<Race> ReadInput(string inputFilepath)
        {
            string[] lines = File.ReadLines(inputFilepath).ToArray();

            List<int> times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<int> recordDistances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            List<Race> races = new List<Race>();
            foreach ((int time, int recordDistance) in times.Zip(recordDistances))
            {
                races.Add(new Race(time, recordDistance));
            }

            return races;
        }

        private static RaceLong ReadInputPartTwo(string inputFilepath)
        {
            string[] lines = File.ReadLines(inputFilepath).ToArray();

            long time = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
            long recordDistance = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));

            RaceLong race = new RaceLong(time, recordDistance);

            return race;
        }

        private static void PartOne(List<Race> races)
        {
            int winningTimesProduct = 1;

            foreach (Race race in races)
            {
                double chargeTimeMinUnrounded = (race.Time - Math.Sqrt(race.Time * race.Time - 4 * race.RecordDistance)) / 2 + 1e-8;
                double chargeTimeMaxUnrounded = (race.Time + Math.Sqrt(race.Time * race.Time - 4 * race.RecordDistance)) / 2 - 1e-8;

                int chargeTimeMin = (int)(chargeTimeMinUnrounded + 1);
                int chargeTimeMax = (int)chargeTimeMaxUnrounded;

                int numberWinningTimes = chargeTimeMax - chargeTimeMin + 1;

                winningTimesProduct *= numberWinningTimes;
            }

            Console.WriteLine($"Part one: Winning times product = {winningTimesProduct}");
        }

        private static void PartTwo(RaceLong race)
        {
            long searchLeft = 0;
            long searchRight = race.Time / 2;

            // binary search
            while (searchLeft < searchRight)
            {
                // implicit flooring
                long midPoint = (searchLeft + searchRight) / 2;

                double distanceTravelled = race.Time * midPoint - midPoint * midPoint;

                if (distanceTravelled > race.RecordDistance)
                {
                    // don't move the right boundary below the midpoint since the solution might be non-integer
                    searchRight = midPoint;
                }
                else if (distanceTravelled < race.RecordDistance)
                {
                    searchLeft = midPoint + 1;
                }
            }

            // determine the total number of winning times
            long winningTimes;
            if (race.Time % 2 == 0)
            {
                winningTimes = (race.Time / 2 - searchRight) * 2 + 1;
            }
            else
            {
                winningTimes = (race.Time / 2 - searchRight) * 2 + 2;
            }

            Console.WriteLine($"Part two: Number of winning times = {winningTimes}");
        }
    }
}