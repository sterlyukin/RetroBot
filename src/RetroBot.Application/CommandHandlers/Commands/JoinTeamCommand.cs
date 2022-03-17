using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class JoinTeamCommand : Command, IRequest<string>
{
}