using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Users;

public class UserAdd
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Address { get; set; }
}
