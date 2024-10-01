using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace New.Models;

public class Account{
    public string Id {get; set;}
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Username la email cua ban")]
    [EmailAddress(ErrorMessage = "Email khong phu hop")]
    public string UserName {get; set;}
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password la bat buoc")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password phai tu 6 den 50 ky tu")]
    public string PassWord {get; set;}
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ten tai khoan la bat buoc")]
    public string AccountName {get; set;}
    [DataType(DataType.Date)]
    public DateTime DateOfBirth {get; set;}
    public string Gender {get; set;}
    public string Bio {get; set;}
    public string Avatar {get; set;}
}