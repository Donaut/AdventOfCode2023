using System.Runtime.CompilerServices;

namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt");
            
            // Smaller data for testing
            //lines = """
            //    seeds: 79 14 55 13

            //    seed-to-soil map:
            //    50 98 2
            //    52 50 48

            //    soil-to-fertilizer map:
            //    0 15 37
            //    37 52 2
            //    39 0 15

            //    fertilizer-to-water map:
            //    49 53 8
            //    0 11 42
            //    42 0 7
            //    57 7 4

            //    water-to-light map:
            //    88 18 7
            //    18 25 70

            //    light-to-temperature map:
            //    45 77 23
            //    81 45 19
            //    68 64 13

            //    temperature-to-humidity map:
            //    0 69 1
            //    1 0 69

            //    humidity-to-location map:
            //    60 56 37
            //    56 93 4
            //    """.Split("\r\n");

            var seeds = new List<long>();
            var seedToSoil = new List<Range>();
            var soilToFertilizer = new List<Range>();
            var fertilizerToWater = new List<Range>();
            var waterToLight = new List<Range>();
            var lightToTemperature = new List<Range>();
            var temperatureToHumidity = new List<Range>();
            var humidityToLocation = new List<Range>();

            List<Range> set = null;
            foreach (var line in lines)
            {
                if(line.StartsWith("seeds:"))
                {
                    seeds = line.Remove(0, "seeds:".Length).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                    continue;
                }
                else if(line.StartsWith("seed-to-soil map:"))
                {
                    set = seedToSoil;
                    continue;
                }
                else if (line.StartsWith("soil-to-fertilizer map:"))
                {
                    set = soilToFertilizer;
                    continue;
                }
                else if (line.StartsWith("fertilizer-to-water map:"))
                {
                    set = fertilizerToWater;
                    continue;
                }
                else if (line.StartsWith("water-to-light map:"))
                {
                    set = waterToLight;
                    continue;
                }
                else if (line.StartsWith("light-to-temperature map:"))
                {
                    set = lightToTemperature;
                    continue;
                }
                else if (line.StartsWith("temperature-to-humidity map:"))
                {
                    set = temperatureToHumidity;
                    continue;
                }
                else if (line.StartsWith("humidity-to-location map:"))
                {
                    set = humidityToLocation;
                    continue;
                }

                if(string.IsNullOrEmpty(line))
                    continue;

                var splittedLine = line.Split(' ');

                set.Add(new Range()
                {
                    DestinationRangeStart = long.Parse(splittedLine[0]),
                    SourceRangeStart = long.Parse(splittedLine[1]),
                    Length = long.Parse(splittedLine[2])
                });
            }

            // Fist question seeds to locations
            var convertTables = new[]
            {
                seedToSoil,
                soilToFertilizer,
                fertilizerToWater,
                waterToLight,
                lightToTemperature,
                temperatureToHumidity,
                humidityToLocation
            };

            var firstQuestionQuery = seeds.Select(seed =>
            {
                return SeedNumberToLocation(seed, convertTables);
            });

            var firstQuestion = firstQuestionQuery.Min();
            
            var secondQuestionQuery = seeds.Chunk(2).SelectMany(range =>
            {
                var start = range[0];
                var length = range[1];

                var buffer = new long[length];

                for(var i = 0; i < length; i++)
                {
                    buffer[i] = start + i;
                }
                
                return buffer;
            }).Select(seed => SeedNumberToLocation(seed, convertTables));

            var secondQuestion = secondQuestionQuery.Min();

            ;

            static long SeedNumberToLocation(long start, IReadOnlyList<Range>[] conversionTables)
            {
                for (var i = 0; i < conversionTables.Length - 1; i++)
                {
                    var convertTable = conversionTables[i];

                    var range = convertTable.SingleOrDefault(x => x.SourceRangeStart <= start && start < x.SourceRangeStart + x.Length, null);

                    // If the range is null we dont have to map anything just pass to the next table.
                    if (range != null)
                        start = range.DestinationRangeStart + (start - range.SourceRangeStart);
                }

                var lastRange = conversionTables.Last().SingleOrDefault(x => x.SourceRangeStart <= start && start < x.SourceRangeStart + x.Length, null);
                var location = start;
                if (lastRange != null)
                    location = lastRange.DestinationRangeStart + (start - lastRange.SourceRangeStart);

                return location;
            }
        }
    }

    class Range
    {
        public long DestinationRangeStart { get; set; }

        public long SourceRangeStart { get; set; }

        public long Length { get; set; }
    }
}
