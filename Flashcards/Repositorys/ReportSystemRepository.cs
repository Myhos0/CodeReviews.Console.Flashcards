using Dapper;
using Flashcards.Database;
using Flashcards.DTO;

namespace Flashcards.Repositorys;
internal class ReportSystemRepository
{
    private readonly DataBase _dataBase;

    public ReportSystemRepository(DataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public IEnumerable<ReportDTO> GetSessionsPerMotnh(int year)
    {
        const string SQL = @"SELECT
                            StackName,
                            ISNULL([1],0) AS January,
                            ISNULL([2],0) AS February,
                            ISNULL([3],0) AS March,
                            ISNULL([4],0) AS April,
                            ISNULL([5],0) AS May,
                            ISNULL([6],0) AS June,
                            ISNULL([7],0) AS July,
                            ISNULL([8],0) AS August,
                            ISNULL([9],0) AS September,
                            ISNULL([10],0) AS October,
                            ISNULL([11],0) AS November,
                            ISNULL([12],0) AS December
                            FROM (SELECT s.Name AS StackName, MONTH(ss.Date) AS MonthName, ss.IdStudySession
                            FROM StudySession ss JOIN Stack s ON ss.IdStack = s.IdStack WHERE YEAR(ss.Date) = @Year)
                            AS SourceTable PIVOT
                            ( COUNT(IdStudySession)
                                FOR MonthName IN (
                                [1],[2],[3],[4],[5],[6], [7],[8],[9],[10],[11],[12])
                            ) AS PivotTable
                            ORDER BY StackName;";

        using var connection = _dataBase.GetConnection();
        return connection.Query<ReportDTO>(SQL, new { Year = year });
    }

    public IEnumerable<ReportDTO> GetAveragePerMonth(int year) 
    {
        const string SQL = @"SELECT
                            StackName,
                            ISNULL([1],0) AS January,
                            ISNULL([2],0) AS February,
                            ISNULL([3],0) AS March,
                            ISNULL([4],0) AS April,
                            ISNULL([5],0) AS May,
                            ISNULL([6],0) AS June,
                            ISNULL([7],0) AS July,
                            ISNULL([8],0) AS August,
                            ISNULL([9],0) AS September,
                            ISNULL([10],0) AS October,
                            ISNULL([11],0) AS November,
                            ISNULL([12],0) AS December
                            FROM
                                (
                                    SELECT s.Name AS StackName, MONTH(ss.Date) AS MonthNumber, ss.TotalScore
                                    FROM StudySession ss JOIN Stack s ON ss.IdStack = s.IdStack
                                    WHERE YEAR(ss.Date) = @Year
                                ) AS SourceTable
                                PIVOT
                                (
                                    AVG(TotalScore)
                                    FOR MonthNumber IN (
                                        [1],[2],[3],[4],[5],[6],
                                        [7],[8],[9],[10],[11],[12]
                                    )
                                ) AS PivotTable
                                ORDER BY StackName;";

        var connection = _dataBase.GetConnection();
        return connection.Query<ReportDTO>(SQL, new { Year = year });
    }
}