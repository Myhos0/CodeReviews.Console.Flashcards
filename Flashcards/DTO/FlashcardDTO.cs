namespace Flashcards.DTO;
internal class FlashcardDTO
{
    public int DisplayId { get; set; }
    public int IdFlashcard { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int QuestionScore { get; set; }
}