using fgssr.Models;
using fgssr.ViewModels;

namespace fgssr.IRepository
{
    public interface ISubjectsMarksRepository
    {
        Task CreateAsync(SubjectsMarksViewModel SMVM);

        Task<IEnumerable<SubjectMark>> GetAllByUserIdAsync(string? id);

        void Delete(SubjectMark SM);
    }
}
