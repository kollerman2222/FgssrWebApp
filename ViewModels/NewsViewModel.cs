using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace fgssr.ViewModels
{
    public class NewsViewModel
    {

        public int NId { get; set; }

        [StringLength(250)]
        public string NewsTitle { get; set; } = string.Empty;

        [StringLength(2500)]
        public string NewsDescription { get; set; } = string.Empty;

       
        public DateOnly? NewsDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    }
}
