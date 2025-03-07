using UserIPAnalytics.Domain.Common;

namespace UserIPAnalytics.Domain.Entities
{
    public class UserIPAddress : Entity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IPAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
