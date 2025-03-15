using MediatR;
using UserIPAnalytics.Domain.Abstractions;

namespace UserIPAnalytics.Application.Pipeline.Queries.Handlers
{
    public class GetUserConnectionsIpsQueryHandler : IRequestHandler<GetUserConnectionsIpsQuery, List<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserConnectionsIpsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<string>> Handle(GetUserConnectionsIpsQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserConnection.GetUserAllIpsAsync(request.userId);
            return result;
        }
    }
}
