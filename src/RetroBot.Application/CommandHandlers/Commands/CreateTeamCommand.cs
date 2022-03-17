using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class CreateTeamCommand : Command, IRequest<string>
{
}