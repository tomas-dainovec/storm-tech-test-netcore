using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Todo.Models.Gravatar;

namespace Todo.Services
{
    public class GravatarClient : IGravatarClient
    {
        private const string GravatarBaseUri = "https://www.gravatar.com/";
        private const int TimeoutSecs = 2;

        private HttpClient _httpClient;
        private ILogger _logger;

        public GravatarClient(HttpClient client, ILogger<GravatarClient> logger)
        {
            client.BaseAddress = new Uri(GravatarBaseUri);
            client.Timeout = TimeSpan.FromSeconds(TimeoutSecs);
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TodoApp", "1.0"));

            _httpClient = client;
            _logger = logger;
        }

        public async Task<GravatarProfile> GetGravatarProfile(string emailHash)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync($"/{emailHash}.json");

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadAsAsync<GravatarResponse>();

                    return response.Entry?.FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "An error occured calling Gravatar API.");
            }

            return null;
        }
    }
}
