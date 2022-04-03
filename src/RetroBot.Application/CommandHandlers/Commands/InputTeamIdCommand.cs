using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class InputTeamIdCommand : Command, IRequest<string>
{
}