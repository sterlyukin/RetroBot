using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class CreateTeamCommand : Command, IRequest<string>
{
}