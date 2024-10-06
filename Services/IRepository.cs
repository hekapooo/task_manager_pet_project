using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services;

public interface IRepository
{
    public Task AddAsync(IDomainObject obj);

    public Task<IDomainObject?> GetTaskAsync(int id);

    public Task<IEnumerable<IDomainObject>> GetTasksAsync();

    public Task<IDomainObject?> GetUserAsync(int id);

    public Task<IEnumerable<IDomainObject>> GetUsersAsync();

    public Task LoadTasksAsync();

    public Task LoadUsersAsync();

    public void Remove(IDomainObject obj);

    public Task SaveAsync();

    public Task UpdateAsync(IDomainObject obj);
}
