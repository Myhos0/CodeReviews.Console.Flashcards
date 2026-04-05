using Spectre.Console;
using Flashcards.Menus.Enums;
using Flashcards.Repositorys;
using Flashcards.Database;
using Flashcards.Model;
using Flashcards.ValidateInputs;
using Flashcards.DTO;

namespace Flashcards.Menus;
internal class StackMenu
{
    private readonly StackRepository stackRepository;
    private readonly ValidateStackInput validateStackInput = new();
    private readonly FlashcardMenu flashcardMenu;
    public StackMenu(DataBase database)
    {
        stackRepository = new StackRepository(database);
        flashcardMenu = new FlashcardMenu(database);
    }
    public void Menu()
    {
        while (true)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<StackMenuOptions>()
                .Title("Select a option for the stack")
                .AddChoices(Enum.GetValues<StackMenuOptions>()));

            switch (menuOption)
            {
                case StackMenuOptions.Select:
                    SelectStack();
                    break;
                case StackMenuOptions.Create:
                    CreateStack();
                    break;
                case StackMenuOptions.View:
                    ViewStack();
                    break;
                case StackMenuOptions.Update:
                    UpdateStack();
                    break;
                case StackMenuOptions.Delete:
                    DeleteStack();
                    break;
                case StackMenuOptions.Exit:
                    return;
            }
        }
    }

    private void SelectStack()
    {
        List<StackDTO> stacks = GetStacks();

        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(stacks);

        int displayId = validateStackInput.ValidateId("Please insert the [#619B8A]ID[/] of the stack to select or [#9A031E]0[/] to cancel.", stacks);

        if (SelectStack(displayId,stacks) == -1) return;

        flashcardMenu.Menu(stackRepository.GetById(SelectStack(displayId,stacks)));
    }

    private void CreateStack()
    {
        ViewStack();

        string name = validateStackInput.ValidateName("Please insert the name of the [#D3F8E2]Stack[/] [#619B8A](The stacks cannot have the same name)[/] or [#9A031E]0[/] to cancel");

        if (name == "0") return;

        try
        {
            stackRepository.Insert(
                new Stack
                {
                    Name = name,
                });

            AnsiConsole.MarkupLine("[green]Stack created successfully![/]");
        }
        catch(Exception ex) 
        {
            if (ex.Message == "DUPLICATE_NAME")
            {
                AnsiConsole.MarkupLine("[red]A stack with that name already exists.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
            }
        }

        Console.ReadKey();
    }

    private void ViewStack() 
    {
        List<StackDTO> stacks = GetStacks();

        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(stacks);
        Console.ReadKey();
    }

    private List<StackDTO> GetStacks() => stackRepository.GetAll().ToList();

    private void UpdateStack()
    {
        List<StackDTO> stacks = GetStacks();

        if (!stacks.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(stacks);

        int displayId = validateStackInput.ValidateId("Please insert the [#619B8A]ID[/] of the stack to update or [#9A031E]0[/] to cancel.", stacks);

        if (SelectStack(displayId, stacks) == -1) return;

        string newName = validateStackInput.ValidateName("Please insert the name of the [#D3F8E2]Stack[/] [#619B8A](The stacks cannot have the same name)[/] or [#9A031E]0[/] to cancel");

        try
        {
            stackRepository.Update(
                new Stack
                {
                    IdStack = SelectStack(displayId,stacks),
                    Name = newName,
                });
        }
        catch (Exception ex) 
        {
            if (ex.Message == "DUPLICATE_NAME")
            {
                AnsiConsole.MarkupLine("[red]A stack with that name already exists.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
            }
        }

        AnsiConsole.MarkupLine("[green]Stack updated succesfully[/]");
        Console.ReadKey();
    }

    private void DeleteStack()
    {
        List<StackDTO> stacks = GetStacks();

        ShowTable(stacks);

        int displayId = validateStackInput.ValidateId("Please insert the [#619B8A]ID[/] of the stack to delete or [#9A031E]0[/] to cancel.", stacks);

        if (SelectStack(displayId, stacks) == -1) return;

        stackRepository.Delete(SelectStack(displayId,stacks));
        AnsiConsole.MarkupLine("[#4CA97C]Stack delete successfully[/]");
        Console.ReadKey();
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

    private int SelectStack(int displayId,List<StackDTO> stacks) 
    {
        if (displayId == -1) return -1;

        StackDTO selected = stacks.FirstOrDefault(f => f.DisplayId == displayId);

        if (selected == null)
        {
            AnsiConsole.MarkupLine("[red]Flashcard not found.[/]");
            Console.ReadKey();
            return -1;
        }

        return selected.IdStack;
    }
}