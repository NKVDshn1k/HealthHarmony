using HealthHarmony.API.Context;
using HealthHarmony.API.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HealthHarmony.API.Repository;

public class UsersRepository : IRepository<User>
{
    protected readonly HealthHarmonyContext _db;
    protected readonly DbSet<User> _set;

    public bool AutoSaveChanges { get; set; } = true;

    public UsersRepository(HealthHarmonyContext db)
    {
        _db = db;
        _set = db.Set<User>();
    }

    public IQueryable<User> Items => 
        _set;


    public IEnumerable<User> Get() =>
        Items.ToArray();

    public async Task<IEnumerable<User>> GetAsync(CancellationToken Cancel = default) =>
        await Items.ToArrayAsync(Cancel);

    public virtual User Get(int id) =>
    Items.FirstOrDefault(x => x.Id.Equals(id));

    public virtual async Task<User> GetAsync(int id, CancellationToken Cancel = default) =>
        await Items.FirstOrDefaultAsync(x => x.Id.Equals(id), Cancel);

    public virtual User Add(User item)
    {
        try
        {
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
        return item;
    }

    public virtual async Task<User> AddAsync(User item, CancellationToken Cancel = default)
    {
        try
        {
            
            await _db.AddAsync(item);
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }

        return item;
    }

    public virtual void Remove(int id)
    {
        var item = Get(id);
        try
        {
            _db.Remove(item);
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual async Task RemoveAsync(int id, CancellationToken Cancel = default)
    {
        var item = await GetAsync(id);
        try
        {
            _db.Remove(item);
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual void Update(User item)
    {
        try
        {
            _db.Update(item);
            if (AutoSaveChanges)
                _db.SaveChanges();
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public virtual async Task UpdateAsync(User item, CancellationToken Cancel = default)
    {
        try
        {
            _db.Update(item);
            if (AutoSaveChanges)
                await _db.SaveChangesAsync(Cancel);
        }
        catch (Exception ex)
        {
            _db.Entry(item).State = EntityState.Detached;
            throw ex;
        }
    }

    public async Task SaveChanges() =>
        _db.SaveChanges();
}
