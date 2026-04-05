using Spectre.Console;

namespace Flashcards.ValidateInputs;

internal class ValidateReportSystemInput
{
    public int ValidateYear(string message)
    {
        int año;

        AnsiConsole.MarkupLine(message);
        string input = Console.ReadLine()?.Trim() ?? "";

        if(int.TryParse(input, out año) && input.Length == 4) 
        {
            if (año >= 2023 && año <= 2026) return año;
        }

        return ValidateYear("[#FF0A0A]Invalid year.[/]Please write a valid year, or [#9A031E]0[/] to cancel.");
    }
}