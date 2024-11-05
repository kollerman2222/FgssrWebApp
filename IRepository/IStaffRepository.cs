using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface IStaffRepository
    {
        Task<Staff?> GetByIdAsync(int? id);

        Task<IEnumerable<Staff>> GetAllAsync();

        Task CreateAsync(StaffViewModel SVM);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(Staff staff);
    }
}
