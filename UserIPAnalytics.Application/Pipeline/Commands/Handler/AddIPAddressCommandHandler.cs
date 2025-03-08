using MediatR;

namespace UserIPAnalytics.Application.Pipeline.Commands.Handler
{
    internal class AddIPAddressCommandHandler : IRequestHandler<AddIPAddressCommand>
    {
        public Task Handle(AddIPAddressCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
