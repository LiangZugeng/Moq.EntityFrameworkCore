namespace Moq.EntityFrameworkCore.Examples.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq.EntityFrameworkCore.Examples.Users.Entities;

    public class UsersService
    {
        private readonly UsersContext usersContext;

        public UsersService(UsersContext usersContext)
        {
            this.usersContext = usersContext;
        }

        public IList<User> GetLockedUsers()
        {
            return this.usersContext.Users.Where(x => x.AccountLocked).ToList();
        }

        public async Task<IList<User>> GetLockedUsersAsync()
        {
            return await this.usersContext.Users.Where(x => x.AccountLocked).ToListAsync();
        }

        public void RemoveLockedUsers()
        {
            var lockedUsers = this.usersContext.Users.Where(u => u.AccountLocked).ToList();
            usersContext.Users.RemoveRange(lockedUsers);
            usersContext.SaveChanges();
        }

        public void AddUsers(User user)
        {
            usersContext.Users.Add(user);
            usersContext.SaveChanges();
        }
    }
}