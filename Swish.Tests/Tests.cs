using System;
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
            _swish = new SwishApi(new SwishApiDetails
            {
                BaseUrl = "https://cpc.getswish.net/swish-cpcapi",
                CallbackUrl = null,
                ClientCertificate = "F06644FAF53150D5B31716ABF121FE112A225AF1",
                RootCertificateV1 = "A8985D3A65E5E5C4B2D7D66D40C6DD2FB19C5436",
                RootCertificateV2 = "03BFF7B54C712504C5BE5A8528163C931618A3C0"
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