using Flashcards.DTO;
using Flashcards.Model;
using Flashcards.Repositorys;
using Spectre.Console;

namespace Flashcards.ValidateInputs;

internal class ValidateStackInput
{
    public string ValidateName(string message)
    {
        AnsiConsole.MarkupLine(message);
        string input = Console.ReadLine()?.Trim() ?? "";

        if (input == "0") return "0";

        if (!string.IsNullOrWhiteSpace(input) && input.Count(char.IsLetter) >= 3)
        {
            return input;
        }

        return "[#FF0A0A]Invalid Name.[/][#619B8A]The name must not be empty and must be longer than 3 letters[/] or [#9A031E]0[/] to cancel. Try again:";
    }

    public int ValidateId(string message, List<StackDTO> stacks)
    {
        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (Int32.TryParse(input, out int value))
            {
                if (value == 0) return -1;

                if (stacks.Any(f => f.DisplayId == value))
                {
                    return value;
                }
            }

            message = "[#FF0A0A]Invalid Id.[/]Please write a valid Id, or [#9A031E]0[/] to cancel.";
        }
    }
}