namespace AOC2024.day2;

public abstract class Day2
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day2/input.txt");
        var reports = input.Select(Report.Parse).ToList();
        Console.WriteLine(reports.Count(it => it.Safe));
        
        Console.WriteLine(reports.Count(it => it.SafeIfDamped));
    }

    private class Report
    {
        private readonly List<int> _levelIntervals = [];
        private readonly List<int> _levels;

        public Report(IEnumerable<int> levels)
        {
            _levels = levels.ToList();
            
            using var enumerator = _levels.GetEnumerator();
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

        public List<Report> Damped()
        {
            var result = new List<Report>();
            for (var i = 0; i < _levels.Count; i++)
            {
                var levels = new List<int>(_levels);
                levels.RemoveAt(i);
                result.Add(new Report(levels));
            }
            return result;
        }
        
        public bool SafeIfDamped => Safe || Damped().Any(it => it.Safe);

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