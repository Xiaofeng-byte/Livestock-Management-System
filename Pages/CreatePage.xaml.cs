namespace COMP609A2.Pages;

public partial class CreatePage : ContentPage
{
    private readonly DatabaseService _dbs;
    public List<string> LivestockTypes { get; set; } = ["Cow", "Sheep"];

    public CreatePage(DatabaseService dbs)
	{
		InitializeComponent();
        _dbs = dbs;
        BindingContext = this;
	}

    private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(TypePicker.SelectedItem is string selectedType)
        {
            if(selectedType == "Cow")
            {
                ColourPicker.ItemsSource = new List<string>
                {
                    "Black",
                    "Red",
                };
                Production.Text = "Milk";
            }
            else if(selectedType == "Sheep")
            {
                ColourPicker.ItemsSource = new List<string>
                {
                    "Black",
                    "White"
                };
                Production.Text = "Wool";
            }
        }
    }

    private async void CreateButton_Clicked(object sender, EventArgs e)
    {
        if (GlobalVariables.IsAdmin == false)
        {
            await DisplayAlertAsync("Permission Denied", "You do not have permission to create livestock records.", "OK");
            return;
        }
        if (TypePicker.SelectedItem is not string livestockType)
        {
            await DisplayAlertAsync("Validation", "Please select a livestock type.", "OK");
            return;
        }
        if (!double.TryParse(ExpenseInput.Text, out double expense) || !double.TryParse(WeightInput.Text, out double weight)
            || !double.TryParse(ProductionInput.Text,out double production))
        {
            await DisplayAlertAsync("Validation", "Please input digitals for expense and weight.", "OK");
            return;
        }
        
        LiveStock ls;
        if (livestockType == "Cow")
        {

            ls = new Cow
            {
                Expense = expense,
                Weight = weight,
                Colour = ColourPicker.SelectedItem.ToString(),
                Milk = production
            };
        }
        else if (livestockType == "Sheep")
        {
            ls = new Sheep
            {
                Expense = expense,
                Weight = weight,
                Colour = ColourPicker.SelectedItem.ToString(),
                Wool = production
            };
        }
        else
            return;
        int added = _dbs.InsertItemLS(ls);
        if (added > 0)
        {
            WeakReferenceMessenger.Default.Send(new DBUpdateMessage(true));
            await DisplayAlertAsync("Success", $"Added {livestockType} with ID {ls.Id}.", "OK");
            TypePicker.SelectedItem = null;
            ExpenseInput.Text = null;
            WeightInput.Text = null;
            ProductionInput.Text = null;
            ColourPicker.SelectedItem = null;
        }
        else
        {
            await DisplayAlertAsync("Error", "This livestock could not be added", "OK");
        }
    }
}