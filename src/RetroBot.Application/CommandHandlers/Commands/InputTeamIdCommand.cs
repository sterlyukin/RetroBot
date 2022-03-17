using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class InputTeamIdCommand : Command, IRequest<string>
{
}