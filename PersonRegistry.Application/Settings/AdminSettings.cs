using System;
using System.Collections.Generic;
using System.Text;

namespace PersonRegistry.Application.Settings
{
    public class AdminSettings
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
