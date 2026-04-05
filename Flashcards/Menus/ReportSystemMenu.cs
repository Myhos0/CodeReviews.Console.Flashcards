using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Menus.Enums;
using Flashcards.Repositorys;
using Flashcards.ValidateInputs;
using Spectre.Console;

namespace Flashcards.Menus;
internal class ReportSystemMenu
{
    private readonly ValidateReportSystemInput validateReportSystemInput = new();
    private readonly ReportSystemRepository reportSystemRepository;

    public ReportSystemMenu(DataBase database) 
    {
        reportSystemRepository = new ReportSystemRepository(database);
    }

    public void Menu() 
    {
        while (true)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<ReportSystemMenuOptions>()
                .Title("Select an Option")
                .AddChoices(Enum.GetValues<ReportSystemMenuOptions>()));

            switch (menuOption) 
            {
                case ReportSystemMenuOptions.SessionPerMonth:
                    SessionsPerMonth();
                    break;
                case ReportSystemMenuOptions.AverageScorePerMonth:
                    AveragePerMonth();
                    break;
                case ReportSystemMenuOptions.Exit:
                    return;
            }
        }
    }

    private void SessionsPerMonth() 
    {
        int year = validateReportSystemInput.ValidateYear("Please write the year[#619B8A](2023-2026)[/] or [#9A031E]0[/] to cancel");

        if (year == -1) return;

        ShowTable(reportSystemRepository.GetSessionsPerMotnh(year));
        
        Console.ReadKey();
    }

    public void AveragePerMonth() 
    {
        int year = validateReportSystemInput.ValidateYear("Please write the year[#619B8A](2023-2026)[/] or [#9A031E]0[/] to cancel");

        if (year == -1) return;

        ShowTable(reportSystemRepository.GetAveragePerMonth(year));

        Console.ReadKey();
    }

    private void ShowTable(IEnumerable<ReportDTO> reports) 
    {
        string[] montths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        var table = new Table().Border(TableBorder.Rounded)
            .AddColumn("StackName")
            .AddColumns(montths);
            
        foreach (var row in reports)
        {
            table.AddRow(
                row.StackName,
                (row.January ?? 0).ToString(),
                (row.February ?? 0).ToString(),
                (row.March ?? 0).ToString(),
                (row.April ?? 0).ToString(),
                (row.May ?? 0).ToString(),
                (row.June ?? 0).ToString(),
                (row.July ?? 0).ToString(),
                (row.August ?? 0).ToString(),
                (row.September ?? 0).ToString(),
                (row.October ?? 0).ToString(),
                (row.November ?? 0).ToString(),
                (row.December ?? 0).ToString()
            );
        }

        table.Border(TableBorder.Horizontal);
        table.Expand();

        AnsiConsole.Write(table);
    }
}