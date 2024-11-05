using fgssr.Custom_DataAnnotation;
using fgssr.Models;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class AdmissionViewModel
    {
        public string userId { get; set; }

        public string? sectionName { get; set; }
        public string? Email { get; set; }
        public int? sectionId { get; set; }
        public string? FullNameEnglish { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NationalId { get; set; }
        public string? ApplicationStatus { get; set; }
        public string? GraduationCertUpload { get; set; }

        public string? Address { get; set; }

        public string? FullNameArabic{ get; set; }

        //public DiplomasSections? Section { get; set; }

        [Required]
        [MaxFileSize(1)]
        [AllowedFileExtentions(".jpg,.jpeg,.png,.pdf")]
        public IFormFile GraduationCert { get; set; }

        public List<ApplicationUser>? UsersList { get; set; }

        public bool isEmailConfirmed { get; set; }
        public bool isPhoneConfirmed { get; set; }

        [MaxLength(300)]
        [Required]
        public string DeclineReason { get; set; } 

        [MaxLength(20)]
        public string? AdmissionNumber { get; set; }

        [MaxLength(20)]
        public string? StudentNumber { get; set; }


        public IEnumerable<DiplomasSections>? sections { get; set; }

    }
}
