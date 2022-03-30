using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class InputTeamNameCommand : Command, IRequest<CommandExecutionResult>
{
}