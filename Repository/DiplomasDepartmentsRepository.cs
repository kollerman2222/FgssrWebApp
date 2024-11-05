using fgssr.Data;
using fgssr.IRepository;
using fgssr.Models;
using fgssr.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace fgssr.Repository
{
    public class DiplomasDepartmentsRepository : IDiplomasDepartmentsRepository
    {
        private readonly ApplicationDbContext _context;

        public DiplomasDepartmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DiplomasDepartmentsViewModel DDVM)
        {

            DiplomasDepartments dd = new()
            {
               DepartmentName = DDVM.DepartmentName,
               Description = DDVM.Description,

            };
             await _context.DiplomasDepartments.AddAsync(dd);
            
        }

        public void Delete(DiplomasDepartments dep)
        {
                 
             _context.DiplomasDepartments.Remove(dep);        
        }

        public async Task<IEnumerable<DiplomasDepartments>> GetAllAsync()
        {
            var departments = await _context.DiplomasDepartments.Include(dep => dep.Sections).ToListAsync();
            return departments;
        }

        public async Task<DiplomasDepartments?> GetByIdAsync(int? id)
        {
            var departments = await _context.DiplomasDepartments.FindAsync(id);
           
           return departments;                    
        }

    }
}
