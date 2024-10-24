using Santander_Code_VMLM.Models;

namespace Santander_Code_VMLM.Interfaces
{
    public interface IHackerNewsApi
    {
        Task<IEnumerable<int>> GetBestStoriesIdsAsync(CancellationToken cancellationToken);
        Task<BestHistories> GetStoryAsync(int id, CancellationToken cancellationToken);
    }
}
