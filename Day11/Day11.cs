using System.Security.Cryptography.X509Certificates;

namespace Day11
{
    internal class Day11
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Coordinate(int Row, int Column);

        private class GalaxyMap
        {
            private List<List<char>> characterArray;

            private List<int> rowIndicesToExpand;
            private List<int> columnIndicesToExpand;
            
            public GalaxyMap(List<List<char>> characterArray)
            {
                this.characterArray = characterArray.Select(x => x.ToList()).ToList();

                // first find rows to expand
                rowIndicesToExpand = new List<int>();
                for (int i = 0; i < characterArray.Count; i++)
                {
                    List<char> row = characterArray[i];
                    if (row.All(x => x == '.'))
                    {
                        rowIndicesToExpand.Add(i);
                    }
                }

                // now find columns to expand
                columnIndicesToExpand = new List<int>();
                for (int i = 0; i < characterArray[0].Count; i++)
                {
                    List<char> column = characterArray.Select(x => x[i]).ToList();
                    if (column.All(x => x == '.'))
                    {
                        columnIndicesToExpand.Add(i);
                    }
                }
            }

            public List<Coordinate> FindGalaxies()
            {
                List<Coordinate> galaxies = new List<Coordinate>();

                for (int i = 0; i < characterArray.Count; i++)
                {
                    for (int j = 0; j < characterArray[i].Count; j++)
                    {
                        if (characterArray[i][j] == '#')
                        {
                            galaxies.Add(new Coordinate(i, j));
                        }
                    }
                }

                return galaxies;
            }

            public void ExpandMap()
            {
                int mapWidth = characterArray[0].Count;
                List<char> emptyRow = Enumerable.Repeat('.', mapWidth).ToList();

                int insertedRows = 0;
                foreach (int index in rowIndicesToExpand)
                {
                    characterArray.Insert(index + insertedRows, emptyRow.ToList());
                    insertedRows++;
                }

                int insertedColumns = 0;
                foreach (int index in columnIndicesToExpand)
                {
                    foreach (List<char> row in characterArray)
                    {
                        row.Insert(index + insertedColumns, '.');
                    }
                    insertedColumns++;
                }
            }

            public long ShortestDistance(Coordinate a, Coordinate b, int expansionFactor)
            {
                int shortestDistanceWithoutExpansion = Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);

                int minRow = Math.Min(a.Row, b.Row);
                int maxRow = Math.Max(a.Row, b.Row);
                int minColumn = Math.Min(a.Column, b.Column);
                int maxColumn = Math.Max(a.Column, b.Column);

                int numberOfRowsToExpand = rowIndicesToExpand.Where(x => x > minRow && x < maxRow).Count();
                int numberColumnsToExpand = columnIndicesToExpand.Where(x => x > minColumn && x < maxColumn).Count();

                return shortestDistanceWithoutExpansion + (numberOfRowsToExpand + numberColumnsToExpand) * (expansionFactor - 1);
            }
        }

        static void Main(string[] args)
        {
            List<List<char>> input = ReadInput(inputFilepath);

            PartOne(new GalaxyMap(input));
            PartTwo(new GalaxyMap(input));
        }

        private static List<List<char>> ReadInput(string inputFilepath)
        {
            List<List<char>> lines = File.ReadLines(inputFilepath).Select(x => x.ToList()).ToList();
            return lines;
        }

        private static int ShortestDistance(Coordinate a, Coordinate b)
        {
            return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
        }

        private static void PartOne(GalaxyMap input)
        {
            input.ExpandMap();
            List<Coordinate> galaxies = input.FindGalaxies();

            int shortestDistanceSum = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++) 
                {
                    shortestDistanceSum += ShortestDistance(galaxies[i], galaxies[j]);
                }
            }

            Console.WriteLine($"Part one: Shortest distance sum = {shortestDistanceSum}");
        }

        private static void PartTwo(GalaxyMap input)
        {
            List<Coordinate> galaxies = input.FindGalaxies();

            long shortestDistanceSum = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    shortestDistanceSum += input.ShortestDistance(galaxies[i], galaxies[j], 1_000_000);
                }
            }

            Console.WriteLine($"Part two: Shortest distance sum = {shortestDistanceSum}");
        }
    }

}