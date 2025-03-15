using MediatR;

namespace UserIPAnalytics.Application.Pipeline.Queries
{
    public class GetUserConnectionsIpsQuery : IRequest<List<string>>
    {
        public long userId { get; set; }
    }
}
