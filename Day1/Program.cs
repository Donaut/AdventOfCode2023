namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sum = File.ReadLines("./input.txt")
                .Select(line =>
                {
                    Dictionary<string, int> values = new()
                    {
                        { "1", 1 },
                        { "2", 2 },
                        { "3", 3 },
                        { "4", 4 },
                        { "5", 5 },
                        { "6", 6 },
                        { "7", 7 },
                        { "8", 8 },
                        { "9", 9 },
                        { "one", 1 },
                        { "two", 2 },
                        { "three", 3 },
                        { "four", 4 },
                        { "five", 5 },
                        { "six", 6 },
                        { "seven", 7 },
                        { "eight", 8 },
                        { "nine", 9 }
                    };

                    var numbers = values.Keys.ToArray();
                    
                    var first = FindFirstOccurrenceOfOfAny(line, numbers);
                    var last = FindLastOccurrenceOfAny(line, numbers);
                    
                    return $"{values[first]}{values[last]}";
                })
                .Select(int.Parse)
                .Sum();

            ;
        }


        /// <summary>
        /// Finds the first occurrence of any string from a given array in a text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <param name="values">An array of strings to find in the text.</param>
        /// <returns>The string from the array that first occurs in the text. If none of the strings are found, it returns an empty string.</returns>
        public static string FindFirstOccurrenceOfOfAny(string text, string[] values)
        {
            var bestIndex = int.MaxValue;
            var bestItem = string.Empty;
            foreach (var item in values)
            {
                var index = text.IndexOf(item);
                if(index == -1)
                    continue;

                if(index < bestIndex)
                {
                    bestIndex = index;
                    bestItem = item;
                }
            }

            return bestItem;
        }

        /// <summary>
        /// Same as <see cref="FindFirstOccurrenceOfOfAny(string, string[])"/> but starts from the back.
        /// </summary>
        public static string FindLastOccurrenceOfAny(string text, string[] values)
        {
            var bestIndex = int.MinValue;
            var bestItem = string.Empty;
            foreach (var item in values)
            {
                var index = text.LastIndexOf(item);
                if(index == -1)
                    continue;

                if(index > bestIndex)
                {
                    bestIndex = index;
                    bestItem = item;
                }
            }

            return bestItem;
        }
    }
}
