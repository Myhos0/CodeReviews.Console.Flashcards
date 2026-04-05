using Spectre.Console;

namespace Flashcards.ValidateInputs;
internal class ValidateStudySessionInput
{
    public bool ValidateAnswerInt(int correctAnswer, string input) => correctAnswer == int.Parse(input);
    public bool ValidateAnswerString(string correctAsnwer, string input) => correctAsnwer.Replace(" ","") == input.Replace(" ","");
}