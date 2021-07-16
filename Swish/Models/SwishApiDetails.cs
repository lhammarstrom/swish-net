using System;

namespace Swish.Models
{
    public class SwishApiDetails
    {
        public string BaseUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string Certificate { get; set; }
        public string CertificatePassword { get; set; }
        public byte[] CertificateAsBytes => Convert.FromBase64String(Certificate);
    }
}