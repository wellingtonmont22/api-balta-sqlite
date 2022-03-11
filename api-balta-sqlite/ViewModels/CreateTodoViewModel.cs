using System.ComponentModel.DataAnnotations;

namespace T.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
