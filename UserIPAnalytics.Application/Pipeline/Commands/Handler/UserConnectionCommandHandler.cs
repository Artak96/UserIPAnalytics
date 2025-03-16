using MediatR;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Application.Pipeline.Commands.Handler
{
    public class UserConnectionCommandHandler : IRequestHandler<UserConnectionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserConnectionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UserConnectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.User.GetUserByIdAsync(request.UserId);

            if (user == null)
            {
                user = new User($"Name_{request.UserId}");
                await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            }

            var connection = new UserConnection(user.Id, request.IpAddress);
            await _unitOfWork.UserConnection.AddAsync(connection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
