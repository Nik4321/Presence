using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presence.Data;
using Presence.Data.Models;

namespace Presence.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<User> AllUsers()
        {
            return this.db.Users;
        }
    }
}
