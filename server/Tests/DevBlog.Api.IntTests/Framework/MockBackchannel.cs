using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DevBlog.Api.IntTests.Framework;

public class MockBackchannel : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri.AbsoluteUri.Equals("https://inmemory.microsoft.com/common/.well-known/openid-configuration"))
        {
            var response = await EmbeddedResourceReader.GetOpenIdConfigurationAsResponseMessage("microsoft-openid-config.json");
            return response;
        }
        if (request.RequestUri.AbsoluteUri.Equals("https://inmemory.microsoft.com/common/discovery/keys"))
        {
            var response = await EmbeddedResourceReader.GetOpenIdConfigurationAsResponseMessage("microsoft-wellknown-keys.json");
            return response;
        }

        throw new NotImplementedException();
    }
}