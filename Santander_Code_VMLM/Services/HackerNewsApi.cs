using Microsoft.Extensions.Caching.Memory;
using Santander_Code_VMLM.Interfaces;
using Santander_Code_VMLM.Models;
using System.Text.Json;

namespace Santander_Code_VMLM.Services
{
    public class HackerNewsApi : IHackerNewsApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HackerNewsApi> _logger;


        public HackerNewsApi(IHttpClientFactory httpClientFactory, ILogger<HackerNewsApi> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        /// <summary>
        /// Get story from Hacker News via API.
        /// </summary>
        /// <param name="id">Story ID.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<BestHistories> GetStoryAsync(int id, CancellationToken cancellationToken)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://hacker-news.firebaseio.com/v0/item/{id}.json");
            using HttpClient client = _httpClientFactory.CreateClient();

            try
            {
                using var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    return await JsonSerializer.DeserializeAsync<BestHistories>(contentStream) ?? throw new NullReferenceException();
                }
                throw new HttpRequestException(string.Format("<{MethodName}>: Failed to retrieve information about story with ID='{storyId}' from Hacker News. Http request returned with status code {StatusCode}.", nameof(GetStoryAsync), id, httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("<{MethodName}>: Failed to retrieve information about story with ID='{storyId}' from Hacker News. {Message}", nameof(GetStoryAsync), id, ex.Message));

                return await Task.FromException<BestHistories>(ex);
            }
        }

        /// <summary>
        /// Get best stories' IDs from Hacker News via API
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetBestStoriesIdsAsync(CancellationToken cancellationToken)
        {
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://hacker-news.firebaseio.com/v0/beststories.json");
            using HttpClient client = _httpClientFactory.CreateClient();

            try
            {
                using var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    //var data = new List<BestHistories>();
                    //while (contentStream.CanRead)
                    //{
                    //    var item = new BestHistories();

                    //    item.Uri = contentStream.
                    //    //item.Nombre = (string)dataReader["Nombre"];
                    //    //item.CorreoElectronico = (string)dataReader["CorreoElectronico"];
                    //    //item.Rol = (string)dataReader["Rol"];
                    //    //item.Estatus = (string)dataReader["Estatus"];
                    //    //item.SesionAbierta = (string)dataReader["SesionAbierta"];

                    //    data.Add(item);
                    //}

                    return await JsonSerializer.DeserializeAsync<IEnumerable<int>>(contentStream) ?? throw new NullReferenceException();
                }
                throw new HttpRequestException(string.Format("<{MethodName}>: Failed to retrieve best stories' ids from Hacker News. Http request returned with status code {StatusCode}.", nameof(GetBestStoriesIdsAsync), httpResponseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("<{MethodName}>: Failed to retrieve best stories' ids from Hacker News. {Message}", nameof(GetBestStoriesIdsAsync), ex.Message));

                return await Task.FromException<IEnumerable<int>>(ex);
            }
        }
    }
}
