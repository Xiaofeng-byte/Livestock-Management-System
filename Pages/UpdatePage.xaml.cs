namespace COMP609A2.Pages;

public partial class UpdatePage : ContentPage
{
    private readonly DatabaseService _dbs;
    public ObservableCollection<int> Id { get; set; } = [];
    public LiveStock livestock;
    public UpdatePage(DatabaseService dbs)
	{
		InitializeComponent();
		_dbs = dbs;
        BindingContext = this;

        dbs.ReadItemsLS().ForEach(x => Id.Add(x.Id));
    }

    private void IDPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = int.Parse(IDPicker.SelectedItem.ToString());
        var item = _dbs.FindItemLS(id);
        livestock = item;
        TypeUpdate.Text = item.ClassName;
        ExpenseUpdate.Text = item.Expense.ToString();
        WeightUpdate.Text=item.Weight.ToString();
        if (item.ClassName == "Cow")
        {
            Cow cow = (Cow)item;
            ColourPicker.ItemsSource = new List<string>
                {
                    "Black",
                    "Red",
                };
            Production.Text = "Milk";
            ProductionUpdate.Text = cow.Milk.ToString();
        }
        else if(item.ClassName == "Sheep")
        {
            Sheep sheep = (Sheep)item;
            ColourPicker.ItemsSource = new List<string>
                {
                    "Black",
                    "White"
                };
            Production.Text = "Wool";
            ProductionUpdate.Text = sheep.Wool.ToString();
        }
        ColourPicker.SelectedItem = item.Colour;
    }

    private async void UpdateButton_Clicked(object sender, EventArgs e)
    {
        if (GlobalVariables.IsAdmin == false)
        {
            await DisplayAlertAsync("Permission Denied", "You do not have permission to update livestock records.", "OK");
            return;
        }

        int added;
        if (!double.TryParse(ExpenseUpdate.Text, out double expense) || !double.TryParse(WeightUpdate.Text, out double weight)
            || !double.TryParse(ProductionUpdate.Text, out double production))
        {
            await DisplayAlertAsync("Validation", "Please input digitals for expense and weight.", "OK");
            return;
        }
        if (livestock != null && livestock.ClassName == "Cow")
        {
            Cow ls = (Cow)livestock;
            ls.Expense = expense;
            ls.Weight = weight;
            ls.Colour = ColourPicker.SelectedItem.ToString();
            ls.Milk = production;
            added = _dbs.UpdateItemLS(ls);
        }
        else if (livestock != null && livestock.ClassName == "Sheep")
        {
            Sheep ls = (Sheep)livestock;
            ls.Expense = expense;
            ls.Weight = weight;
            ls.Colour = ColourPicker.SelectedItem.ToString();
            ls.Wool = production;
            added = _dbs.UpdateItemLS(ls);
        }
        else
        {
            await DisplayAlertAsync("Error", "Please choose an ID first", "OK");
            return;
        }
            
        
        if (added > 0)
        {
            WeakReferenceMessenger.Default.Send(new DBUpdateMessage(true));
            await DisplayAlertAsync("Success", $"Livestock with ID {livestock.Id} has updated", "OK");
        }
        else
        {
            await DisplayAlertAsync("Error", "This livestock could not be updated", "OK");
        }
    }
}