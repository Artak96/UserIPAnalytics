namespace UserIPAnalytics.Application.Models.Response
{
    public class UserLastConnection
    {
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectionTime { get; set; }
    }
}
