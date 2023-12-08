using System.Numerics;

namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt");

            //lines = """
            //    LR

            //    11A = (11B, XXX)
            //    11B = (XXX, 11Z)
            //    11Z = (11B, XXX)
            //    22A = (22B, XXX)
            //    22B = (22C, 22C)
            //    22C = (22Z, 22Z)
            //    22Z = (22B, 22B)
            //    XXX = (XXX, XXX)
            //    """.Split("\r\n");

            var directions = lines.First();

            var maps = lines
                .Skip(2) // Skip the first two lines so we can parse more easily
                .Select(x => new
                {
                    Name = x.Substring(0, 3),
                    Left = x.Substring(7, 3),
                    Right = x.Substring(12, 3)
                })
                .ToDictionary(x => x.Name);

            // FIRST QUESTION
            //{
            //    var current = "AAA";
            //    var count = 0;
            //    var directionIndex = 0;
            //    while (current != "ZZZ")
            //    {
            //        var direction = directions[directionIndex % directions.Length];

            //        var map = maps[current];

            //        current = direction switch
            //        {
            //            'L' => map.Left,
            //            'R' => map.Right,
            //            _ => throw new NotImplementedException($"Direction is not implemented: {direction}")
            //        };

            //        count++;
            //        directionIndex++;
            //    }

            //    var firstQuestionAnswer = count;
            //}

            // SECOND QUESTION
            {
                var query = maps
                    .Where(x => x.Key.EndsWith('Z'))
                    //.Where(x => x.Key.EndsWith('A'))
                    .Select(x =>
                    {
                        var start = x.Key;
                        var current = start;

                        var count = 0L;
                        var directionIndex = 0;
                        var first = true;
                        while (!current.EndsWith('Z') || first)
                        //while(!current.EndsWith('Z'))
                        {
                            first = false;
                            if (directionIndex >= directions.Length)
                                directionIndex = 0;

                            var direction = directions[directionIndex];

                            var map = maps[current];

                            current = direction switch
                            {
                                'L' => map.Left,
                                'R' => map.Right,
                                _ => throw new NotImplementedException($"Direction is not implemented: {direction}")
                            };

                            count++;
                            directionIndex++;
                        }

                        return count;
                    });

                var secondQuestion = MathHelpers.LeastCommonMultiple(query);
            }

            ;


        }
    }
}

// https://stackoverflow.com/a/74765134
public static class MathHelpers
{
    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        => a / GreatestCommonDivisor(a, b) * b;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);
}