namespace Flashcards.Model;
internal class StudySession
{
    public int IdSessionStudy { get; set; }
    public DateTime Date { get; set; }
    public int TotalScore { get; set; }
    public int IdStack { get; set; }
}