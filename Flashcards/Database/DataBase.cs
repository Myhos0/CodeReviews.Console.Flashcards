using Microsoft.Data.SqlClient;

namespace Flashcards.Database;

internal class DataBase
{
    private readonly string _stringConnection;

    public DataBase(string stringConnetion) 
    {
        _stringConnection = stringConnetion; 
    }

    public SqlConnection GetConnection() => new(_stringConnection);
}