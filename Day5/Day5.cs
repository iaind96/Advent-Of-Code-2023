using System.Text.RegularExpressions;

namespace Day5
{
    internal class Day5
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Input(List<long> Seeds, List<Mapping> Mappings);

        private record struct MappingRange(long DestinationRangeStart, long SourceRangeStart, long RangeLength);

        public record struct ValueRange(long RangeStart, long RangeLength);

        private class Mapping
        {
            private List<MappingRange> mappingRanges;

            public Mapping()
            {
                mappingRanges = new List<MappingRange>();
            }

            public void AddMappingRange(long destinationRangeStart, long sourceRangeStart, long rangeLength)
            {
                mappingRanges.Add(new MappingRange(destinationRangeStart, sourceRangeStart, rangeLength));
            }

            public void SortMappingRanges()
            {
                mappingRanges = mappingRanges.OrderBy(mappingRange => mappingRange.SourceRangeStart).ToList();
            }

            public long MapSourceToDestination(long sourceValue)
            {
                foreach (MappingRange mappingRange in mappingRanges)
                {
                    if (sourceValue >= mappingRange.SourceRangeStart && sourceValue - mappingRange.SourceRangeStart < mappingRange.RangeLength)
                    {
                        return mappingRange.DestinationRangeStart + sourceValue - mappingRange.SourceRangeStart; 
                    }
                }

                return sourceValue;
            }

            public List<ValueRange> MapSourceRangeToDestinationRanges(long sourceRangeStart, long sourceRangeLength)
            {
                long currentValue = sourceRangeStart;
                long valuesRemaining = sourceRangeLength;
                
                List<ValueRange> destinationRanges = new List<ValueRange>();

                int mappingIndex = 0;
                while (valuesRemaining > 0 & mappingIndex < mappingRanges.Count)
                {
                    MappingRange currentMapping = mappingRanges[mappingIndex];

                    if (currentValue < currentMapping.SourceRangeStart)
                    {
                        long rangeStart = currentValue;
                        long rangeLength = Math.Min(valuesRemaining, currentMapping.SourceRangeStart - rangeStart);

                        destinationRanges.Add(new ValueRange(rangeStart, rangeLength));

                        currentValue = currentMapping.SourceRangeStart;
                        valuesRemaining -= rangeLength;
                    }
                    else if (currentValue >= currentMapping.SourceRangeStart && currentValue - currentMapping.SourceRangeStart < currentMapping.RangeLength)
                    {
                        long rangeStart = currentMapping.DestinationRangeStart + currentValue - currentMapping.SourceRangeStart;
                        long rangeLength = Math.Min(valuesRemaining, currentMapping.DestinationRangeStart + currentMapping.RangeLength - rangeStart);

                        destinationRanges.Add(new ValueRange(rangeStart, rangeLength));

                        currentValue = currentMapping.SourceRangeStart + currentMapping.RangeLength;
                        valuesRemaining -= rangeLength;

                    }
                    else
                    {
                        mappingIndex += 1;
                    }
                }

                if (valuesRemaining > 0)
                {
                    destinationRanges.Add(new ValueRange(currentValue, valuesRemaining));
                }

                return destinationRanges;
            }
        }

        static void Main(string[] args)
        {
            Input input = ReadInput(inputFilepath);

            PartOne(input);
            PartTwo(input);
        }

        private static Input ReadInput(string inputFilepath)
        {
            List<long> seeds;
            List<Mapping> mappings;

            using (StreamReader streamReader = new StreamReader(inputFilepath))
            {
                // get seeds
                seeds = streamReader.ReadLine().Split(": ")[1].Split().Select(long.Parse).ToList();
                streamReader.ReadLine();

                // get mappings
                mappings = new List<Mapping>();

                string line;
                Mapping mapping = new Mapping();
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (Regex.Match(line, @"map").Success)
                    {
                        mapping = new Mapping();
                    }
                    else if (string.IsNullOrWhiteSpace(line))
                    {
                        mapping.SortMappingRanges();
                        mappings.Add(mapping);
                    }
                    else
                    {
                        List<long> mappingRange = line.Split().Select(long.Parse).ToList();
                        mapping.AddMappingRange(mappingRange[0], mappingRange[1], mappingRange[2]);
                    }
                }

                // add final mapping
                mapping.SortMappingRanges();
                mappings.Add(mapping);
            }

            Input input = new Input(seeds, mappings);

            return input;
        }

        private static void PartOne(Input input)
        {
            List<long> mappedValues = new List<long>();

            foreach (long seed in input.Seeds)
            {
                long mappedValue = seed;
                foreach (Mapping mapping in input.Mappings)
                {
                    mappedValue = mapping.MapSourceToDestination(mappedValue);
                }
                mappedValues.Add(mappedValue);
            }

            long minimumLocationNumber = mappedValues.Min();

            Console.WriteLine($"Part one: Minimum location number = {minimumLocationNumber}");
        }

        private static void PartTwo(Input input)
        {
            List<ValueRange> seedRanges= new List<ValueRange>();
            for (int i = 0; i < input.Seeds.Count; i += 2)
            {
                seedRanges.Add(new ValueRange(input.Seeds[i], input.Seeds[i + 1]));
            }

            long minimumLocationNumber = long.MaxValue;

            foreach (ValueRange seedRange in seedRanges)
            {
                List<ValueRange> mappedRanges = new List<ValueRange>() { seedRange };
                foreach (Mapping mapping in input.Mappings)
                {
                    List<ValueRange> mappedRangesNext = new List<ValueRange>();
                    foreach (ValueRange mappedRange in mappedRanges)
                    {
                        mappedRangesNext.AddRange(mapping.MapSourceRangeToDestinationRanges(mappedRange.RangeStart, mappedRange.RangeLength));
                    }
                    mappedRanges = mappedRangesNext;
                }

                mappedRanges = mappedRanges.OrderBy(vr => vr.RangeStart).ToList();

                minimumLocationNumber = Math.Min(minimumLocationNumber, mappedRanges[0].RangeStart);
            }

            Console.WriteLine($"Part two: Minimum location number = {minimumLocationNumber}");
        }
    }
}