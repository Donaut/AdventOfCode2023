using System.Drawing;

namespace Day3
{
    struct Number
    {
        public int Value;

        public int SymbolStart;

        public int SymbolEnd;

        public int Y;
    }

    struct Symbol
    {
        public char Value;

        public int X;

        public int Y;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var numbers = new List<Number>();
            var symbols = new List<Symbol>();

            var y = -1;
            var lines = File.ReadLines("./input.txt");
            foreach (var line in lines)
            {
                var i = 0;
                y++;
                while (i < line.Length)
                {
                    if (char.IsDigit(line[i]))
                    {
                        var startIndex = i;
                        while (i < line.Length
                            && char.IsDigit(line[i]))
                        {
                            i++;
                        }

                        var number = int.Parse(line.AsSpan(startIndex, i - startIndex));
                        numbers.Add(new Number()
                        {
                            Value = number,
                            SymbolStart = startIndex - 1,
                            SymbolEnd = i + 1,
                            Y = y
                        });
                    }
                    else if (line[i] == '.')
                    {
                        // DO NOTHING.
                        i++;
                    }
                    else
                    {
                        symbols.Add(new Symbol()
                        {
                            Value = line[i],
                            X = i,
                            Y = y
                        });
                        i++;
                    }
                }
            }

            ;
            var firstQuestion = numbers.Where(number =>
            {
                var isValid = symbols.Any(symbol =>
                {
                    var isSymbolAround = symbol.Y == number.Y || symbol.Y == number.Y - 1 || symbol.Y == number.Y + 1;
                    return isSymbolAround && symbol.X >= number.SymbolStart && symbol.X < number.SymbolEnd;
                });

                return isValid;
            }).Sum(x => x.Value);

            var secondQuestion = symbols.Sum(symbol =>
            {
                if(symbol.Value != '*')
                    return 0;

                var query = numbers.Where(number =>
                {
                    var isNumberAround = number.Y == symbol.Y || number.Y == symbol.Y - 1 || number.Y == symbol.Y + 1;
                    return isNumberAround && symbol.X >= number.SymbolStart && symbol.X < number.SymbolEnd;
                }).ToList();

                if(query.Count < 2)
                    return 0;

                return query[0].Value * query[1].Value;
            });

            ;
        }
    }
}
