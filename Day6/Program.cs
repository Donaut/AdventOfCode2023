namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("./input.txt");

            //lines = """
            //    Time:      7  15   30
            //    Distance:  9  40  200
            //    """.Split("\r\n");

            var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse);
            var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse);

            var firstQuestion = times.Zip(distances).Aggregate(1L, (seed, current) => seed * CalculateRange(current.First, current.Second));

            var bigTime = long.Parse(string.Concat(times));
            var bigDistance = long.Parse(string.Concat(distances));
            var secondQuestion = CalculateRange(bigTime, bigDistance);
            
            ;

            static long CalculateRange(long time, long distance)
            {
                distance += 1; // The new distance has to be larger than the old distance it cannot be EQUAL.

                // x^2 - time * x + distance = 0
                var a = 1D;
                double b = -time;
                double c = distance;

                var max = (-b + Math.Sqrt(b * b - 4 * a * c)) / 2 * a;
                var min = (-b - Math.Sqrt(b * b - 4 * a * c)) / 2 * a;

                var result = (long)(Math.Floor(max) - Math.Ceiling(min)) + 1;
                return result;
            }
        }

        record class Game(int time, int distance);
    }
}
