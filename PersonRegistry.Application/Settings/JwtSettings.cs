using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpirationHours { get; set; }
    }
}
