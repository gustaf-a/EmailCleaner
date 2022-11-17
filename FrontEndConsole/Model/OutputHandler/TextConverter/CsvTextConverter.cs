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

    public string GetInstructions(List<string> emailActionsInstructionLines)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"To mark a line for processing, add the requested symbol in the beginning of the line between the '{MarkingCellSymbolFirst} {MarkingCellSymbolLast}'.");
        sb.AppendLine("Spaces are trimmed away.");
        sb.AppendLine("");
        sb.AppendLine("Marked cell example (replace x with the needed code/command):");
        sb.AppendLine($"{MarkingCellSymbolFirst} x {MarkingCellSymbolLast}");
        sb.AppendLine("");
        sb.AppendLine("Not marked cell (no action executed):");
        sb.AppendLine($"{MarkingCellSymbolFirst}  {MarkingCellSymbolLast}");
        sb.AppendLine("");

        sb.AppendLine("-- Available commands --");
        foreach (var instructionLine in emailActionsInstructionLines)
           sb.AppendLine(instructionLine);

        return sb.ToString();
    }

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
