using System;
using System.Collections.Generic;
using System.Text;

namespace Presence.Infrastructure.Exceptions
{
    public class UserAuthenticationException : ApplicationException
    {
        public UserAuthenticationException(string message) : base(message)
        {
        }
    }
}
