
namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;

            var lines = File
                .ReadLines("./input.txt")
                .Select(ParseGame);


            var firstQuestion = lines
                .Where(x =>
                {
                    var cubes = x.Sets.SelectMany(y => y);

                    var impossible = cubes.Any(cube =>
                    {
                        if (cube.Color == Color.Red && cube.Count > maxRed)
                        {
                            return true;
                        }
                        else if (cube.Color == Color.Green && cube.Count > maxGreen)
                        {
                            return true;
                        }
                        else if (cube.Color == Color.Blue && cube.Count > maxBlue)
                        {
                            return true;
                        }

                        return false;
                    });

                    return !impossible;
                })
                .Sum(x => x.GameId);

            var secondQuestion = lines
                .Select(game =>
                {
                    var cubes = game.Sets
                        .SelectMany(x => x)
                        .GroupBy(x => x.Color)
                        .Select(x => x.Max(x => x.Count));


                    return cubes.Aggregate(1, (x, y) => x * y);
                })
                .Sum();

            Console.WriteLine($"First question: {firstQuestion}");
            Console.WriteLine($"Second question: {secondQuestion}");
            ;
        }

        private static Game ParseGame(string line)
        {
            var gameIdStart = line.IndexOf(' ') + 1;
            var gameIdEnd = line.IndexOf(':');

            var gameId = int.Parse(line.Substring(gameIdStart, gameIdEnd - gameIdStart));

            var sets = line.Substring(gameIdEnd + 1).Split(';')
                .Select(x =>
                {
                    var cubes = x.Split(",")
                        .Select(c => c.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        .Select(numberAndColor => new Cube()
                        {
                            Count = int.Parse(numberAndColor[0]),
                            Color = numberAndColor[1] switch
                            {
                                "red" => Color.Red,
                                "green" => Color.Green,
                                "blue" => Color.Blue,
                                _ => throw new ArgumentException("Unmapped color", numberAndColor[1])
                            }
                        });
                    return cubes.ToArray();
                })
                .ToList();

            return new Game() { GameId = gameId, Sets = sets };
        }

        struct Game
        {
            public int GameId { get; set; }

            public List<Cube[]> Sets { get; set; } = new List<Cube[]>();

            public Game()
            {
            }
        }

        struct Cube
        {
            public int Count { get; set; }

            public Color Color { get; set; }
        }

        enum Color
        {
            Red,
            Green,
            Blue,
        }
    }
}
