using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class InputTeamNameCommand : Command, IRequest<string>
{
}