using MediatR;

namespace UserIPAnalytics.Application.Pipeline.Commands
{
    public class AddIPAddressCommand : IRequest
    {
        public string IpAddress { get; set; }
    }
}
