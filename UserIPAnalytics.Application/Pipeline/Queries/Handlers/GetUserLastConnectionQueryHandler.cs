using MediatR;
using UserIPAnalytics.Application.Models.Response;
using UserIPAnalytics.Domain.Abstractions;

namespace UserIPAnalytics.Application.Pipeline.Queries.Handlers
{
    public class GetUserLastConnectionQueryHandler : IRequestHandler<GetUserLastConnectionQuery, UserLastConnection>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserLastConnectionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserLastConnection> Handle(GetUserLastConnectionQuery request, CancellationToken cancellationToken)
        {
            var lastConnection = await _unitOfWork.UserConnection.GetUserLastConnectionAsync(request.UserId);
            if (lastConnection == null)
            {
                return new UserLastConnection();
            }

            return new UserLastConnection
            {
                UserId = lastConnection.UserId,
                IpAddress = lastConnection.IpAddress,
                ConnectionTime = lastConnection.CreatedDate,
            };
        }
    }
}
