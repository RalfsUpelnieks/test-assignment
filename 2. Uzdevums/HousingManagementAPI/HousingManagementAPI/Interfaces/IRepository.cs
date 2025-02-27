﻿using System.Linq.Expressions;

namespace HousingManagementAPI.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        IQueryable<T> Get(Expression<Func<T, bool>> filter);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Save();

        Task SaveAsync();
    }
}
