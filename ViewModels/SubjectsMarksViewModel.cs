using fgssr.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace fgssr.ViewModels
{
    public class SubjectsMarksViewModel
    {
        public string StudentId { get; set; }

        public int SubjectId { get; set; }

        public string? Grade { get; set; }

        public int? Percentage { get; set; }

        public int Year { get; set; } = DateTime.Now.Year;

        public string? SectionName { get; set; }

    }
}
