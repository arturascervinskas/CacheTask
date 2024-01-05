using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto;

public class User
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public int Role { get; set; }
}
