using Dapper;
using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Model;

namespace Flashcards.Repositorys;
internal class StudySessionRepository
{
    private readonly DataBase _dataBase;

    public StudySessionRepository(DataBase dataBase) 
    {
        _dataBase = dataBase;
    }

    public IEnumerable<StudySessionDTO> GetAll() 
    {
        const string SQL = @"SELECT ss.Date, ss.TotalScore, s.Name AS StackName FROM StudySession ss JOIN Stack s ON ss.IdStack = s.IdStack";

        using var connection = _dataBase.GetConnection();
        return connection.Query<StudySessionDTO>(SQL);
    }

    public IEnumerable<StudySessionDTO> GetSessionsByStack(int idStack) 
    {
        const string SQL = @"SELECT ss.Date, ss.TotalScore, s.Name AS StackName FROM StudySession ss JOIN Stack s ON ss.IdStack = s.IdStack WHERE ss.IdStack = @IdStack";

        using var connection = _dataBase.GetConnection();
        return connection.Query<StudySessionDTO>(SQL, new { IdStack = idStack});
    }

    public void Insert(StudySession sessionStudy) 
    {
        const string SQL = @"INSERT INTO StudySession( Date, TotalScore, IdStack) VALUES ( @Date, @TotalScore, @IdStack)";

        using var connection = _dataBase.GetConnection();
        connection.Execute(SQL, sessionStudy);
    }
}