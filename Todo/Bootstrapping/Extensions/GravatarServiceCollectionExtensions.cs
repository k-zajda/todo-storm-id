using System;
using Microsoft.Extensions.DependencyInjection;
using Todo.Services.Abstractions;

namespace Todo.Bootstrapping.Extensions
{
    public static class GravatarServiceCollectionExtensions
    {
        public static void AddGravatar(this IServiceCollection services)
        {
            services.AddTransient<IGravatarService, GravatarService>();
            services.AddTransient<IGravatarHttpClient, GravatarHttpClient>();
            services.AddHttpClient(GravatarHttpClient.HttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://en.gravatar.com");
            });
        }
    }
}