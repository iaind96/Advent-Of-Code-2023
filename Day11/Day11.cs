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
            
            public GalaxyMap(List<List<char>> characterArray)
            {
                this.characterArray = characterArray;
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
                // first expand rows
                List<int> rowIndicesToInsert = new List<int>();
                for (int i = 0; i < characterArray.Count; i++)
                {
                    List<char> row = characterArray[i];
                    if (row.All(x => x == '.'))
                    {
                        rowIndicesToInsert.Add(i);
                    }
                }

                int mapWidth = characterArray[0].Count;
                List<char> emptyRow = Enumerable.Repeat('.', mapWidth).ToList();

                int insertedRows = 0;
                foreach (int index in rowIndicesToInsert)
                {
                    characterArray.Insert(index + insertedRows, emptyRow.ToList());
                    insertedRows++;
                }

                // now expand columns
                List<int> columnIndicesToInsert = new List<int>();
                for (int i = 0; i < characterArray[0].Count; i++)
                {
                    List<char> column = characterArray.Select(x => x[i]).ToList();
                    if (column.All(x => x == '.'))
                    {
                        columnIndicesToInsert.Add(i);
                    }
                }

                int insertedColumns = 0;
                foreach (int index in columnIndicesToInsert)
                {
                    foreach (List<char> row in characterArray)
                    {
                        row.Insert(index + insertedColumns, '.');
                    }
                    insertedColumns++;
                }
            }
        }

        static void Main(string[] args)
        {
            GalaxyMap input = ReadInput(inputFilepath);

            PartOne(input);
            PartTwo(input);
        }

        private static GalaxyMap ReadInput(string inputFilepath)
        {
            List<List<char>> lines = File.ReadLines(inputFilepath).Select(x => x.ToList()).ToList();

            GalaxyMap galaxyMap = new GalaxyMap(lines);
            return galaxyMap;
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

        }
    }
}