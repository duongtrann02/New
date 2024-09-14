using System.ComponentModel.DataAnnotations;

namespace New.Models;

public class Account{
    public string Id {get; set;}
    [Required(ErrorMessage = "Ô này không được để trống!")]
    public string UserName {get; set;}
    [Required(ErrorMessage = "Ô này không được để trống!")]
    public string PassWord {get; set;}
    [Required(ErrorMessage = "Ô này không được để trống!")]
    public string AccountName {get; set;}
    [Required(ErrorMessage = "Ô này không được để trống!")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth {get; set;}
    [Required(ErrorMessage = "Ô này không được để trống!")]
    public string Gender {get; set;}
    
}