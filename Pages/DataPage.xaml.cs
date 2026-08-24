using System.Net;

namespace COMP609A2.Pages;

public partial class DataPage : ContentPage
{
    public ObservableCollection<LiveStock> LiveStocks { get; set; } = [];
    public DataPage(DatabaseService dbs)
	{
		InitializeComponent();
        BindingContext = this;
        dbs.ReadItems().ForEach(x => LiveStocks.Add(x));
        WeakReferenceMessenger.Default.Register<DBUpdateMessage>(this, (r, m) =>
        {
            //if (m.Value == true) 
            LiveStocks.Clear();
            dbs.ReadItems().ForEach(x => LiveStocks.Add(x));
        }
        );
    }
}