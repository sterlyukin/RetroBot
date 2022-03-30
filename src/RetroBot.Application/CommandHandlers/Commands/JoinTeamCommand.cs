using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class JoinTeamCommand : Command, IRequest<CommandExecutionResult>
{
}