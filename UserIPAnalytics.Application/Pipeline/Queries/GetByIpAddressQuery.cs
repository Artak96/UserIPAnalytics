using MediatR;
using UserIPAnalytics.Application.Models.Response;

namespace UserIPAnalytics.Application.Pipeline.Queries
{
    public class GetByIpAddressQuery : IRequest<List<FindUserIpAddress>>
    {
        public string IpAddress { get; set; }
    }
}
