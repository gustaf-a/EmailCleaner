namespace FrontEndConsole.Model.EmailActions;

internal class EmailActionComparer : IComparer<IEmailAction>
{
    public int Compare(IEmailAction? x, IEmailAction? y)
    {
        return x.GetPriority() > y.GetPriority() ? 1 : 0;
    }
}
