using System.ComponentModel.DataAnnotations;

namespace New_Project.Models.ViewModels
{
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
