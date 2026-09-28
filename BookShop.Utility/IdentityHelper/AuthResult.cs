using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Utility.IdentityHelper
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
