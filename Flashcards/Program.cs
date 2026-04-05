using Flashcards.Database;
using Flashcards.Menus;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

class Program
{
    public static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        DataBase db = new DataBase(connectionString);
        
        MainMenu menu = new MainMenu(db);

        menu.Menu();
    }
}