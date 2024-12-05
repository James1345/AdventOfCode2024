namespace AOC2024.day2;

public abstract class Day2
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day2/input.txt");
        var reports = input.Select(Report.Parse);
        Console.WriteLine(reports.Count(it => it.Safe));
    }

    private class Report
    {
        private readonly List<int> _levelIntervals = [];

        public Report(IEnumerable<int> levels)
        {
            using var enumerator = levels.GetEnumerator();
            enumerator.MoveNext();
            var current = enumerator.Current;
            while (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                _levelIntervals.Add(next - current);
                current = next;
            }
        }

        public static Report Parse(string input)
        {
            return new Report(input.Split(' ').Select(int.Parse));
        }

        public bool Safe => SingleDirection && DifferenceIsSafe;

        private bool SingleDirection => _levelIntervals.All(it => it > 0) || _levelIntervals.All(it => it < 0);

        private bool DifferenceIsSafe
        {
            get
            {
                var intervalMagnitudes = _levelIntervals.Select(Math.Abs).ToList();

                return intervalMagnitudes.Max() <= 3 && intervalMagnitudes.Min() >= 1;
            }
        }
    }
}