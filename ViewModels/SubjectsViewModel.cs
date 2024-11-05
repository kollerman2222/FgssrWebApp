using fgssr.Models;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class SubjectsViewModel
    {

        public int SubjectId { get; set; }

        [MaxLength(100)]
        [Required]
        public string SubjectNameEnglish { get; set; } = string.Empty;

        [MaxLength(100)]
        [Required]
        public string SubjectNameArabic { get; set; } = string.Empty;


        [MaxLength(20)]
        public string? SubjectCode { get; set; }

        [MaxLength(100)]
        public string? ScientificDegree { get; set; }

        public int? SubjectHours { get; set; } = 3;

        public int SectionId { get; set; }

        public DiplomasSections? Sections { get; set; }


        public int CasSectionId { get; set; }

        public List<DiplomasSections>? CasSections { get; set; } = new List<DiplomasSections>();


        public int CasDepartmentId { get; set; }

        public List<DiplomasDepartments>? CasDepartments { get; set; }

    }
}
