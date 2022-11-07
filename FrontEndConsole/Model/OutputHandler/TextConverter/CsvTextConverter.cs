using EmailCleaner.Client.Data;
using Serilog;
using System.Text;

namespace FrontEndConsole.Model.OutputHandler.TextConverter;

public class CsvTextConverter : ITextConverter
{
    private const string MarkingCellSymbolFirst = "[";
    private const string MarkingCellSymbolLast = "]";

    private readonly string MarkingCell = $"{MarkingCellSymbolFirst} {MarkingCellSymbolLast}";

    private const char CellSeparator = ',';
    private const char SecondaryCellSeparator = ';';

    private const int NumberOfCells = 4;

    public MarkedEmailGroup Deserialize(string text)
    {
        var cells = text.Split(CellSeparator);

        if (cells.Length != NumberOfCells)
            Log.Warning($"Unexpexted number of cells in text when converting to EmailGroup: {text}");

        var processingCode = cells
            .First()
            .Replace(MarkingCellSymbolFirst, "")
            .Replace(MarkingCellSymbolLast, "")
            .Trim();

        var emailIds = GetEmailIds(cells.Last());

        return new MarkedEmailGroup(processingCode, emailIds);
    }

    private static List<string> GetEmailIds(string cellValue)
    {
        return cellValue.Split(SecondaryCellSeparator).ToList();
    }

    public string Serialize(EmailGroup emailGroup)
    {
        var sb = new StringBuilder();

        sb.Append(MarkingCell);
        sb.Append(CellSeparator);

        sb.Append(emailGroup.EmailsCount);
        sb.Append(CellSeparator);

        sb.Append(emailGroup.GroupedByValue);
        sb.Append(CellSeparator);

        sb.Append(CreateEmailIdString(emailGroup.Emails));

        return sb.ToString();
    }

    private static string CreateEmailIdString(List<EmailData> emails)
    {
        var emailIds = emails.Select(e => e.Id);

        return string.Join(SecondaryCellSeparator, emailIds);
    }
}
