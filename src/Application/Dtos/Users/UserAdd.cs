using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Users;

public class UserAdd
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Address { get; set; }
}
