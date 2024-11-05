using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fgssr.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }

        [Required]
        public string MessageContent { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public string SenderId { get; set; } = string.Empty;
       
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
      
    }
}
