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

    public Command? BuildCommand(MessageEventArgs receivedMessage)
    {
        if (string.IsNullOrWhiteSpace(receivedMessage.Message.Text))
            return null;

        var commandIsParsed = menuCommands.TryGetValue(receivedMessage.Message.Text, out Command? command);
        return commandIsParsed && command is not null ? BuildCommandData(command, receivedMessage) : null;
    }

    public Command? BuildCommand(UserState? userState, MessageEventArgs receivedMessage)
    {
        if (userState is null)
            return null;

        var commandIsParsed = processCommands.TryGetValue(userState.Value, out Command? command);
        return commandIsParsed && command is not null ? BuildCommandData(command, receivedMessage) : null;
    }

    private Command BuildCommandData(Command command, MessageEventArgs receivedMessage)
    {
        command.UserId = receivedMessage.Message.From.Id;
        command.Text = receivedMessage.Message.Text;
        command.Username = receivedMessage.Message.From.Username;
        command.FirstName = receivedMessage.Message.From.FirstName;

        return command;
    }
}