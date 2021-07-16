using Flurl;
using Flurl.Http;
using Swish.Models;
using System.Threading.Tasks;
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

            using (var certificate = new X509Certificate2(
                _configuration.CertificateAsBytes,
                _configuration.CertificatePassword,
                X509KeyStorageFlags.DefaultKeySet))
            {
                var result =
                    await new FlurlClient(
                        _configuration.BaseUrl.AppendPathSegments("api", "v2", "paymentrequests", reference)).Configure(
                        c => c.HttpClientFactory = new SwishHttpFactory(certificate)).Request().PutJsonAsync(new
                    {
                        amount = amountToPay,
                        payerAlias = phoneAlias,
                        message = messageToCustomer,
                        payeePaymentReference = reference,
                        currency = SwishDefaults.Currency.Sek,
                        payeeAlias = receiverAlias,
                        callbackUrl = _configuration.CallbackUrl
                    });

                return result.ResponseMessage.IsSuccessStatusCode
                    ? (false, "")
                    : (true, await result.ResponseMessage.Content.ReadAsStringAsync());
            }
        }
    }
}