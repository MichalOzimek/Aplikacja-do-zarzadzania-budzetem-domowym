using Microsoft.EntityFrameworkCore;
using ProjectSoftwareWorkshop.Contracts;
using ProjectSoftwareWorkshop.Data;

namespace ProjectSoftwareWorkshop.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ProjectSoftwareWorkshopDbContext Context;

    public GenericRepository(ProjectSoftwareWorkshopDbContext context)
    {
        Context = context;
    }

    public async Task<T> AddAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> Exists(int id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<T> GetAsync(int? id)
    {
        if (id is null)
        {
            return null;
        }

        return await Context.Set<T>().FindAsync(id);
    }


    public async Task UpdateAsync(T entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }
}