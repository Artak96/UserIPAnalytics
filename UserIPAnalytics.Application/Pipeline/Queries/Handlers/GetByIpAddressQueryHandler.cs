using MediatR;

namespace UserIPAnalytics.Application.Pipeline.Queries.Handlers
{
    public class GetByIpAddressQueryHandler : IRequestHandler<GetByIpAddressQuery, object>
    {
        public Task<object> Handle(GetByIpAddressQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
