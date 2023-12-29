using System.Net;
using System.Xml.Linq;

namespace Application.Dtos.Users;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
