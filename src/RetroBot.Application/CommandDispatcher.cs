using RetroBot.Application.CommandHandlers.Commands;
using RetroBot.Core;
using Telegram.Bot.Args;

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

    public Command? BuildCommand(string commandName, MessageEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(commandName))
            return null;

        var commandIsParsed = menuCommands.TryGetValue(commandName, out Command? command);
        return commandIsParsed && command is not null ? BuildCommandData(command, e) : null;
    }

    public Command? BuildCommand(UserState? userState, MessageEventArgs e)
    {
        if (userState is null)
            return null;

        var commandIsParsed = processCommands.TryGetValue(userState.Value, out Command? command);
        return commandIsParsed && command is not null ? BuildCommandData(command, e) : null;
    }

    private Command BuildCommandData(Command command, MessageEventArgs e)
    {
        command.UserId = e.Message.From.Id;
        command.Text = e.Message.Text;
        command.Username = e.Message.From.Username;
        command.FirstName = e.Message.From.FirstName;

        return command;
    }
}