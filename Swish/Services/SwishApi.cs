using System;
using Swish.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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
        private readonly HttpClient _client;
        private readonly SwishApiDetails _configuration;

        public SwishApi(SwishApiDetails configuration)
        {
            _configuration = configuration;
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (
                    sender,
                    certificate,
                    chain,
                    sslPolicyErrors) => true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            var certificates = new X509Certificate2Collection();
            certificates.Import(_configuration.CertificatePath, _configuration.CertificatePassword, X509KeyStorageFlags.DefaultKeySet);

            foreach (var certificate in certificates)
            {
                if (certificate.HasPrivateKey)
                {
                    handler.ClientCertificates.Add(certificate);
                }
                else
                {
                    store.Add(certificate);
                }
            }

            _client = new HttpClient(handler) {BaseAddress = new Uri(_configuration.BaseUrl)};
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

            var result = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{_configuration.BaseUrl}/api/v2/paymentrequests/{reference}"),
                Content = JsonContent.Create(new
                {
                    amount = amountToPay,
                    payerAlias = phoneAlias,
                    payeeAlias = receiverAlias,
                    message = messageToCustomer,
                    payeePaymentReference = reference,
                    currency = SwishDefaults.Currency.Sek,
                    callbackUrl = _configuration.CallbackUrl
                })
            });

            return result.IsSuccessStatusCode
                ? (false, string.Empty)
                : (true, await result.Content.ReadAsStringAsync());
        }
    }
}