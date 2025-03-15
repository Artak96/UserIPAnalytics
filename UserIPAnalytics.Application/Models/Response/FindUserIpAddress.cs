namespace UserIPAnalytics.Application.Models.Response
{
    public class FindUserIpAddress
    {
        public long UserId { get; set; }

        public string IpAddress { get; set; }
    }
}
