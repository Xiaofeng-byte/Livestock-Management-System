namespace COMP609A2.Pages;

public partial class ChartPage : ContentPage
{
    private readonly DatabaseService _dbs;
    public ObservableCollection<ChartData> ChartDataLSType { get; set; } = new();
    public ObservableCollection<ChartData> ChartDataLSColour { get; set; } = new();
    public ChartPage(DatabaseService dbs)
	{
		InitializeComponent();
        _dbs = dbs;
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadChartData();
    }
    private void LoadChartData()
    {
        // Retrieve livestock from SQLite
        var livestock = _dbs.ReadItemsLS();

        // Count cows and sheep
        int cowCount = livestock.Count(x => x is Cow);
        int sheepCount = livestock.Count(x => x is Sheep);

        // Update chart data
        ChartDataLSType.Clear();

        ChartDataLSType.Add(new ChartData("Cow", cowCount));
        ChartDataLSType.Add(new ChartData("Sheep", sheepCount));

        int redCount = livestock.Count(x => x.Colour?.ToLower() == "red");
        int blackCount = livestock.Count(x => x.Colour?.ToLower() == "black");
        int whiteCount = livestock.Count(x => x.Colour?.ToLower() == "white");

        // Update colour chart data
        ChartDataLSColour.Clear();

        ChartDataLSColour.Add(new ChartData("Red", redCount));
        ChartDataLSColour.Add(new ChartData("Black", blackCount));
        ChartDataLSColour.Add(new ChartData("White", whiteCount));
    }
    public List<Brush> CustomBrushes { get; set; } = new()
{
    new SolidColorBrush(Colors.Red),
    new SolidColorBrush(Colors.Black),
    new SolidColorBrush(Colors.Gray)
};
}