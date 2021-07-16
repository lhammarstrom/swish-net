using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Flurl.Http.Configuration;

namespace Swish.Models
{
    public class SwishHttpFactory : DefaultHttpClientFactory
    {
        private readonly X509Certificate2 _certificate;

        public SwishHttpFactory(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (
                    sender,
                    certificate,
                    chain,
                    sslPolicyErrors) => true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };

            handler.ClientCertificates.Add(_certificate);
            return handler;
        }
    }
}