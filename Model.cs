using System.ComponentModel;

namespace COMP609A2;

public abstract class LiveStock
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public double Expense { get; set; }
    public double Weight { get; set; }
    public string? Colour { get; set; }
    public abstract double Income{ get; }
    public abstract double Cost { get; }
    public abstract double Profit { get; }
    public abstract string? ClassName { get; }
}
[Table("Cow")]
public class Cow : LiveStock
{
    public double Milk { get; set; }
    public override double Income
    {
        get { return Math.Round(Milk * AppConstants.MILKPRICE,2); }
    }
    public override double Cost
    {
        get { return Math.Round(base.Expense * (1 + AppConstants.TAX), 2); }
    }
    public override double Profit
    {
        get { return (Income - Cost); }
    }
    public override string? ClassName
    {
        get { return GetType().Name; }
    }
}
[Table("Sheep")]
public class Sheep : LiveStock
{
    public double Wool { get; set; }
    public override double Income
    {
        get { return Math.Round(Wool * AppConstants.MILKPRICE,2); }
    }
    public override double Cost
    {
        get { return Math.Round(base.Expense * (1 + AppConstants.TAX), 2); }
    }
    public override double Profit
    {
        get { return (Income - Cost); }
    }
    public override string? ClassName
    {
        get { return GetType().Name; }
    }
}

public class Users
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100)]
    public string? Username { get; set; }
    [PasswordPropertyText(true),MaxLength(15)]
    public string? Password { get; set; }
    public Boolean IsAdmin { get; set; }
}