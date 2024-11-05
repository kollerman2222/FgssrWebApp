using fgssr.Models;
using fgssr.ViewModels;
using System.Linq.Expressions;

namespace fgssr.IRepository
{
    public interface IDiplomasDepartmentsRepository
    {
        Task<DiplomasDepartments?> GetByIdAsync(int? id);

        Task<IEnumerable<DiplomasDepartments>> GetAllAsync();

        Task CreateAsync(DiplomasDepartmentsViewModel DDVM);


        void Delete(DiplomasDepartments dep);

        

    }
}
