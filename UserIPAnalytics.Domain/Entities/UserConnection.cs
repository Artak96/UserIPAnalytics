using UserIPAnalytics.Domain.Common;

namespace UserIPAnalytics.Domain.Entities
{
    public class UserConnection : Entity
    {
        public long Id { get; set; }
        public string IpAddress { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public UserConnection() { }
        public UserConnection(long userId, string ipAddress)
        {
            UserId = userId;
            IpAddress = ipAddress;
        }
    }
}
