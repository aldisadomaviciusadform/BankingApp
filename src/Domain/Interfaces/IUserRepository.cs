using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity?> Get(Guid id);

    public Task<IEnumerable<UserEntity>> Get();

    public Task<Guid> Add(UserEntity item);

    public Task<int> Update(UserEntity item);

    public Task Delete(Guid id);
}