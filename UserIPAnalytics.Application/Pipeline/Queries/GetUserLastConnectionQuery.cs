using MediatR;
using UserIPAnalytics.Application.Models.Response;

namespace UserIPAnalytics.Application.Pipeline.Queries
{
    public class GetUserLastConnectionQuery : IRequest<UserLastConnection>
    {
        public long UserId { get; set; }
    }
}
