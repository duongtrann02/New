using System.Collections.Generic;

namespace New.Models
{
    public class ProfileViewModel
    {
        public Account Account { get; set; }
        public List<Post> Posts { get; set; } // Giả sử bạn có model Post
    }
}