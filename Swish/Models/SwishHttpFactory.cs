using System;
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
            var handler = new HttpClientHandler();
            
            // Got help for this code on https://stackoverflow.com/questions/61677247/can-a-p12-file-with-ca-certificates-be-used-in-c-sharp-without-importing-them-t
            using X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            var certs = new X509Certificate2Collection();
            certs.Import(Environment.CurrentDirectory + "\\Swish_Merchant_TestCertificate_1234679304.p12", "swish");

            foreach (X509Certificate2 cert in certs)
            {
                if (cert.HasPrivateKey)
                {
                    handler.ClientCertificates.Add(cert);
                }
                else
                {
                    store.Add(cert);
                }
            }

            return handler;
        }
    }
}