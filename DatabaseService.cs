//using Google.Android.Material.Color.Utilities;

namespace COMP609A2;

public class DatabaseService
{
    private readonly SQLiteConnection _conn;
    public DatabaseService()
    {
        var dbName = "FarmData.db";
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName);
        if (File.Exists(dbPath) == false)
        {
            using Stream stream = Current.OpenAppPackageFileAsync(dbName).Result;
            using MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);
            File.WriteAllBytes(dbPath, memoryStream.ToArray());//Convert the memory contents into a byte[]
        }
        _conn = new SQLiteConnection(dbPath);
    }
    public List<LiveStock> ReadItems()
    {
        var liveStocks = new List<LiveStock>();
        var lst1 = _conn.Table<Cow>().ToList();
        liveStocks.AddRange(lst1);
        var lst2 = _conn.Table<Sheep>().ToList();
        liveStocks.AddRange(lst2);
        return liveStocks;
    }
    public int InsertItem(LiveStock item)
    {
        return _conn.Insert(item);
    }
    public int UpdateItem(LiveStock item)
    {
        return _conn.Update(item);
    }
    public LiveStock FindItem(int id)
    {
        LiveStock item;
        item = _conn.Find<Cow>(id);
        if (item == null)
        {
            item = _conn.Find<Sheep>(id);
        }
        return item;
    }
}
