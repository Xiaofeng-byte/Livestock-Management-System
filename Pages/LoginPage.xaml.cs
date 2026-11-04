using System.Net;

namespace COMP609A2.Pages;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _dbs;
    public ObservableCollection<int> Id { get; set; } = [];
    public Users users;
    public LoginPage(DatabaseService dbs)
    {
        InitializeComponent();
        UsernameEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;
        _dbs = dbs;
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (GlobalVariables.IsLoggedIn)
        {
            UsernameEntry.Text = GlobalVariables.CurrentUser;
            PasswordEntry.Text = string.Empty;
            IsAdmin.SelectedIndex = GlobalVariables.IsAdmin ? 1 : 0;
        }
        else
        {
            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            IsAdmin.SelectedIndex = 0;
        }
    }
    private async void Login_Clicked(object sender, EventArgs e)
    {
        if (GlobalVariables.IsLoggedIn == false)
        {
            string? username = UsernameEntry.Text?.Trim();
            string? password = PasswordEntry.Text?.Trim();
            var user = _dbs.FindItemUser(username, password);
            if (user != null)
            {
                // Login successful
                GlobalVariables.CurrentUser = user.Username;
                GlobalVariables.UserID = user.Id;
                GlobalVariables.IsLoggedIn = true;
                GlobalVariables.IsAdmin = user.IsAdmin;
                await DisplayAlertAsync("Login Successful", "You have been logged in.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                // Login failed
                await DisplayAlertAsync("Login Failed", "Invalid username or password.", "OK");
            }
        }
    }
    private async void Logout_Clicked(object sender, EventArgs e)
    {
        if(GlobalVariables.IsLoggedIn)
        {
            GlobalVariables.CurrentUser = null;
            GlobalVariables.UserID = -1;
            GlobalVariables.IsLoggedIn = false;
            GlobalVariables.IsAdmin = false;
            await DisplayAlertAsync("Logout Successful", "You have been logged out.", "OK");
            // Navigate to the login page
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
    private async void Signin_Clicked(object sender, EventArgs e)
    {
        string? username = UsernameEntry.Text?.Trim();
        string? password = PasswordEntry.Text?.Trim();
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Validation", "Please enter both username and password.", "OK");
            return;
        }

        var newUser = new Users
        {
            Username = username,
            Password = password,
            IsAdmin = IsAdmin.SelectedIndex == 1 
        };
        var user = _dbs.FindItemUser(username, password);
        if (user != null)
        {
            await DisplayAlertAsync("Sign-in Failed", "Username already exists.", "OK");
            return;
        }
        _dbs.InsertItemUser(newUser);
        var existingUser = _dbs.FindItemUser(username, password);
        if (existingUser != null)
        {
            GlobalVariables.CurrentUser = existingUser.Username;
            GlobalVariables.UserID = existingUser.Id;
            GlobalVariables.IsLoggedIn = true;
            GlobalVariables.IsAdmin = existingUser.IsAdmin;
        }
        
        await DisplayAlertAsync("Sign-in Successful", "You have been signed in.", "OK");
        // Navigate to the sign-in page
        await Shell.Current.GoToAsync("//MainPage");
    }
}