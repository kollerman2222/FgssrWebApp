using fgssr.Custom_DataAnnotation;
using System.ComponentModel.DataAnnotations;

namespace fgssr.ViewModels
{
    public class EventsViewModel
    {
        public int EveId { get; set; }
        public required string EventTitle { get; set; }

        public string EventImageName { get; set; } = string.Empty;

        [StringLength(2500)]
        public string EventDescription { get; set; } = string.Empty;

        [StringLength(1500)]
        public string EventLocation { get; set; } = string.Empty;

        public int Time { get; set; }

        public int DateDay { get; set; }

        [StringLength(150)]
        public string DateMonth { get; set; } = string.Empty;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile CreateEventImage { get; set; } = default!;

		[MaxFileSize(1)]
		[AllowedFileExtentions(".jpg,.jpeg,.png")]
		public IFormFile? EditEventImage { get; set; }

    }
}
