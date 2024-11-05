using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface IEventsRepository
    {

        Task<Events?> GetByIdAsync(int? id);

        Task<IEnumerable<Events>> GetAllAsync();

        Task CreateAsync(EventsViewModel EVM);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(Events eve);
    }
}
