using MediatR;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Application.Pipeline.Commands.Handler
{
    internal class AddIPAddressCommandHandler : IRequestHandler<AddIPAddressCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddIPAddressCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddIPAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                var newUser = new User($"Name_{request.UserId}");
                await _unitOfWork.User.AddAsync(newUser);
            }

            var connection = new UserIPAddress { UserId = userId, IP = ip };
            await _unitOfWork.UserIPAddress.AddAsync(connection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
