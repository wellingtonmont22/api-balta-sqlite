using System.ComponentModel.DataAnnotations;

namespace T.ViewModels
{
    public class UpdateViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
