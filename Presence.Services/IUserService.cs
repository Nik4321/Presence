﻿using Presence.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presence.Services
{
    public interface IUserService
    {
        IQueryable<User> AllUsers();
    }
}