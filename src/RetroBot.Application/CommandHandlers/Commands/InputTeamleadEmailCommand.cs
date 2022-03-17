using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class InputTeamleadEmailCommand : Command, IRequest<string>
{
}