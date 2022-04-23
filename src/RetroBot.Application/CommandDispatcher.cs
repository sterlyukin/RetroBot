using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Core;

namespace RetroBot.Application;

public sealed class CommandDispatcher
{
    private readonly IDictionary<string, Command?> menuCommands;
    private readonly IDictionary<UserState, Command> processCommands;

    public CommandDispatcher(Messages messages)
    {
        menuCommands = new Dictionary<string, Command?>
        {
            {
                messages.StartMenuCommand,
                new StartCommand()
            },
            {
                messages.JoinTeamMenuCommand,
                new JoinTeamCommand()
            },
            {
                messages.CreateTeamMenuCommand,
                new CreateTeamCommand()
            }
        };
        
        processCommands = new Dictionary<UserState, Command>
        {
            {
                UserState.OnInputTeamId,
                new InputTeamIdCommand()
            },
            {
                UserState.OnInputTeamName,
                new InputTeamNameCommand()
            },
            {
                UserState.OnInputTeamleadEmail,
                new InputTeamleadEmailCommand()
            },
            {
                UserState.OnReset,
                new ResetCommand()
            }
        };
    }

    public Command? BuildCommand(long userId, string text, string userName, string firstName)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var commandIsParsed = menuCommands.TryGetValue(text, out Command? command);
        return commandIsParsed && command is not null
            ? InitializeCommandData(command, userId, text, userName, firstName)
            : null;
    }

    public Command? BuildCommand(UserState? userState, long userId, string text, string userName, string firstName)
    {
        if (userState is null)
            return null;

        var commandIsParsed = processCommands.TryGetValue(userState.Value, out Command? command);
        return commandIsParsed && command is not null
            ? InitializeCommandData(command, userId, text, userName, firstName)
            : null;
    }

    private Command InitializeCommandData(Command command, long userId, string text, string userName, string firstName)
    {
        command.UserId = userId;
        command.Text = text;
        command.Username = userName;
        command.FirstName = firstName;

        return command;
    }
}