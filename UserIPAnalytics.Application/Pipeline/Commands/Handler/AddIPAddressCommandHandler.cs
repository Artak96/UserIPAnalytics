using MediatR;
using UserIPAnalytics.Domain.Abstractions;

namespace UserIPAnalytics.Application.Pipeline.Commands.Handler
{
    internal class AddIPAddressCommandHandler : IRequestHandler<AddIPAddressCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddIPAddressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Handle(AddIPAddressCommand request, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }
    }
}
