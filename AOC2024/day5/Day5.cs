
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
        
        Console.WriteLine(
            updates
                .Where(it => !it.IsValid(ruleset))
                .Select(it => it.CorrectOrderForRuleset(ruleset))
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

        public Update CorrectOrderForRuleset(RuleSet ruleSet)
        {
            var pages = _pages.ToArray();
            Array.Sort(pages, ruleSet);
            return new Update(pages.ToList());
        }
    }

    private class RuleSet : IComparer<int>
    {
        public readonly Dictionary<int, HashSet<int>> RulesBySecond;

        public RuleSet(List<PageOrderingRule> rules)
        {
            RulesBySecond = new Dictionary<int, HashSet<int>>();
            foreach (var (first, second) in rules)
            {
                if (!RulesBySecond.ContainsKey(second))
                {
                    RulesBySecond[second] = new HashSet<int>();
                }
                
                RulesBySecond[second].Add(first);
            }
        }

        public int Compare(int x, int y)
        {
            if(x == y) return 0;
            if (RulesBySecond.TryGetValue(x, out var rulesXSecond))
            {
                if (rulesXSecond.Contains(y)) return 1;
            }
            
            if (RulesBySecond.TryGetValue(y, out var rulesYSecond))
            {
                if (rulesYSecond.Contains(x)) return -1;
            }
            return 0;
        }
    }
}