using FrontEndConsole.Model.Configuration;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace FrontEndConsole.View
{
    internal class ConsoleMainUserInterfaceV1 : IMainUserInterface
    {
        private readonly ApplicationOptions _applicationOptions;

        public ConsoleMainUserInterfaceV1(IConfiguration configuration)
        {
            _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();
        }

        public void Alert(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }

        public void Clear()
        {
        }

        public void Display(string message)
        {
            Console.WriteLine(message);
        }

        public bool GetUserInputYesNo(string prompt)
        {
            var result = ShowMenu(prompt, new() { "Yes", "No" });

            return "Yes".Equals(result);
        }

        public void SetStatus(string sectionTitle)
        {
            Console.WriteLine(sectionTitle);
        }

        public string ShowMenu(string prompt, List<string> menuAlternatives)
        {
            var selectedItem = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(prompt)
                    .PageSize(Math.Max(menuAlternatives.Count, 3))
                    .AddChoices(menuAlternatives)
                );

            return selectedItem;
        }

        public void Start()
        {
        }
    }
}
