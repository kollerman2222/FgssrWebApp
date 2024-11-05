using fgssr.Models;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class DiplomasDepartmentsViewModel
    {

        public int DepId { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Title")]
        public required string DepartmentName { get; set; }

        [StringLength(2500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public ICollection<DiplomasSections> Sections { get; set; } = new List<DiplomasSections>();

    }
}
