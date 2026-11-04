using System.ComponentModel;
using System.Net;

namespace COMP609A2.Pages;

public partial class DataPage : ContentPage
{
    private readonly DatabaseService _dbs;
    public ObservableCollection<LiveStock> LiveStocks { get; set; } = [];
    public DataPage(DatabaseService dbs)
	{
		InitializeComponent();

        BindingContext = this;
        _dbs = dbs;
        dbs.ReadItemsLS().ForEach(x => LiveStocks.Add(x));
        WeakReferenceMessenger.Default.Register<DBUpdateMessage>(this, (r, m) =>
        {
            //if (m.Value == true) 
            LiveStocks.Clear();
            dbs.ReadItemsLS().ForEach(x => LiveStocks.Add(x));
        }
        );
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if(GlobalVariables.IsAdmin == false)
        {
            await DisplayAlertAsync("Permission Denied", "You do not have permission to delete livestock records.", "OK");
            return;
        }
        if (sender is Button button && button.BindingContext is LiveStock livestock)
        {
            bool result = await DisplayAlertAsync(
                "Delete",
                $"Are you sure you want to delete this livestock ID {livestock.Id} ?",
                "Yes",
                "No");
            if (result)
            {
                int deleted = _dbs.DeleteItemLS(livestock);
                if (deleted > 0)
                {
                    WeakReferenceMessenger.Default.Send(new DBUpdateMessage(true));
                    await DisplayAlertAsync("Success", $"Livestock with ID {livestock.Id} deleted", "OK");
                }
            }
        }
    }
}