﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Presence.Services.Options
{
    public class JwtSettings
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }
}
