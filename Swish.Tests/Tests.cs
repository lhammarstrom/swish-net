using System;
using System.IO;
using System.Threading.Tasks;
using Swish.Models;
using Swish.Services;
using Xunit;

namespace Swish.Tests
{
    public class Test
    {
        private readonly SwishApi _swish;

        public Test()
        {
            // ugly hack
            var directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName)?.FullName)?.FullName;
            _swish = new SwishApi(new SwishApiDetails
            {
                BaseUrl = "https://cpc.getswish.net/swish-cpcapi",
                CallbackUrl = "https://example.com/api/swishcb/paymentrequests",
                CertificatePath = directory + "/Swish_Merchant_TestCertificate_1234679304.p12",
                CertificatePassword = "swish"
            });
        }

        [Fact]
        public async Task TestPaymentRequest()
        {
            var (hasError, _) = await _swish.MakePaymentRequest(
                1,
                Guid.NewGuid().ToString("N").ToUpper(),
                "4671111111",
                "Swish Test",
                "Testing");

            Assert.False(hasError);
        }
    }
}