using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class ResetCommand : Command, IRequest<CommandExecutionResult>
{
}