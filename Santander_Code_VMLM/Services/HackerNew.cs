
using Santander_Code_VMLM.Interfaces;
using Santander_Code_VMLM.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Santander_Code_VMLM.Services
{
    public class HackerNew : IHackerNew
    {
        private readonly ILogger<HackerNew> _logger;
        private readonly IMemoryCache _cache;
        private readonly IHackerNewsApi _hackerNewsAPI;

        public HackerNew(ILogger<HackerNew> logger, IMemoryCache cache, IHackerNewsApi hackerNewsAPI)
        {
            _logger = logger;
            _cache = cache;
            _hackerNewsAPI = hackerNewsAPI;
        }

        /// <summary>
        /// Get the indicated number of best stories from Hacker News.
        /// </summary>
        /// <param name="count">Number of stories to retrieve,</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns></returns>
        public async Task<IEnumerable<BestHistories>> GetBestStoriesAsync(int count, CancellationToken cancellationToken)
        {
            _logger.LogDebug(string.Format("<{0}> Started processing the request to retrieve {1} best stories from Hacker News.", nameof(GetBestStoriesAsync), count));

            try
            {
                // Trying to get stories from cache firstly
                var cachedStroies = _cache.Get<IEnumerable<BestHistories>>("stories");
                if (cachedStroies is not null)
                {
                    return cachedStroies.Take(count).ToList();
                }

                // if cache was flushed, request stories from Hacker News
                var bestStoriesIds = await _hackerNewsAPI.GetBestStoriesIdsAsync(cancellationToken);
                var bestStories = bestStoriesIds.Select(id =>
                {
                    return _hackerNewsAPI.GetStoryAsync(id, cancellationToken);
                }).ToArray();

                var result = await Task.WhenAll(bestStories);

                _cache.Set("stories", result, TimeSpan.FromMinutes(3));

                return result.Take(count);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);

                return await Task.FromException<IEnumerable<BestHistories>>(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return await Task.FromException<IEnumerable<BestHistories>>(ex);
            }
        }
    }
}

