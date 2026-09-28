using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Services.CloudinaryService
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;
        public string APISecret { get; set; } = string.Empty;
        public string APIKey { get; set; } = string.Empty;
    }
}
