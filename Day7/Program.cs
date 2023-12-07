using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt");

            //lines = """
            //    32T3K 765
            //    T55J5 684
            //    KK677 28
            //    KTJJT 220
            //    QQQJA 483
            //    """.Split("\r\n");

            var games = lines.Select(x =>
                {
                    var line = x.Split(' ');

                    var hand = line[0];
                    var bid = long.Parse(line[1]);

                    return new Game(hand, bid);
                })
                .OrderBy(x => x.Hand, GameComparer.Default);

            var question = games.Select((x, i) => x.Bid * (i + 1)).Sum();

            var success = question == 251135960;

            ;
        }

        class GameComparer : IComparer<string>
        {
            public static GameComparer Default = new GameComparer();

            /// <summary>
            /// Used for deciding the sort order for single characters.
            /// </summary>
            private readonly Dictionary<char, int> _characterTable;

            private readonly char[] _characterArray;

            /// <summary>
            /// Used for deciding the sort order for whole cards.
            /// </summary>
            //private readonly Dictionary<string, int> _cardTable;

            public GameComparer()
            {
                _characterArray = new[] { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
                Array.Reverse(_characterArray); // Too lazy to flip the letters
                _characterTable = _characterArray.ToDictionary(x => x, y => Array.IndexOf(_characterArray, y) + 1);
            }

            public int Compare(string? first, string? second)
            {
                if(first == null && second == null)
                    return 0;
                else if(first == null)
                    return -1;
                else if(second == null)
                    return 1;

                var jokerFirst = FindBestJoker(first);
                var jokerSecond = FindBestJoker(second);

                var category = CompareCategory(jokerFirst, jokerSecond);

                if (category == 0)
                    return CompareHands(first, second);

                return category;
            }

            private int CompareHands(string x, string y)
            {
                for (var i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        var x1 = _characterTable[x[i]];
                        var y1 = _characterTable[y[i]];

                        if (x1 >= y1)
                            return 1;
                        else
                            return -1;
                    }
                }
                return 0;
            }

            private string FindBestJoker(string hand)
            {
                var firstTurnOn = hand[0] == 'J';
                var secondTurnOn = hand[1] == 'J';
                var thirdTurnOn = hand[2] == 'J';
                var fourthTurnOn = hand[3] == 'J';
                var fifthTurnOn = hand[4] == 'J';

                var best = hand;
                Span<char> buffer = stackalloc char[5];
                hand.CopyTo(buffer);

                for (var i = 0; i < (firstTurnOn ? _characterArray.Length : 1); i++)
                {
                    if (firstTurnOn)
                        buffer[0] = _characterArray[i];

                    for (var i2 = 0; i2 < (secondTurnOn ? _characterArray.Length : 1); i2++)
                    {
                        if (secondTurnOn)
                            buffer[1] = _characterArray[i2];

                        for (var i3 = 0; i3 < (thirdTurnOn ? _characterArray.Length : 1); i3++)
                        {
                            if (thirdTurnOn)
                                buffer[2] = _characterArray[i3];

                            for (var i4 = 0; i4 < (fourthTurnOn ? _characterArray.Length : 1); i4++)
                            {
                                if (fourthTurnOn)
                                    buffer[3] = _characterArray[i4];

                                for (var i5 = 0; i5 < (fifthTurnOn ? _characterArray.Length : 1); i5++)
                                {
                                    if (fifthTurnOn)
                                        buffer[4] = _characterArray[i5];

                                    var currentString = new string(buffer);
                                    var current = CompareCategory(best, currentString);

                                    if (current < 0)
                                        best = currentString;
                                    ;
                                }
                            }
                        }
                    }
                }

                return best;
            }

            private int CompareCategory(string x, string y)
            {
                var xCounts = x.GroupBy(x => x).Select(x => new { Char = x.Key, Count = x.Count() }).ToArray();
                var yCounts = y.GroupBy(x => x).Select(x => new { Char = x.Key, Count = x.Count() }).ToArray();

                {
                    var firstIsFiveOfAKind = xCounts.Length == 1;
                    var secondIsFiveOfAKind = yCounts.Length == 1;

                    if (firstIsFiveOfAKind && !secondIsFiveOfAKind)
                        return 1;
                    else if (secondIsFiveOfAKind && !firstIsFiveOfAKind)
                        return -1;
                }

                {
                    var xIsFourOfAKind = xCounts.Any(x => x.Count == 4);
                    var yIsFourOfAKind = yCounts.Any(x => x.Count == 4);

                    if (xIsFourOfAKind && !yIsFourOfAKind)
                        return 1;
                    else if (yIsFourOfAKind && !xIsFourOfAKind)
                        return -1;
                }

                {
                    var xIsFullHouse = xCounts.Any(x => x.Count == 3) && xCounts.Length == 2;
                    var yIsFullHouse = yCounts.Any(x => x.Count == 3) && yCounts.Length == 2;

                    if (xIsFullHouse && !yIsFullHouse)
                        return 1;
                    else if (yIsFullHouse && !xIsFullHouse)
                        return -1;
                }


                {
                    var xIsThreeOfAKind = xCounts.Any(x => x.Count == 3) && xCounts.Length == 3;
                    var yIsThreeOfAKind = yCounts.Any(x => x.Count == 3) && yCounts.Length == 3;

                    if (xIsThreeOfAKind && !yIsThreeOfAKind)
                        return 1;
                    else if (yIsThreeOfAKind && !xIsThreeOfAKind)
                        return -1;
                }

                {
                    var xIsTwoPair = xCounts.Count(x => x.Count == 2) == 2;
                    var yIsTwoPair = yCounts.Count(y => y.Count == 2) == 2;

                    if (xIsTwoPair && !yIsTwoPair)
                        return 1;
                    else if (yIsTwoPair && !xIsTwoPair)
                        return -1;

                }

                {
                    var xIsOnePair = xCounts.Count(x => x.Count == 2) == 1;
                    var yIsOnePair = yCounts.Count(x => x.Count == 2) == 1;

                    if (xIsOnePair && !yIsOnePair)
                        return 1;
                    else if (yIsOnePair && !xIsOnePair)
                        return -1;
                }

                {
                    var xIsHighCard = xCounts.Length == 5;
                    var yIsHighCard = yCounts.Length == 5;

                    if (xIsHighCard && !yIsHighCard)
                        return 1;
                    else if (yIsHighCard && !xIsHighCard)
                        return -1;

                }

                return 0;
            }
        }

        public record Game(string Hand, long Bid);
    }
}
