using MediatR;

namespace UserIPAnalytics.Application.Pipeline.Commands
{
    public class UserConnectionCommand : IRequest
    {
        public string IpAddress { get; set; }
        public long UserId { get; set; }
    }
}
