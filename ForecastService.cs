namespace COMP609A2;

// Static utility class: computes Feature 6 profit forecast from the current livestock list.
public static class ForecastService
{
    // type: "Cow" | "Sheep"
    public static (double avgDailyProfit, double totalForecast) Calculate(List<LiveStock> all, string type, int quantity, int days)
    {
        var group = type.Equals("Cow", StringComparison.OrdinalIgnoreCase)
            ? all.OfType<Cow>().Cast<LiveStock>().ToList()
            : all.OfType<Sheep>().Cast<LiveStock>().ToList();

        if (group.Count == 0) return (0, 0);

        var avgDailyProfit = Math.Round(group.Sum(a => a.Profit) / group.Count, 2);
        var totalForecast = Math.Round(avgDailyProfit * quantity * days, 2);
        return (avgDailyProfit, totalForecast);
    }
}
