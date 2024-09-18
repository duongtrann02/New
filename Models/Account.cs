using System.ComponentModel.DataAnnotations;

namespace New.Models;

public class Account{
    public string Id {get; set;}
    public string UserName {get; set;}
    public string PassWord {get; set;}
    public string AccountName {get; set;}
    [DataType(DataType.Date)]
    public DateTime DateOfBirth {get; set;}
    public string Gender {get; set;}
    
}