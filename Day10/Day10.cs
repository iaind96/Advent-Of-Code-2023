using System.Data;

namespace Day10
{
    internal class DAy10
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepathExample2 = "input_example2.txt";
        private static readonly string inputFilepathExample3 = "input_example3.txt";
        private static readonly string inputFilepathExample4 = "input_example4.txt";
        private static readonly string inputFilepathExample5 = "input_example5.txt";
        private static readonly string inputFilepath = "input.txt";

        private record struct Point(int Row, int Column, char Pipe);

        private record struct Direction(int X, int Y)
        {
            public static Direction North = new Direction(-1, 0);
            public static Direction South = new Direction(1, 0);
            public static Direction West = new Direction(0, -1);
            public static Direction East = new Direction(0, 1);

            public static Direction None = new Direction(0, 0);

            public static List<Direction> All = new List<Direction>() { North, South, East, East };
        }

        static void Main(string[] args)
        {
            string[] inputs = ReadInput(inputFilepath);

            PartOne(inputs);
            PartTwo(inputs);
        }

        private static string[] ReadInput(string inputFilepath)
        {
            string[] lines = File.ReadLines(inputFilepath).ToArray();
            return lines;
        }

        private static Point FindStart(string[] map)
        {
            bool isStartFound = false;
            int index = 0;
            int rowLength = map[0].Length;
            int startRow = 0;
            int startColumn = 0;
            while (!isStartFound)
            {
                startRow = index / rowLength;
                startColumn = index % rowLength;

                if (map[startRow][startColumn] == 'S')
                {
                    isStartFound = true;
                }

                index++;
            }

            return new Point(startRow, startColumn, map[startRow][startColumn]);
        }

        private static bool IsValidStartingTile(Direction direction, char tile)
        {
            if (direction == Direction.North)
            {
                return "|7F".Any(x => x == tile);
            }
            else if (direction == Direction.South) 
            {
                return "|LJ".Any(x => x == tile);
            }
            else if (direction == Direction.West)
            {
                return "-J7".Any(x => x == tile);
            }
            else if (direction == Direction.East)
            {
                return "-LF".Any(x => x == tile);
            }
            else
            {
                throw new Exception("Not valid direction");
            }
        }

        private static List<Point> WalkPath(int startRow, int startColumn, string[] map)
        {
            List<Point> path = new List<Point>();
            path.Add(new Point(startRow, startColumn, map[startRow][startColumn]));

            int currentRow = startRow;
            int currentColumn = startColumn;
            Direction currentDirection = Direction.None;
            foreach (Direction direction in Direction.All)
            {
                currentRow = startRow + direction.X;
                currentColumn = startColumn + direction.Y;
                
                if (currentRow >= 0 & currentRow < map.Length & currentColumn >= 0 & currentColumn < map[0].Length && IsValidStartingTile(direction, map[currentRow][currentColumn]))
                {
                    currentDirection = direction;
                    break;
                }
            }

            path.Add(new Point(currentRow, currentColumn, map[currentRow][currentColumn]));

            while (currentRow != startRow | currentColumn != startColumn)
            {
                switch (map[currentRow][currentColumn])
                {
                    case 'L':
                        if (currentDirection == Direction.South)
                        {
                            currentDirection = Direction.East;
                        }
                        else if (currentDirection == Direction.West)
                        {
                            currentDirection = Direction.North;
                        }
                        break;

                    case 'J':
                        if (currentDirection == Direction.South)
                        {
                            currentDirection = Direction.West;
                        }
                        else if (currentDirection == Direction.East)
                        {
                            currentDirection = Direction.North;
                        }
                        break;

                    case '7':
                        if (currentDirection == Direction.North)
                        {
                            currentDirection = Direction.West;
                        }
                        else if (currentDirection == Direction.East)
                        {
                            currentDirection = Direction.South;
                        }
                        break;

                    case 'F':
                        if (currentDirection == Direction.North)
                        {
                            currentDirection = Direction.East;
                        }
                        else if (currentDirection == Direction.West)
                        {
                            currentDirection = Direction.South;
                        }
                        break;

                    default:
                        break;
                }

                currentRow += currentDirection.X;
                currentColumn += currentDirection.Y;

                path.Add(new Point(currentRow, currentColumn, map[currentRow][currentColumn]));
            }

            return path;
        }

        private static void PartOne(string[] input)
        {
            Point startPoint = FindStart(input);

            List<Point> path = WalkPath(startPoint.Row, startPoint.Column, input);

            Console.WriteLine($"Part one: Further point from start = {(path.Count - 1) / 2}");
        }

        private static void PartTwo(string[] input)
        {
            Point startPoint = FindStart(input);

            List<Point> path = WalkPath(startPoint.Row, startPoint.Column, input);
            path.RemoveAt(0);

            // replace the starting point with the effective pipe piece it is (works for actual problem input only)
            Point lastPoint = path[path.Count - 1];
            Point newLastPoint = new Point(lastPoint.Row, lastPoint.Column, 'L');
            path.RemoveAt(path.Count - 1);
            path.Add(newLastPoint);

            int enclosedTileCount = 0;
            for (int currentRow = 0; currentRow < input.Length; currentRow++)
            {
                List<Point> pointsOnRow = path.Where(point => point.Row == currentRow).OrderBy(point => point.Column).ToList();

                bool isEnclosed = false;
                bool isOnHorizontal = false;
                char lastCorner = '.';
                for (int pointIndex = 0; pointIndex < pointsOnRow.Count - 1; pointIndex++)
                {

                    if (pointsOnRow[pointIndex].Pipe == '|')
                    {
                        isEnclosed = !isEnclosed;
                    }
                    else if ("LJ7F".Any(x => x == pointsOnRow[pointIndex].Pipe) & !isOnHorizontal)
                    {
                        lastCorner = pointsOnRow[pointIndex].Pipe;
                        isOnHorizontal = true;
                    }
                    else if ("LJ7F".Any(x => x == pointsOnRow[pointIndex].Pipe) & isOnHorizontal)
                    {
                        if (lastCorner == 'L')
                        {
                            isEnclosed = pointsOnRow[pointIndex].Pipe == '7' ^ isEnclosed;
                        }
                        else if (lastCorner == 'F')
                        {
                            isEnclosed = pointsOnRow[pointIndex].Pipe == 'J' ^ isEnclosed;
                        }

                        isOnHorizontal = false;
                    }

                    if (isEnclosed)
                    {
                        enclosedTileCount += pointsOnRow[pointIndex + 1].Column - pointsOnRow[pointIndex].Column - 1;
                    }
                }
            }

            Console.WriteLine($"Part two: Enclosed tile count = {enclosedTileCount}");
        }
    }
}