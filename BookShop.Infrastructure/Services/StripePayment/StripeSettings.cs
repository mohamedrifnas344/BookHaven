using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Services.StripePayment
{
    public class StripeSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
    }
}
