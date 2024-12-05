
namespace AOC2024.day5;

public abstract class Day5
{
    public static void Invoke()
    {
        var (ruleset, updates) = Parse("day5/input.txt");

        Console.WriteLine(
            updates
                .Where(it => it.IsValid(ruleset))
                .Sum(it => it.MiddleValue)
            );

    }
    
    private static (RuleSet, List<Update>) Parse(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var parsedRules = false;
        var updates = new List<Update>();
        var rules = new List<PageOrderingRule>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parsedRules = true;
                continue;
            }

            if (!parsedRules)
            {
                var ruleValues = line.Split('|').Select(int.Parse).ToList();
                rules.Add(new PageOrderingRule(ruleValues.First(), ruleValues.Last()));
            }
            else
            {
                updates.Add(new Update(line.Split(',').Select(int.Parse).ToList()));
            }
        }
        return (new RuleSet(rules), updates);
    }

    private record PageOrderingRule(int First, int Last);

    private class Update
    {
        public readonly int MiddleValue;
        private readonly List<int> _pages;

        public Update(List<int> pages)
        {
            _pages = pages;
            MiddleValue = pages[(_pages.Count - 1)/2];
        }

        public bool IsValid(RuleSet ruleSet)
        {
            var pagesMustPreceed = new HashSet<int>();
            foreach (var pageNumber in _pages)
            {
                if (pagesMustPreceed.Contains(pageNumber))
                {
                    return false;
                }
                pagesMustPreceed.UnionWith(ruleSet.RulesBySecond.TryGetValue(pageNumber, out var value) ? value: []);
            }
            return true;
        }

        
    }

    private class RuleSet
    {
        public readonly Dictionary<int, HashSet<int>> RulesByFirst;
        public readonly Dictionary<int, HashSet<int>> RulesBySecond;

        public RuleSet(List<PageOrderingRule> rules)
        {
            RulesByFirst = new Dictionary<int, HashSet<int>>();
            RulesBySecond = new Dictionary<int, HashSet<int>>();
            foreach (var (first, second) in rules)
            {
                if (!RulesByFirst.ContainsKey(first))
                {
                    RulesByFirst[first] = new HashSet<int>();
                }
                RulesByFirst[first].Add(second);

                if (!RulesBySecond.ContainsKey(second))
                {
                    RulesBySecond[second] = new HashSet<int>();
                }
                RulesBySecond[second].Add(first);
            }
        }
    }
}