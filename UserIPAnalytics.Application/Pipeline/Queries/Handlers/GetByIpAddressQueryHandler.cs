using MediatR;
using UserIPAnalytics.Application.Models.Response;
using UserIPAnalytics.Domain.Abstractions;

namespace UserIPAnalytics.Application.Pipeline.Queries.Handlers
{
    public class GetByIpAddressQueryHandler : IRequestHandler<GetByIpAddressQuery, List<FindUserIpAddress>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetByIpAddressQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FindUserIpAddress>> Handle(GetByIpAddressQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserConnection.FindUsersByIpPartAsync(request.IpAddress);
            var userIpAddreses = new List<FindUserIpAddress>();
            foreach (var uia in userIpAddreses)
            {
                var userData = new FindUserIpAddress
                {
                    IpAddress = uia.IpAddress,
                    UserId = uia.UserId,
                };
                userIpAddreses.Add(userData);
            }
            return userIpAddreses;
        }
    }
}
