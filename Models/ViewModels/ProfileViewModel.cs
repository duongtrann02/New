using System.Collections.Generic;

namespace New_Project.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
