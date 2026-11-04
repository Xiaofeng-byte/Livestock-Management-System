namespace COMP609A2.Pages;

public partial class ForecastPage : ContentPage
{
    private readonly DatabaseService _dbs;

    public ForecastPage(DatabaseService dbs)
    {
        InitializeComponent();
        _dbs = dbs;
    }

    private async void CalculateButton_Clicked(object sender, EventArgs e)
    {
        if (TypePicker.SelectedItem is not string type)
        {
            await DisplayAlertAsync("Validation", "Please select a livestock type.", "OK");
            return;
        }
        if (!int.TryParse(QuantityInput.Text, out int quantity) || quantity <= 0)
        {
            await DisplayAlertAsync("Validation", "Please enter investment quantity.", "OK");
            return;
        }
        if (!int.TryParse(DaysInput.Text, out int days) || days <= 0)
        {
            await DisplayAlertAsync("Validation", "Please enter a valid number of days (greater than 0).", "OK");
            return;
        }

        var all = _dbs.ReadItemsLS();
        var (avgDailyProfit, totalForecast) = ForecastService.Calculate(all, type, quantity, days);

        AvgProfitLabel.Text = $"Based on Avg Daily {type} Profit of ${avgDailyProfit}";
        ForecastLabel.Text = $"Buying {quantity} {(type == "Cow" ? "cows" : "sheep")} would bring in estimated profit over {days} days: ${totalForecast}";
    }
}
