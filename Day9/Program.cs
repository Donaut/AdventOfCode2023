namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("./input.txt");

            //lines = """
            //    0 3 6 9 12 15
            //    1 3 6 10 15 21
            //    10 13 16 21 30 45
            //    """.Split("\r\n");

            var query = lines.Select(x =>
            {
                var innerList = new List<List<int>>();
                var parent = x.Split(' ').Select(int.Parse).ToList();
                innerList.Add(parent);
                while (!parent.All(x => x == 0))
                {
                    var child = new List<int>();
                    for (var i = 1; i < parent.Count; i++)
                        child.Add(parent[i] - parent[i - 1]);
                    innerList.Add(child);
                    parent = child;
                }
                
                return innerList;
            });

            var firstQuestion = query.Select(x =>
            {
                var prevValue = 0;
                for (var i = x.Count - 2; i >= 0; i--)
                {
                    var innerList = x[i];

                    var last = innerList[innerList.Count - 1];

                    prevValue = prevValue + last;
                }

                return prevValue;
            }).Sum();

            var secondQuestion = query.Select(x =>
            {
                var prevValue = 0;

                for (var i = x.Count - 2; i >= 0; i--)
                {
                    var innerList = x[i];

                    var first = innerList[0];

                    prevValue = first - prevValue;
                }

                return prevValue;
            }).Sum();

            ;
            //lists.Add();
        }
    }
}
