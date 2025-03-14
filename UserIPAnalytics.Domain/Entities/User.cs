using UserIPAnalytics.Domain.Common;

namespace UserIPAnalytics.Domain.Entities
{
    public class User : Entity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserConnection> UserConnections { get; set; } = new List<UserConnection>();

        public User() { }
        public User(string name)
        {
            Name = name;
        }
    }
}
