namespace Day12
{
    internal class Day12
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Record(string Conditions, List<int> DamagedGroups);

        static void Main(string[] args)
        {
            List<Record> inputs = ReadInput(inputFilepathExample);

            PartOne(inputs);
            //PartTwo(inputs);

            List<Record> inputsUnfolded = UnfoldRecords(inputs);
            PartTwo(inputsUnfolded);
        }

        private static List<Record> ReadInput(string inputFilepath)
        {
            var lines = File.ReadLines(inputFilepath);
            
            List<Record> records = new List<Record>();
            foreach (var line in lines)
            {
                string[] recordComponents = line.Split();
                List<int> damageGroups = recordComponents[1].Split(',').Select(int.Parse).ToList();
                records.Add(new Record(recordComponents[0], damageGroups));
            }

            return records;
        }

        private static List<Record> UnfoldRecords(List<Record> records, int N = 5)
        {
            List<Record> unfoldedRecords = new List<Record>();
            foreach (Record record in records)
            {
                string unfoldedConditions = string.Join('?', Enumerable.Repeat(record.Conditions, N));
                List<int> unfoldedGroups = Enumerable.Repeat(record.DamagedGroups, N).SelectMany(x => x).ToList();
                unfoldedRecords.Add(new Record(unfoldedConditions, unfoldedGroups));
            }
            return unfoldedRecords;
        }

        private static void PartOne(List<Record> records)
        {

            int totalValidArrangementsSum = 0;
            foreach (Record record in records)
            {
                int totalSprings = record.DamagedGroups.Sum();

                int springsKnown = 0;
                List<int> potentialSpringIndices = new List<int>();
                for (int i = 0; i < record.Conditions.Length; i++)
                {
                    if (record.Conditions[i] == '#') springsKnown++;
                    else if (record.Conditions[i] == '?') potentialSpringIndices.Add(i);
                }
                
                int damagedSpringsToFind = totalSprings - springsKnown;

                List<List<int>> potentiaSpringPositions = GetCombinations(potentialSpringIndices, damagedSpringsToFind);

                int totalValidArrangements = 0;
                foreach (List<int> positions in potentiaSpringPositions)
                {
                    char[] potentialArrangementArray = record.Conditions.ToCharArray();
                    foreach (int position in positions)
                    {
                        potentialArrangementArray[position] = '#';
                    }
                    string potentialArrangement = new string(potentialArrangementArray).Replace('?', '.');

                    if (CheckArrangement(potentialArrangement, record.DamagedGroups))
                    {
                        totalValidArrangements++;
                    }
                }

                totalValidArrangementsSum += totalValidArrangements;
            }

            Console.WriteLine($"Part one: Total valid arrangements sum = {totalValidArrangementsSum}");
        }

        private static void PartTwo(List<Record> records)
        {
            int x = GetArrangementCount(records[5].Conditions, records[5].DamagedGroups);

            int totalValidArrangementsSum = 0;
            foreach (Record record in records)
            {
                int totalValidArrangements = GetArrangementCount(record.Conditions, record.DamagedGroups);

                totalValidArrangementsSum += totalValidArrangements;
            }

            Console.WriteLine($"Part two: Total valid arrangements sum = {totalValidArrangementsSum}");
        }

        private static int GetArrangementCount(string conditions, List<int> damagedGroups)
        {
            int arrangementsCount = 0;

            RecurseArrangements(conditions, ref arrangementsCount, damagedGroups, 0, 0);

            return arrangementsCount;
        }

        private static void RecurseArrangements(string conditions, ref int arrangementsCount, List<int> groupSizes, int currentOffsetConditions, int currentGroupIndex)
        {
            if (currentGroupIndex == groupSizes.Count)
            {
                arrangementsCount += 1;
                return;
            }

            int currentGroupSize = groupSizes[currentGroupIndex];
            for (int i = currentOffsetConditions; i < conditions.Length - currentGroupSize + 1; i++)
            {
                bool isCurrentGroupPossible = true;
                // check if previous value is a #
                if (i - 1 > 0)
                {
                    isCurrentGroupPossible &= conditions[i - 1] != '#';
                    return;
                }

                // check if possible to form group in next items in conditions
                for (int j = i; j < i + currentGroupSize; j++)
                {
                    isCurrentGroupPossible &= conditions[j] == '?' | conditions[j] == '#';
                }

                // need to check if next value is not a # or end of condtions
                if (i + currentGroupSize < conditions.Length)
                {
                    isCurrentGroupPossible &= conditions[i + currentGroupSize] != '#';
                    
                    //// return as this means it's not possible to fit the group beforee the next #
                    //return;
                }

                if (isCurrentGroupPossible)
                {
                    RecurseArrangements(conditions, ref arrangementsCount, groupSizes, i + currentGroupSize + 1, currentGroupIndex + 1);
                }
            }
        }

        private static bool CheckArrangement(string arrangement, List<int> damagedGroups)
        {
            string[] groups = arrangement.Split('.', StringSplitOptions.RemoveEmptyEntries);
            List<int> groupSizes = groups.Select(x => x.Length).ToList();

            bool isValid = damagedGroups.SequenceEqual(groupSizes);

            return isValid;
        }

        private static List<List<int>> GetCombinations(List<int> items, int R)
        {
            int[] combinationBuffer = new int[R];
            List<List<int>> combinations = new List<List<int>>();

            RecurseCombinations(items, combinationBuffer, combinations, 0, 0, R);

            return combinations;
        }

        private static void RecurseCombinations(List<int> items, int[] combinationBuffer, List<List<int>> combinations, int currentOffset, int currentIndex, int R)
        {
            if (currentIndex == R)
            {
                combinations.Add(combinationBuffer.ToList());
                return;
            }

            for (int i = currentOffset; i < items.Count; i++)
            {
                combinationBuffer[currentIndex] = items[i];
                RecurseCombinations(items, combinationBuffer, combinations, i + 1, currentIndex + 1, R);
            }
        }
    }
}