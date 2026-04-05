using Dapper;
using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Model;

namespace Flashcards.Repositorys;

internal class FlashcardRespository
{
    private readonly DataBase _database;

    public FlashcardRespository(DataBase dataBase) 
    {
        _database = dataBase;
    }

    public void Insert(Flashcard flashcard) 
    {
        const string SQl = @"INSERT INTO Flashcard (Question, Answer, QuestionScore, IdStack) VALUES (@Question, @Answer, @QuestionScore, @IdStack)";

        using var connnection = _database.GetConnection();
        connnection.Execute(SQl,flashcard);
    }

    public IEnumerable<FlashcardDTO> GetAll(int idStack) 
    {
        const string SQL = @"SELECT ROW_NUMBER() OVER (ORDER BY IdFlashcard) AS DisplayId,IdFlashcard, Question, Answer, QuestionScore FROM Flashcard WHERE IdStack = @IdStack";

        using var connection = _database.GetConnection();
        return connection.Query<FlashcardDTO>(SQL, new { IdStack = idStack });
    }

    public IEnumerable<StudySessionFlashcardDTO> GetFlashcardsByStack(int id)
    {
        const string SQL = @"SELECT Question, Answer, QuestionScore FROM Flashcard WHERE IdStack = @Id";

        using var connection = _database.GetConnection();
        return connection.Query<StudySessionFlashcardDTO>(SQL, new { Id = id });
    }

    public void Delete(int idFlashcard)
    {
        const string SQL = @"DELETE FROM Flashcard WHERE IdFlashcard = @Id";

        var connnection = _database.GetConnection();
        connnection.Execute(SQL, new { Id = idFlashcard });
    }

    public void Update(Flashcard flashcard) 
    {
        const string SQL = @"UPDATE Flashcard SET Question = @Question, Answer = @Answer, QuestionScore = @QuestionScore WHERE IdFlashcard = @IdFlashcard";

        var connnection = _database.GetConnection();
        connnection.Execute(SQL,flashcard);
    }
}