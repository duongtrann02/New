using System.ComponentModel.DataAnnotations;

namespace New.Models;

public class Post{
    public string Id {get; set;}
    public string Author {get; set;}
    public string Content {get; set;}
    public DateTime PostedDate {get; set;}
    public int Likes {get; set;}
    public List<string> LikedBy {get; set;}
    public int Shares {get; set;}
    public List<Comment> Comments {get; set;}
    public int CommentsCount{
        get {return Comments != null ? Comments.Count : 0; }
    }
    public Post(){
        Id = Guid.NewGuid().ToString();
        Author = this.Author;
        PostedDate = DateTime.Now;
        Likes = 0;
        Shares = 0;
        Comments = new List<Comment>();
        LikedBy = new List<string>();
    }
    public void Like(string userID){
        if(!LikedBy.Contains(userID)){
            Likes++;
            LikedBy.Add(userID);
        }
    }
    public void UnLike(string userID){
        if(LikedBy.Contains(userID)){
            Likes--;
            LikedBy.Remove(userID);
        }
    }
    public void AddComment(Comment comment){
        Comments.Add(comment);
    }
}