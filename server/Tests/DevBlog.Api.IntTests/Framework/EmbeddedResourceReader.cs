﻿using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Api.IntTests.Framework;

public class EmbeddedResourceReader
{
    private const string EmbeddedResourceQualifier = "DevBlog.Api.IntTests";

    public static X509Certificate2 GetCertificate()
    {
        const string resourceName = $"{EmbeddedResourceQualifier}.test-api-authpfx";
        using var certificateStream = typeof(EmbeddedResourceReader).Assembly.GetManifestResourceStream(resourceName);
        if (certificateStream == null)
        {
            return null;
        }

        var rawBytes = new byte[certificateStream.Length];
        for (var i = 0; i < certificateStream.Length; i++)
        {
            rawBytes[i] = (byte)certificateStream.ReadByte();
        }

        return new X509Certificate2(rawBytes, "api-demo", X509KeyStorageFlags.UserKeySet);
    }
    public static async Task<HttpResponseMessage> GetOpenIdConfigurationAsResponseMessage(string resource)
    {
        var resourceName = $"{EmbeddedResourceQualifier}.well_known." + resource;
        using (var stream = typeof(EmbeddedResourceReader).Assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return new HttpResponseMessage
            {
                Content = content,
            };
        }
    }
}