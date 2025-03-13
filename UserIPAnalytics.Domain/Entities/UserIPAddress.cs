using UserIPAnalytics.Domain.Common;

namespace UserIPAnalytics.Domain.Entities
{
    public class UserIPAddress : Entity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IPAddress { get; set; }

        public UserIPAddress() { }
        public UserIPAddress(long userId, string ipAddress)
        {
            UserId = userId,
                IP
        }
    }
}
