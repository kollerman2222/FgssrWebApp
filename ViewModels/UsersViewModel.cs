using fgssr.Custom_DataAnnotation;
using fgssr.Models;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class UsersViewModel
    {
        public string UserId { get; set; }

        public string FullNameEnglish { get; set; } = string.Empty;

        public string FullNameArabic { get; set; } = string.Empty;

        public string? Email { get; set; }
        public DateOnly BirthDate { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ApplicationStatus { get; set; }

        public string? ProfileImage { get; set; }

        public string GraduationCertUpload { get; set; } = string.Empty;


        public int? SectionId { get; set; }

        public string? SectionName { get; set; }

        public DiplomasSections? Section { get; set; }

        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile CreateUserImage { get; set; } = default!;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile? EditUserImage { get; set; }

        [Required]
		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile GraduationCert { get; set; }

    }
}

