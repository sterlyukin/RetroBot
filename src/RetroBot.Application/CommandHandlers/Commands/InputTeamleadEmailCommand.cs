using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class InputTeamleadEmailCommand : Command, IRequest<string>
{
}