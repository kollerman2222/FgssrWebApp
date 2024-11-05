using fgssr.Data;
using fgssr.IRepository;
using fgssr.Models;
using fgssr.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace fgssr.Repository
{
    public class SubjectsRepository:ISubjectsRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(SubjectsViewModel SVM)
        {

            Subjects ss = new()
            {
                SubjectNameEnglish = SVM.SubjectNameEnglish,
                SubjectNameArabic = SVM.SubjectNameArabic,
                SubjectCode = SVM.SubjectCode,
                ScientificDegree = SVM.ScientificDegree,
                SectionId = SVM.CasSectionId,
                SubjectHours=SVM.SubjectHours,
                
            };
            await _context.Subjects.AddAsync(ss);

        }


        public void Delete(Subjects ss)
        {

            _context.Subjects.Remove(ss);
        }


        public async Task<IEnumerable<Subjects>> GetAllAsync()
        {
            var subjects = await _context.Subjects.Include(ss => ss.Section.Department).ToListAsync();
            return subjects;
        }


        public async Task<IEnumerable<Subjects>> GetAllBySectionNameAsync(string sectionName)
        {
            var subjects = await _context.Subjects.Where(ss => ss.Section.SectionName==sectionName).ToListAsync();
            return subjects;
        }


        public async Task<Subjects?> GetByIdAsync(int? id)
        {
            var subjects = await _context.Subjects.Include(ss => ss.Section.Department).FirstOrDefaultAsync(ss => ss.SubjectId == id);
            return subjects;
        }




    }
}
