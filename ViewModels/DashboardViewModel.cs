using fgssr.Models;

namespace fgssr.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<DiplomasDepartments> Departments { get; set; }

        public IEnumerable<DiplomasSections> Sections { get; set; }

        public IEnumerable<Events> Events { get; set; }

        public IEnumerable<News> News { get; set; }

        public IEnumerable<Staff> Staff { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }

       
    }
}
