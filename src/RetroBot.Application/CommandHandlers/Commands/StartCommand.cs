using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public class StartCommand : Command, IRequest<string>
{
}