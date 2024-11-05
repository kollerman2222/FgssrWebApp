using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface ISubjectsRepository
    {
        Task<Subjects?> GetByIdAsync(int? id);

        Task<IEnumerable<Subjects>> GetAllAsync();

        Task<IEnumerable<Subjects>> GetAllBySectionNameAsync(string sectionName);

        Task CreateAsync(SubjectsViewModel SVM);

        void Delete(Subjects SVM);

    }
}
