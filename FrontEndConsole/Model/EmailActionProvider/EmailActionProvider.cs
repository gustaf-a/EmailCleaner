using FrontEndConsole.Model.EmailActions;
using FrontEndConsole.Services;
using Serilog;

namespace FrontEndConsole.Model.EmailActionProvider;

public class EmailActionProvider : IEmailActionProvider
{
    private readonly Dictionary<List<string>, IEmailAction> _emailActions = new();

    public EmailActionProvider(IApiGatewayService apiGatewayService)
    {
        AddEmailAction(new DeleteForced());
    }

    private void AddEmailAction(IEmailAction emailAction)
    {
        _emailActions.Add(emailAction.GetCodes(), emailAction);
    }

    public IEmailAction GetEmailAction(string code)
    {
        foreach (var keyList in _emailActions.Keys)
            if (keyList.Contains(code))
                return _emailActions[keyList];

        Log.Error($"Failed to find action for: {code}.");

        //TODO Review again-action?
        return null;
    }

    public List<string> GetEmailActionInstructions()
    {
        var instructions = _emailActions
            .Select(ea => {
                return $"{string.Join(" | ", ea.Key)} : {ea.Value.GetDescription()}";
                })
            .ToList();

        return instructions;
    }
}
