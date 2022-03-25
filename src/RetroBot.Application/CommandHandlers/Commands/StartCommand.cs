using MediatR;

namespace RetroBot.Application.CommandHandlers.Commands;

public sealed class StartCommand : Command, IRequest<string>
{
}