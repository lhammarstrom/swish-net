using System;
using System.Net.Http;
using Flurl;
using Flurl.Http;
using Swish.Models;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;

namespace Swish.Services
{
    public interface ISwishApi
    {
        Task<(bool HasError, string Message)> MakePaymentRequest(
            int amountToPay,
            string reference,
            string phoneNumber,
            string receiverAlias,
            string messageToCustomer);
    }

    public class SwishApi : ISwishApi
    {
        private readonly SwishApiDetails _configuration;

        public SwishApi(SwishApiDetails configuration)
        {
            _configuration = configuration;
        }

        public async Task<(bool HasError, string Message)> MakePaymentRequest(
            int amountToPay,
            string reference,
            string phoneNumber,
            string receiverAlias,
            string messageToCustomer)
        {
            var phoneAlias = phoneNumber.Replace(" ", "").Replace("+", "");
            if (phoneAlias.StartsWith("0")) phoneAlias = phoneAlias.Substring(1, phoneAlias.Length);

            // using var certificate = new X509Certificate2(
            //     _configuration.CertificateAsBytes,
            //     _configuration.CertificatePassword,
            //     X509KeyStorageFlags.EphemeralKeySet);
            //
            // var client = new FlurlClient(
            //     _configuration.BaseUrl.AppendPathSegments("api", "v2", "paymentrequests", reference)).Configure(
            //     c => c.HttpClientFactory = new SwishHttpFactory(certificate));
            //
            // var request = client.Request();
            //
            // var result =
            //     await request.PutJsonAsync(new
            //     {
            //         amount = amountToPay,
            //         payerAlias = phoneAlias,
            //         message = messageToCustomer,
            //         payeePaymentReference = reference,
            //         currency = SwishDefaults.Currency.Sek,
            //         payeeAlias = receiverAlias,
            //         callbackUrl = _configuration.CallbackUrl
            //     });
            
            var requestData = new
            {
                payeePaymentReference = reference,
                callbackUrl = _configuration.CallbackUrl,
                payerAlias = phoneNumber,
                payeeAlias = phoneAlias,
                amount = amountToPay,
                currency = "SEK",
                message = messageToCustomer
            };
            
            HttpClientHandler handler;
            HttpClient client;
            PrepareHttpClientAndHandler(out handler, out client);
            
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_configuration.BaseUrl + "/api/v2/paymentrequests/11A86BE70EA346E4B1C39C874173F088"),
                Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json")
            };

            var response = client.SendAsync(httpRequestMessage).Result;

            // return result.ResponseMessage.IsSuccessStatusCode
            //     ? (false, "")
            //     : (true, await result.ResponseMessage.Content.ReadAsStringAsync());

            return (true, "asd");
        }
        
        private void PrepareHttpClientAndHandler(out HttpClientHandler handler, out HttpClient client)
        {
            handler = new HttpClientHandler();
            
            // Got help for this code on https://stackoverflow.com/questions/61677247/can-a-p12-file-with-ca-certificates-be-used-in-c-sharp-without-importing-them-t
            using (X509Store store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadWrite);

                var certs = new X509Certificate2Collection();
                certs.Import("Swish_Merchant_TestCertificate_1234679304.p12", "swish");

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
            }

            client = new HttpClient(handler);
        }
    }
}