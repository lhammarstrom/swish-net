using Xunit;
using System;
using Swish.Models;
using Swish.Services;
using System.Threading.Tasks;

namespace Swish.Tests
{
    public class Test
    {
        private readonly SwishApi _swish;

        public Test()
        {
            _swish = new SwishApi(new SwishApiDetails
            {
                BaseUrl = "https://mss.cpc.getswish.net/swish-cpcapi",
                CallbackUrl = "https://example.com/api/swishcb/paymentrequests",
                Certificate =
                    "MIIFbTCCA1WgAwIBAgIQC/OllYj2VMdng1/JWlNhDzANBgkqhkiG9w0BAQ0FADBrMQswCQYDVQQGEwJTRTEeMBwGA1UECgwVTm9yZGVhIEJhbmsgQUIgKHB1YmwpMREwDwYDVQQFEwhOREVBU0VTUzEpMCcGA1UEAwwgTm9yZGVhIEN1c3RvbWVyIENBMSB2MiBmb3IgU3dpc2gwHhcNMjAwNTEzMDc0MzEzWhcNMjIwNTEzMDc0MzEzWjA3MQswCQYDVQQGEwJTRTETMBEGA1UECgwKNTU2MDk5Nzk4MjETMBEGA1UEAwwKMTIzNDY3OTMwNDCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAKY3xNFHa0ipVemTS7x7LeF4+iZy71EBucTatk5/uia/1z6eGxDe4sBV1Fj6iWheFO8IuxAEytZrIB6A4Fm4hyVwQgwfiEbuseAIJIZNtyr9Eal63+tTvRUu2N+9S92jSwr6RxRhI1424B7G9HsXBKkQCm/82eDh3Js1i+H6188uUfDH6OBPZndQB6fg8bqag8szIumHmO4yVUaLmaoaJDKMgch8VV991RL6C6mReG3S4P/YLmpxPHkf70rJOgFuQiJR0k4iNJiyWUGBZgRdXa2T90g/FqbWNJ6CQPgP4MDdpcsbfMyCVnJk5QTM9CCaGw+hCxbJKgKuL32xvT2EDr13C0xI/grZp+XckAD0rwaxBlStn+IwWx9XrehCVejUCu0LXO9rtIUJHVoeMLLrjhGH6IwJw8bCHRv5w5cWrZQaj01YACKwkTfscxiUA5nYGOmwY6uig7n/yddERgCJxXfLmLH4YpL7hTw+HFw/ONqWgWzlAndsC3mv3qA34myHQLvmsJHvtSlNfwznVM7hLtY9lgxh4nXV74Jr2avIK+5XuA3LsWBBlhYEUAhaQXGblMFFeAU8Bbndoco6RdDUeVrPePz7/L78Ya0BzMHPKKB/aZOG3WF71zmD98C3yobJbR1dSUEGtjdniMeqgw/8Srmlbt2YIgqBydFujAqdVeOrAgMBAAGjQTA/MBEGA1UdDgQKBAhNn2qhfWaWojAVBgNVHSAEDjAMMAoGCCqFcIFtAQEBMBMGA1UdIwQMMAqACEI+A3SZ5AwHMA0GCSqGSIb3DQEBDQUAA4ICAQCCV9+6xizZ5jDPELX9bzZhF2CPNKRQlPmBddbn7NkW3fMsO4s2bUnyiWrxqci+e/EbR5nJXIR6fcZ0VamD7lVkUqWxahZh26Zkd7I4UsFPe4z2JwkWPBdYW2F+ioMJdlX3E2MMDF1AajEMBDg73vf/5Mf7BiLZ9l+qjLZoBApfCt3yMH/GQfSPbAYMLnpGBl3XHUTsJR2haHkFrzPBD5OqfGpbrkqF8Pk0HSJP4so1xfIOc2+otpQkCzlG2Ja/jQsCD4ylQCNt1VSLZpmzb8+8c9WIIUYk9Dj8YlgPKmOeW1H9sCyVJG8qG/XK93lQxOUvs4HT8E5L8SVQqW3JSrHg+RXns5wSLvWLgBfk1jBaAbOkldtbnV48K4v7ziCe5JQN2ssxwVi+cFpJUQCaes5I6oxx4F2saZLmfatMGnZGWWs1otuUYnk1/bZxd6iOFMpUP7Od3m3ei3D8NB/mio8Y5EzOgVGNUshxoePODi3u/whiioyWWGCz9LFGhaVgqlz1DpPXuv8r5VaFGNC9JubWur5DQFnxRQ6V/yitA2uP4A9rFWxji63yrNVsteJlHsII1/iUkFvVZfgMD1xnJf+n+tVo4dUE6DYDHspkStR7qrZ66mZl0hacW9dNvN781qDr2zwZ5AtAjimUfAWddz7VzY0rqYYGvDOvbBRfrGW0dA==",
                CertificatePassword = "swish"
            });
        }

        [Fact]
        public async Task TestPaymentRequest()
        {
            var (hasError, _) = await _swish.MakePaymentRequest(
                1,
                Guid.NewGuid().ToString("N").ToUpper(),
                "1234679304",
                "Swish Test",
                "Testing");

            Assert.False(hasError);
        }
    }
}