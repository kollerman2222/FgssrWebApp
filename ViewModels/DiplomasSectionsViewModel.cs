using fgssr.Custom_DataAnnotation;
using fgssr.Models;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class DiplomasSectionsViewModel
    {
        public int SecId { get; set; }


        [StringLength(200)]
        [Required]
        [Display(Name = "Diploma Name")]
        public  string SectionName { get; set; }

        public string SectionImage { get; set; } = string.Empty;

        [StringLength(2500)]
        public string? Description { get; set; }

        public bool isActive { get; set; } = true;

        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png")]
        public IFormFile CreateSectionImage { get; set; } = default!;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile? EditSectionImage { get; set; } = default!;

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        
        public DiplomasDepartments? Department { get; set; }


        public int TotalPages { get; set; }

        public int Page { get; set; }

        public string? departmentSearch { get; set; }

        public string? query { get; set; }
        public List<Subjects>? Subjects { get; set; }

        public List<DiplomasSections> secs { get; set; } = new List<DiplomasSections>();

    }
}
