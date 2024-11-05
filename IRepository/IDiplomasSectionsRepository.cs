using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface IDiplomasSectionsRepository
    {
        Task<DiplomasSections?> GetByIdAsync(int? id);

        Task<DiplomasSections?> GetByNameAsync(string name);

        Task<IEnumerable<DiplomasSections>> GetAllAsync();

        Task CreateAsync(DiplomasSectionsViewModel DSVM);


        Task<string?> GetNamebyIdAsync(int? id);

        Task<string> SaveNewImage(IFormFile image);

        string uploadFolderPublic { get; }

        void Delete(DiplomasSections DS);

    }
}
