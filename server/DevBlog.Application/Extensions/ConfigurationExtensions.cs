using System;
using Microsoft.Extensions.Configuration;
namespace DevBlog.Application.Extensions;

public static class ConfigurationExtensions
{

    public static T GetSafely<T>(this IConfiguration configuration)
    {
        var result = configuration.Get<T>();

        if (result == null)
            throw new Exception($"Error getting value {typeof(T)} from configuration");

        return result;
    }
}