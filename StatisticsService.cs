namespace COMP609A2;

public class DailyProfitBreakdown
{
    public int CowCount { get; set; }
    public int SheepCount { get; set; }
    public double TotalDailyCowProfit { get; set; }
    public double TotalDailySheepProfit { get; set; }
    public double AvgDailyCowProfit { get; set; }
    public double AvgDailySheepProfit { get; set; }
}

public class LivestockStats
{
    public int Count { get; set; }
    public double Percentage { get; set; }
    public double TotalTax { get; set; }
    public double TotalProfit { get; set; }
    public double AverageWeight { get; set; }
    public double TotalMilk { get; set; }
    public double TotalWool { get; set; }
    public double TotalProduce { get; set; }
    public DailyProfitBreakdown? Breakdown { get; set; }
}

// Static utility class: computes Feature 5 statistics from the current livestock list.
public static class StatisticsService
{
    // type: "All" | "Cow" | "Sheep"   colour: "All" | "Black" | "Red" | "White"
    public static LivestockStats Calculate(List<LiveStock> all, string type, string colour, int days)
    {
        var filtered = all.Where(a =>
                (type.Equals("All", StringComparison.OrdinalIgnoreCase) || a.ClassName!.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
                (colour.Equals("All", StringComparison.OrdinalIgnoreCase) || (a.Colour?.Equals(colour, StringComparison.OrdinalIgnoreCase) ?? false)))
            .ToList();

        var milkTotal = filtered.OfType<Cow>().Sum(c => c.Milk) * days;
        var woolTotal = filtered.OfType<Sheep>().Sum(s => s.Wool) * days;

        var stats = new LivestockStats
        {
            Count = filtered.Count,
            Percentage = all.Count == 0 ? 0 : Math.Round((double)filtered.Count / all.Count * 100, 2),
            TotalTax = Math.Round(filtered.Sum(a => a.Weight * AppConstants.TAX) * days, 2),
            TotalProfit = Math.Round(filtered.Sum(a => a.Profit) * days, 2),
            AverageWeight = filtered.Count == 0 ? 0 : Math.Round(filtered.Sum(a => a.Weight) / filtered.Count, 2),
            TotalMilk = Math.Round(milkTotal, 2),
            TotalWool = Math.Round(woolTotal, 2),
            TotalProduce = Math.Round(milkTotal + woolTotal, 2)
        };

        // Daily Profit Breakdown only applies to the All/All selection
        if (type.Equals("All", StringComparison.OrdinalIgnoreCase) && colour.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            var cows = all.OfType<Cow>().ToList();
            var sheep = all.OfType<Sheep>().ToList();
            var totalCowProfit = Math.Round(cows.Sum(c => c.Profit), 2);
            var totalSheepProfit = Math.Round(sheep.Sum(s => s.Profit), 2);

            stats.Breakdown = new DailyProfitBreakdown
            {
                CowCount = cows.Count,
                SheepCount = sheep.Count,
                TotalDailyCowProfit = totalCowProfit,
                TotalDailySheepProfit = totalSheepProfit,
                AvgDailyCowProfit = cows.Count == 0 ? 0 : Math.Round(totalCowProfit / cows.Count, 2),
                AvgDailySheepProfit = sheep.Count == 0 ? 0 : Math.Round(totalSheepProfit / sheep.Count, 2)
            };
        }

        return stats;
    }
}
