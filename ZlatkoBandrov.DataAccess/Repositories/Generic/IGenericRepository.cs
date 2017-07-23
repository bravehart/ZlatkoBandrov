using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ZlatkoBandrov.DataAccess.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> AsQuery(Expression<Func<T, bool>> predicate);
        IEnumerable<T> AsList(Expression<Func<T, bool>> predicate);
        T Get(Expression<Func<T, bool>> predicate);
        void Add(params T[] items);
        void Add(T item);
        void Update(params T[] items);
        void Update(T item);
        void Remove(params T[] items);
        void Remove(T item);
    }
}
