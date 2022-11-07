using FrontEndConsole.Model.AppActions;

namespace FrontEndConsole.View;

internal interface IMainUserInterface
{
    void Alert(string message);
    void Clear();
    void Display(string message);
    bool GetUserInputYesNo(string prompt);
    void SetStatus(string sectionTitle);
    string ShowMenu(string prompt, List<string> menuAlternatives);
    public void Start();
}
