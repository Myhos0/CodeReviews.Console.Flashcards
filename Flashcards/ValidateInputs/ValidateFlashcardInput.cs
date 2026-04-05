using Flashcards.DTO;
using Flashcards.Repositorys;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.ValidateInputs;
internal class ValidateFlashcardInput
{
    public string ValidateQuestion(string message) 
    {
        AnsiConsole.MarkupLine(message);
        string input = Console.ReadLine()?.Trim() ?? "";

        if (input == "0") return "0";

        if (!input.All(char.IsDigit) && !string.IsNullOrWhiteSpace(input) && input.Count(char.IsLetter) >= 6) 
        {
            return input;
        }

        return ValidateQuestion("[#FF0A0A]Question not accepted.[/][#619B8A]The question must not be empty or only numbers and must be longer than 6 letters.[/]");
    }

    public string ValidateAnswer(string message) 
    {
        AnsiConsole.MarkupLine(message);
        string input = Console.ReadLine()?.Trim() ?? "";

        if (input == "0") return "0";

        if (!string.IsNullOrWhiteSpace(input)) 
        {
            return input;
        }

        return ValidateAnswer("[#FF0A0A]Answer not accepted.[/][#619B8A]The answer must not be empty[/] press [#9A031E]0[/] to cancel");
    }

    public int ValidateScore(string message) 
    {
        AnsiConsole.MarkupLine(message);
        string input = Console.ReadLine()?.Trim() ?? "";

        if (input == "0") return -1;

        if(int.TryParse(input, out int score)) 
        {
            if(score >=0 && score <= 10) return score;
        }

        return ValidateScore("[#FF0A0A]Invalid score.[/] Please insert a valid score[#619B8A](0-10)[/] or press [#9A031E]0[/] to cancel");
    }

    public int ValidateInt(string message, List<FlashcardDTO> flashcardRespository) 
    {
        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (Int32.TryParse(input, out int value))
            {
                if (value == 0) return -1;

                if (flashcardRespository.Any(f => f.DisplayId == value))
                {
                    return value;
                }
            }

            message = "[#FF0A0A]Invalid Id.[/]Please write a valid Id, or [#9A031E]0[/] to cancel.";
        }
    }
}