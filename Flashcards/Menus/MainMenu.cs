using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Menus.Enums;
using Flashcards.Model;
using Flashcards.Repositorys;
using Flashcards.ValidateInputs;
using Spectre.Console;

namespace Flashcards.Menus;
internal class MainMenu
{
    private readonly StackMenu stackMenu;
    private readonly StudySessionMenu sessionStudyMenu;
    private readonly StackRepository stackRepository;
    private readonly ReportSystemMenu reportSystemMenu;
    private readonly ValidateStackInput validateStackInput = new();

    public MainMenu(DataBase dataBase) 
    {
        sessionStudyMenu = new StudySessionMenu(dataBase);
        stackMenu = new StackMenu(dataBase);
        stackRepository = new StackRepository(dataBase);
        reportSystemMenu = new ReportSystemMenu(dataBase);
    }

    public void Menu()
    {
        bool open = true;

        while (open)
        {
            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<MainMenuOptions>()));

            switch (menuOption)
            {
                case MainMenuOptions.Stack:
                    stackMenu.Menu();
                    break;
                case MainMenuOptions.StudySession:
                    GoSessionMenu();
                    break;
                case MainMenuOptions.ReportSystem:
                    reportSystemMenu.Menu();
                    break;
                case MainMenuOptions.Exit:
                    open = false;
                    break;
            }
        }
    }

    private void GoSessionMenu() 
    {
        List<StackDTO> stacks = stackRepository.GetAll().ToList();

        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(stacks);

        int displayId = validateStackInput.ValidateId("Please insert the [#619B8A]ID[/] of the stack to select or [#9A031E]0[/] to cancel.", stacks);

        if (displayId == -1) return;

        StackDTO selected = stacks.FirstOrDefault(f => f.DisplayId == displayId);

        if (selected == null)
        {
            AnsiConsole.MarkupLine("[red]Flashcard not found.[/]");
            Console.ReadKey();
            return;
        }

        sessionStudyMenu.Menu(stackRepository.GetById(selected.IdStack));
    }

    private void ShowTable(IEnumerable<StackDTO> stacks)
    {
        AnsiConsole.Clear();

        var table = new Table().Border(TableBorder.Rounded)
           .AddColumn("[#619B8A]Id[/]")
           .AddColumn("Name");

        foreach (var s in stacks)
        {
            table.AddRow(
                "[#619B8A]" + s.DisplayId.ToString() + "[/]",
                s.Name
            );
        }

        table.Border(TableBorder.Horizontal);
        table.Expand();
        AnsiConsole.Write(table);
    }
}