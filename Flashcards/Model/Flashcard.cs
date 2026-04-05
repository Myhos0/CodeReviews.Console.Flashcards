namespace Flashcards.Model;
internal class Flashcard
{
    public int IdFlashcard { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int QuestionScore { get; set; }
    public int IdStack { get; set; }
}