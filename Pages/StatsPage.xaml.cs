namespace COMP609A2.Pages;

public partial class StatsPage : ContentPage
{
    private readonly DatabaseService _dbs;

    public StatsPage(DatabaseService dbs)
    {
        InitializeComponent();
        _dbs = dbs;

        // Keep results in sync if records change on another tab while this page is open
        WeakReferenceMessenger.Default.Register<DBUpdateMessage>(this, (r, m) =>
        {
            if (TypePicker.SelectedItem is string && ColourPicker.SelectedItem is string && !string.IsNullOrWhiteSpace(DaysInput.Text))
            {
                Calculate();
            }
        });
    }

    private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var previousColour = ColourPicker.SelectedItem as string;

        if (TypePicker.SelectedItem is not string selectedType)
            return;

        List<string> colours = selectedType switch
        {
            "Cow" => ["All", "Black", "Red"],
            "Sheep" => ["All", "Black", "White"],
            _ => ["All", "Black", "Red", "White"]
        };
        ColourPicker.ItemsSource = colours;

        // Keep the previous colour selected if it's still valid for the new type, otherwise reset to "All"
        ColourPicker.SelectedItem = colours.Contains(previousColour!) ? previousColour : "All";
    }

    private async void CalculateButton_Clicked(object sender, EventArgs e)
    {
        if (TypePicker.SelectedItem is not string || ColourPicker.SelectedItem is not string)
        {
            await DisplayAlertAsync("Validation", "Please select a livestock type and colour.", "OK");
            return;
        }
        if (!int.TryParse(DaysInput.Text, out int days) || days <= 0)
        {
            await DisplayAlertAsync("Validation", "Please enter a valid number of days (greater than 0).", "OK");
            return;
        }

        Calculate();
    }

    private void Calculate()
    {
        string type = (string)TypePicker.SelectedItem;
        string colour = (string)ColourPicker.SelectedItem;
        int.TryParse(DaysInput.Text, out int days);

        var all = _dbs.ReadItems();
        var stats = StatisticsService.Calculate(all, type, colour, days);

        CountLabel.Text = $"Number of selected livestock ({type} type in {colour} colour): {stats.Count}";
        PercentageLabel.Text = $"Percentage of total livestock: {stats.Percentage}%";
        TaxLabel.Text = $"Tax over {days} days: ${stats.TotalTax}";
        ProfitLabel.Text = $"Profit over {days} days: ${stats.TotalProfit}";
        AvgWeightLabel.Text = $"Average weight of selected livestock: {stats.AverageWeight}kg";
        ProduceLabel.Text = $"Total produce over {days} days: {stats.TotalProduce}kg";
        MilkLabel.Text = stats.TotalMilk > 0 ? $"- Milk (from Cows): {stats.TotalMilk}kg" : "";
        WoolLabel.Text = stats.TotalWool > 0 ? $"- Wool (from Sheep): {stats.TotalWool}kg" : "";

        if (stats.Breakdown is not null)
        {
            var b = stats.Breakdown;
            BreakdownSection.IsVisible = true;
            BreakdownHeaderLabel.Text = $"Daily Profit Breakdown ({b.CowCount} cows, {b.SheepCount} sheep)";
            TotalCowProfitLabel.Text = $"- Total daily cow profit: ${b.TotalDailyCowProfit}";
            TotalSheepProfitLabel.Text = $"- Total daily sheep profit: ${b.TotalDailySheepProfit}";
            AvgCowProfitLabel.Text = $"- Average daily cow profit: ${b.AvgDailyCowProfit}";
            AvgSheepProfitLabel.Text = $"- Average daily sheep profit: ${b.AvgDailySheepProfit}";
        }
        else
        {
            BreakdownSection.IsVisible = false;
        }
    }
}
