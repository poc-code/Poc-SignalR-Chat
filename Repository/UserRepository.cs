using server.Models;
using System.Collections.Generic;
using System.Linq;

namespace server.Repository
{
    public static class UserRepository
    {
        public static List<User> _users = new List<User> {
                new User { Id = 1, Username = "batman", Password = "batman", Role = "heroes", Group = "heroes" },
                new User { Id = 2, Username = "robin", Password = "robin", Role = "heroes", Group = "heroes" },
                new User { Id = 1, Username = "coringa", Password = "coringa", Role = "vilan", Group = "vilans" },
                new User { Id = 1, Username = "pinguim", Password = "pinguin", Role = "vilan", Group = "vilans" },
                new User { Id = 3, Username = "uira", Password = "Senha2020", Role = "admin", Group = "admin" }
            };

        public static User Get(string username, string password)
        {
            return _users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password.ToLower()).FirstOrDefault();
        }

        public static List<User> GetAll()
        {
            return _users;
        }

        public static IEnumerable<User> GetByGroup(string group)
        {
            return _users.Where(x => x.Group.Equals(group));
        }

        public static IEnumerable<string> GetGroups()
        {
            return _users.GroupBy(g => g.Group).Select(s => s.Key);
        }
    }
}
