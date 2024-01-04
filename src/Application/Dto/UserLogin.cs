using System.ComponentModel.DataAnnotations;

namespace Application.Dto;

public class UserLogin
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
}
