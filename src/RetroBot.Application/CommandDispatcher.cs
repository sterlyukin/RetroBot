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
            }
        };
    }

    public Command? GetCommandByName(string commandName)
    {
        if (string.IsNullOrWhiteSpace(commandName))
            return null;
        
        return menuCommands.TryGetValue(commandName, out Command? command) ? command : null;
    }

    public Command? GetCommandByState(UserState? userState)
    {
        if (userState is null)
            return null;

        return processCommands.TryGetValue(userState.Value, out Command? command) ? command : null;
    }
}