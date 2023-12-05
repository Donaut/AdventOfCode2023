namespace Day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var games = new List<Game>();

            var lines = File.ReadLines("./input.txt");

            // Some test :D
            //lines = """
            //    Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            //    Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            //    Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            //    Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            //    Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            //    Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            //    """.Split("\r\n");

            // Parsing the file
            foreach (var line in lines)
            {
                var firstSplit = line.Split(':');
                var gameId = int.Parse(firstSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

                var secondSplit = firstSplit[1].Split('|');
                var winningNumbers = secondSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                var numbers = secondSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                games.Add(new Game()
                {
                    Id = gameId,
                    WinningNumbers = winningNumbers,
                    Numbers = numbers
                });
            }

            var firstQuestion = games.Sum(x =>
            {
                var count = x.WinningNumbers.Where(y => x.Numbers.Contains(y)).Count();

                if(count == 0)
                    return 0;

                var sum = 1;
                for(var i = 1; i < count; i++)
                {
                    sum = sum + sum;
                }

                return sum;
            });

            
            var instanceCounts = games.ToDictionary(x => x.Id, y => 1);
            foreach (var game in games)
            {
                var count = game.WinningNumbers.Where(y => game.Numbers.Contains(y)).Count();

                var current = instanceCounts[game.Id];
                for (var i = 1; i < count + 1; i++)
                {
                    var id = game.Id + i;
                    instanceCounts[id] += current;
                }
            }

            var last = games.Last();
            var second = instanceCounts.Where(x => x.Key <= last.Id).Sum(x => x.Value);
            ;
        }
    }

    class Game
    {
        public int Id;

        public int[] WinningNumbers = Array.Empty<int>();

        public int[] Numbers = Array.Empty<int>();
    }
}
