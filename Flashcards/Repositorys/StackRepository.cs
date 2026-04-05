using Dapper;
using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Model;
using Microsoft.Data.SqlClient;

namespace Flashcards.Repositorys;
internal class StackRepository
{
    private readonly DataBase _dataBase;

    public StackRepository(DataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public void Insert(Stack stack) 
    {
        const string SQL = @"INSERT INTO Stack (Name) VALUES (@Name)";

        using var connection = _dataBase.GetConnection();

        try
        {
            connection.Execute(SQL, stack);
        }
        catch(SqlException ex) 
        {
            if(ex.Number == 2627 || ex.Number == 2601) 
            {
                throw new Exception("DUPLICATE_NAME");
            }

            throw;
        }
    }

    public IEnumerable<StackDTO> GetAll() 
    {
        const string SQL = @"SELECT ROW_NUMBER() OVER (ORDER BY IdStack) AS DisplayId,IdStack, Name FROM Stack ORDER BY IdStack ASC;";

        using var connection = _dataBase.GetConnection();
        return connection.Query<StackDTO>(SQL);
    }

    public void Update(Stack stack) 
    {
        const string SQL = @"UPDATE Stack SET Name = @Name WHERE IdStack = @IdStack";

        using var connection = _dataBase.GetConnection();

        try
        {
            int rows = connection.Execute(SQL, stack);

            if (rows == 0)
                throw new Exception("No record found to update");
        }
        catch (SqlException ex) 
        {
            if (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("DUPLICATE_NAME");
            }

            throw;
        }
    }

    public void Delete(int id) 
    {
        const string SQL = @"DELETE FROM Stack WHERE IdStack = @Id";

        using var connection = _dataBase.GetConnection();
        int rows = connection.Execute(SQL, new { Id = id });

        if (rows == 0)
            throw new Exception("No records Found.");
    }

    public Stack GetById(int id) 
    {
        const string SQL = @"SELECT IdStack,Name FROM Stack WHERE IdStack = @Id";

        using var connection = _dataBase.GetConnection();

        return connection.QueryFirstOrDefault<Stack>(SQL, new { Id = id} );
    }
}