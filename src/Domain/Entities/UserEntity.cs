namespace Domain.Entities;

public class UserEntity : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
