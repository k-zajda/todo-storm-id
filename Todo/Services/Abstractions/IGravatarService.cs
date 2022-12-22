using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Todo.Services.Abstractions
{
    public interface IGravatarService
    {
        Task<string> GetUsernameByEmailAsync(string emailAddress);
    }
    
    public class GravatarService :IGravatarService
    {
        private const int GRAVATAR_TIMEOUT_MILLISECONDS = 200000;
        
        private readonly IGravatarHttpClient httpClient;
        private readonly IMemoryCache cache;

        public GravatarService(IGravatarHttpClient httpClient, IMemoryCache cache)
        {
            this.httpClient = httpClient;
            this.cache = cache;
        }
        
        public async Task<string> GetUsernameByEmailAsync(string emailAddress)
        {
            var cancellation = new CancellationTokenSource(
                TimeSpan.FromMilliseconds(GRAVATAR_TIMEOUT_MILLISECONDS));

            var hash = Gravatar.GetHash(emailAddress);

            var entry = await this.cache.GetOrCreateAsync(GetProfileCacheKey(hash), async _ =>
            {
                try
                {
                    var profile = await httpClient.GetProfileByHash(hash, cancellation.Token);
                    return profile;
                }
                catch (OperationCanceledException)
                {
                    // Probably want to do more smart stuff here, like lazy loading profile anyway.
                    return null;
                }
            });

            return entry?.Entry?.FirstOrDefault()?.DisplayName;
        }

        private static string GetProfileCacheKey(string hash) => $"GRAVATAR_PROFILE_{hash}";
    }

    public interface IGravatarHttpClient
    {
        Task<GravatarProfile> GetProfileByHash(string hash, CancellationToken cancellationToken);
    }
    
    public class GravatarHttpClient : IGravatarHttpClient
    {
        public const string HttpClientName = "GRAVATAR_HTTP";
        
        private readonly IHttpClientFactory clientFactory;

        public GravatarHttpClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        
        public async Task<GravatarProfile> GetProfileByHash(string hash, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentNullException(nameof(hash));

            using var client = clientFactory.CreateClient(HttpClientName);
            var request = new HttpRequestMessage(HttpMethod.Get, hash + ".json");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));
            var response = await client.SendAsync(request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
                return new GravatarProfile();

            var profile = await response.Content.ReadFromJsonAsync<GravatarProfile>(cancellationToken: cancellationToken);

            return profile;
        }
    }

    public class GravatarProfile
    {
        public IEnumerable<GravatarEntry> Entry { get; set; }
    }

    public class GravatarEntry
    {
        public string Id { get; set; }
        
        public string DisplayName { get; set; }
    }
}