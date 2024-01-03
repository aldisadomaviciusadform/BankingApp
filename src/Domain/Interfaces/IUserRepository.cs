using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity?> Get(Guid id);
    public Task<IEnumerable<UserEntity>> Get();
    public Task<Guid> Add(UserEntity user);
    public Task<int> Update(UserEntity user);
    public Task Delete(Guid id);
}