using Santander_Code_VMLM.Models;

namespace Santander_Code_VMLM.Interfaces
{
    public interface IHackerNew
    {
        Task<IEnumerable<BestHistories>> GetBestStoriesAsync(int count, CancellationToken cancellationToken);
    }
}


