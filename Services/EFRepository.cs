using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Services;

public class EFRepository : IRepository
{
    private UnitOfWork _unitOfWork;

    public EFRepository(UnitOfWork unit)
    {
        _unitOfWork = unit;
        _unitOfWork.Database.EnsureCreated();
    }

    public async Task AddAsync(IDomainObject obj)
    {
        if (obj is TaskItem task)
            await _unitOfWork.Tasks.AddAsync(task);
        else if (obj is User user)
            await _unitOfWork.Users.AddAsync(user);
    }

    public async Task<IDomainObject?> GetTaskAsync(int id)
    {
        return await _unitOfWork.Tasks.FindAsync(id);
    }

    public async Task<IEnumerable<IDomainObject>> GetTasksAsync()
    {
        return await _unitOfWork.Tasks
                        .Include(task => task.Performer)
                        .OrderBy(t => t.Status)
                        .ThenBy(i => i.Id)
                        .ToListAsync();
    }

    public async Task<IDomainObject?> GetUserAsync(int id)
    {
        return await _unitOfWork.Users.FindAsync(id);
    }

    public async Task<IEnumerable<IDomainObject>> GetUsersAsync()
    {
        return await _unitOfWork.Users.ToListAsync();
    }

    public async Task LoadTasksAsync()
    {
        await _unitOfWork.Tasks.LoadAsync();
    }

    public async Task LoadUsersAsync()
    {
        await _unitOfWork.Users.LoadAsync();
    }

    public void Remove(IDomainObject obj)
    {
        if (obj is TaskItem task)
            _unitOfWork.Tasks.Remove(task);
        else if (obj is User user)
            _unitOfWork.Users.Remove(user);
    }

    public async Task SaveAsync()
    {
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(IDomainObject obj)
    {
        _unitOfWork.Entry(obj).State = EntityState.Modified;
        await _unitOfWork.SaveChangesAsync();
    }
}
