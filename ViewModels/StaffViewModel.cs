using fgssr.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class StaffViewModel
    {

        public int StID { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; } = string.Empty;

        [Display(Name = "Position")]

        public string StaffPosition { get; set; } = string.Empty;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile CreateStaffImage { get; set; } = default!;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile? EditStaffImage { get; set; }

        [StringLength(2500)]
        [Display(Name = "Biograpghy")]
        public string Biograpghy { get; set; } = string.Empty;

        public string StaffImageName { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
