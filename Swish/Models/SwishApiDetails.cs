namespace Swish.Models
{
    public class SwishApiDetails
    {
        public string BaseUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string ClientCertificate { get; set; }
        public string RootCertificateV1 { get; set; }
        public string RootCertificateV2 { get; set; }
    }
}