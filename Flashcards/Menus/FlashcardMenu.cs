using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Menus.Enums;
using Flashcards.Model;
using Flashcards.Repositorys;
using Flashcards.ValidateInputs;
using Spectre.Console;

namespace Flashcards.Menus;
internal class FlashcardMenu
{
    private readonly ValidateFlashcardInput validateFlashcardInput = new();
    private readonly FlashcardRespository flashcardRespository;
    private Stack _stack;

    public FlashcardMenu(DataBase database) 
    {
        flashcardRespository = new FlashcardRespository(database);
    }

    public void Menu(Stack stack) 
    {
        _stack = stack;

        while (true)
        {
            Console.Clear();

            var text = new Markup($"Current Stack: [#D3F8E2]{_stack.Name}[/]");
            AnsiConsole.Write(Align.Right(text));

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<FlashcardMenuOptions>()
                .Title("Select an option for the [#7FD1B9]FlashCards[/]")
                .AddChoices(Enum.GetValues<FlashcardMenuOptions>()));

            switch (menuOption) 
            {
                case FlashcardMenuOptions.Create:
                    Create();
                    break;
                case FlashcardMenuOptions.View:
                    View();
                    break;
                case FlashcardMenuOptions.Update:
                    Update();
                    break;
                case FlashcardMenuOptions.Delete:
                    Delete();
                    break;
                case FlashcardMenuOptions.Exit:
                    return;
            }
        }
    }

    public void Create() 
    {
        List<string> values = GetUserInput(
            "Please insert a question.[#619B8A]The question must not be empty or only numbers and must be longer than 6 letters.[/] press [#9A031E]0[/] to cancel",
            "Please insert an asnwer.[#619B8A]The answer must not be empty[/] press [#9A031E]0[/] to cancel",
            "Please insert a score to the question [#619B8A](0-10)[/] or press [#9A031E]0[/] to cancel");

        if (values.Exists(v => v == "0" || v == "-1")) return;

        flashcardRespository.Insert(
            new Flashcard
            {
                Question = values[0],
                Answer = values[1],
                QuestionScore = Int32.Parse(values[2]),
                IdStack = _stack.IdStack
            });

        AnsiConsole.MarkupLine("[green]Flashcard added successfully[/]");
        Console.ReadKey();
    }

    public void View()
    {
        IEnumerable<FlashcardDTO> flashcards = flashcardRespository.GetAll(_stack.IdStack);

        if(!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No stacks found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(flashcards);
        Console.ReadKey();
    }

    public void Update() 
    {
        List<FlashcardDTO> flashcards = flashcardRespository.GetAll(_stack.IdStack).ToList();

        if (!flashcards.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No flashcards found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(flashcards);

        int displayId = validateFlashcardInput.ValidateInt("Enter the [#619B8A]Id[/] of the flashcard to update or [#9A031E]0[/] to cancel:", flashcards);

        if (displayId == -1) return;

        FlashcardDTO selected = flashcards.FirstOrDefault(f => f.DisplayId == displayId);

        if (selected == null)
        {
            AnsiConsole.MarkupLine("[red]Flashcard not found.[/]");
            Console.ReadKey();
            return;
        }

        List<string> values = GetUserInput(
            "Please insert a new question.[#619B8A]The question must not be empty or only numbers and must be longer than 6 letters.[/] press [#9A031E]0[/] to cancel",
            "Please insert an new asnwer.[#619B8A]The answer must not be empty[/] press [#9A031E]0[/] to cancel",
            "Please insert a new score to the question [#619B8A](0-10)[/] or press [#9A031E]0[/] to cancel");

        flashcardRespository.Update(
            new Flashcard
            {
                IdFlashcard = selected.IdFlashcard,
                Question = values[0],
                Answer = values[1],
                QuestionScore = Int32.Parse(values[2])
            });
    }

    public void Delete() 
    {
        List<FlashcardDTO> flashcards = flashcardRespository.GetAll(_stack.IdStack).ToList();

        if (!flashcards.Any()) 
        {
            AnsiConsole.MarkupLine("[yellow]No flashcards found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(flashcards);

        int displayId = validateFlashcardInput.ValidateInt("Enter the [#619B8A]Id[/] of the flashcard to delete or [#9A031E]0[/] to cancel:",flashcards);

        if (displayId == -1) return;

        FlashcardDTO selected = flashcards.FirstOrDefault(f => f.DisplayId == displayId);

        flashcardRespository.Delete(selected.IdFlashcard);

        if (selected == null)
        {
            AnsiConsole.MarkupLine("[red]Flashcard not found.[/]");
            Console.ReadKey();
            return;
        }

        AnsiConsole.MarkupLine("[green]Flashcard deleted successfully![/]");
        Console.ReadKey();
    }

    public void ShowTable(IEnumerable<FlashcardDTO> flashcards) 
    {
        AnsiConsole.Clear();

        var table = new Table().Border(TableBorder.Rounded)
           .AddColumn("[#619B8A]Id[/]")
           .AddColumn("Question")
           .AddColumn("Answer")
           .AddColumn("[#EFDD8D]Question Score[/]");

        foreach (var f in flashcards)
        {
            table.AddRow(
                "[#619B8A]"+ f.DisplayId.ToString() + "[/]",
                f.Question,
                f.Answer,
                "[#EFDD8D]" + f.QuestionScore.ToString() + "[/]"
            );
        }

        table.Border(TableBorder.Horizontal);
        table.Expand();
        AnsiConsole.Write(table);
    }

    public List<string> GetUserInput(string questionMessage, string answerMessage, string questionScoreMessage) 
    {
        List<string> values = new();

        string question = validateFlashcardInput.ValidateQuestion(questionMessage);
        string answer = validateFlashcardInput.ValidateAnswer(answerMessage);
        int questionScore = validateFlashcardInput.ValidateScore(questionScoreMessage);

        values.Add(question);
        values.Add(answer);
        values.Add(questionScore.ToString());

        Console.WriteLine($"Question: {question}");
        Console.WriteLine($"Answer: {answer}");
        Console.WriteLine($"Score: {questionScore}");
        Console.ReadKey();

        return values;
    }
}