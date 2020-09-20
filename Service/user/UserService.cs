using server.Models;
using server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Service.user
{
    public class UserService
    {

        public async Task<IEnumerable<string>> GetGroupAll()
        {
            return await Task.FromResult(UserRepository.GetGroups());
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await Task.FromResult(UserRepository.Get(username, password));
        }

        public async Task<IEnumerable<User>> GetGroupUsers(string group)
        {
            return await Task.FromResult(UserRepository.GetByGroup(group));
        }
    }
}
