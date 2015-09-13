using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Lyra.Models.Database.Repositories
{
    public class DbRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly object _lockObj = new object();

        public DbRepository(DbContext context)
        {
            this._dbSet = context.Set<T>();
        }

        public T Add(T item)
        {
            lock (this._lockObj)
                return this._dbSet.Add(item);
        }

        public T Remove(T item)
        {
            lock (this._lockObj)
                return this._dbSet.Remove(item);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            lock (this._lockObj)
                return this._dbSet.Where(predicate);
        }

        public bool Contains(Func<T, bool> predicate)
        {
            lock (this._lockObj)
                return this._dbSet.Any(predicate);
        }

        public IEnumerable<T> ToEnumerable()
        {
            lock (this._lockObj)
                return this._dbSet.ToList();
        }
    }
}