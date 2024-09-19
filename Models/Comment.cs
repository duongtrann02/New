using System.ComponentModel.DataAnnotations;

namespace New.Models;

public class Comment{
    public string Id {get; set;}
    public string Author {get; set;}
    public string Content {get; set;}
    public DateTime CommentedDate {get; set;}
    public Comment(string author, string content){
        Id = Guid.NewGuid().ToString();
        Author = author;
        Content = content;
        CommentedDate = DateTime.Now;
    }
    
}