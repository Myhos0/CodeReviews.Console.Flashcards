using Flashcards.Database;
using Flashcards.DTO;
using Flashcards.Menus.Enums;
using Flashcards.Model;
using Flashcards.Repositorys;
using Flashcards.ValidateInputs;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace Flashcards.Menus;
internal class StudySessionMenu
{
    private readonly StudySessionRepository sessionStudyRepository;
    private readonly FlashcardRespository flashcardRespository;
    private readonly ValidateStudySessionInput validateSessionStudyInput = new();
    private Stack _stack;

    public StudySessionMenu(DataBase database)
    {
        flashcardRespository = new FlashcardRespository(database);
        sessionStudyRepository = new StudySessionRepository(database);
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
                new SelectionPrompt<SessionStudyMenuOptions>()
                .Title("Select a option for the stack")
                .AddChoices(Enum.GetValues<SessionStudyMenuOptions>()));

            switch (menuOption)
            {
                case SessionStudyMenuOptions.StartSession:
                    StartSessionStudy();
                    break;
                case SessionStudyMenuOptions.ViewAllSessions:
                    ViewAllSessions();
                    break;
                case SessionStudyMenuOptions.ViewCurrentStackSessions:
                    ViewSessionsByStack();
                    break;
                case SessionStudyMenuOptions.Exit:
                    return;
            }
        }
    }

    private void StartSessionStudy()
    {
        List<StudySessionFlashcardDTO> questions = flashcardRespository.GetFlashcardsByStack(_stack.IdStack).ToList();

        if(questions.Count == 0) 
        {
            AnsiConsole.WriteLine("Current stack not have flashcards");
            Console.ReadKey();
            return;
        }

        int totalScore = 0;
        int numberQuestion = 1;
        int answerInt;

        foreach (var question in questions)
        {
            AnsiConsole.MarkupLine($"Question [#6665DD]{numberQuestion}[/] of [#473BF0]{questions.Count}[/]");

            AnsiConsole.MarkupLine($"\n[#7798AB]{question.Question}[/]\n");

            if (int.TryParse(question.Answer, out answerInt))
            {
                AnsiConsole.MarkupLine("Please write the asnwer [#619B8A](Number)[/]");
                string input = Console.ReadLine()?.Trim() ?? "";

                if(validateSessionStudyInput.ValidateAnswerInt(answerInt, input)) 
                {
                    totalScore += question.QuestionScore;
                    AnsiConsole.MarkupLine($"[green]Correct Asnwer.[/] Question Score: {question.QuestionScore}");
                    AnsiConsole.MarkupLine($"[#BDC667]Total Score: {totalScore}[/]\n");
                }
                else 
                {
                    AnsiConsole.MarkupLine($"[red]Wrong Answer.[/]");
                    AnsiConsole.MarkupLine($"[#77966D]The asnwer was: {question.Answer}[/]\n");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("Please write the asnwer [#619B8A](Number)[/]");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (validateSessionStudyInput.ValidateAnswerString(question.Answer, input))
                {
                    totalScore += question.QuestionScore;
                    AnsiConsole.MarkupLine($"[green]Correct Asnwer.[/] [#F2F3D9]Question Score: {question.QuestionScore}[/]");
                    AnsiConsole.MarkupLine($"[#BDC667]Total Score: {totalScore}[/]\n");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Wrong Answer.[/]");
                    AnsiConsole.MarkupLine($"The asnwer was: [#77966D]{question.Answer}[/]\n");
                }
            }

            numberQuestion++;
        }

        sessionStudyRepository.Insert(
            new StudySession 
            {
                Date = DateTime.Now,
                TotalScore = totalScore,
                IdStack = _stack.IdStack,
            });

        AnsiConsole.MarkupLine($"Date of the study session: {DateTime.Now.ToString("yyyy-MM-dd")}");
        AnsiConsole.MarkupLine($"[#BDC667]Total Score: {totalScore}[/]");
        AnsiConsole.MarkupLine("[green]Session saved successfully[/]");
        Console.ReadKey();
    }

    private void ViewAllSessions() 
    {
        ShowTable(sessionStudyRepository.GetAll());
        Console.ReadKey();
    }

    private void ViewSessionsByStack() 
    {
        ShowTable(sessionStudyRepository.GetSessionsByStack(_stack.IdStack));
        Console.ReadKey();
    }

    private void ShowTable(IEnumerable<StudySessionDTO> sessionStudies)
    {
        AnsiConsole.Clear();

        var table = new Table().Border(TableBorder.Rounded)
           .AddColumn("[#D3F8E2]Stack[/]")
           .AddColumn("Date")
           .AddColumn("[#EFDD8D]TotalScore[/]");

        foreach (var s in sessionStudies)
        {
            table.AddRow(
                "[#D3F8E2]" + s.StackName + "[/]",
                s.Date.ToString("yyyy-MM-dd"),
                "[#EFDD8D]" + s.TotalScore.ToString() + "[/]"
            );
        }

        table.Border(TableBorder.Horizontal);
        table.Expand();
        AnsiConsole.Write(table);
    }
}