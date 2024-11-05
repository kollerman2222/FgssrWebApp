using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface INewsRepository
    {
        Task<News?> GetByIdAsync(int? id);

        Task<IEnumerable<News>> GetAllAsync();

        Task CreateAsync(NewsViewModel NVM);

        void Delete(News news);
    }
}
