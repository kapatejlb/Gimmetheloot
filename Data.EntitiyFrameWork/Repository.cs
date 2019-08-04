using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Data.EntitiyFrameWork
{
    public class Repository<T> : IManageEntities<T> where T : class
    {
        private readonly DataBase1Context _DbContext;

        public Repository(DataBase1Context DbContext)
        {
            _DbContext = DbContext;
        }

        public void Add(T Entity)
        {
            var DbSet = _DbContext.Set<T>();
            DbSet.Add(Entity);
        }

        public void Delete(int Id)
        {
            var DbSet = _DbContext.Set<T>();
            var Entity = DbSet.Find(Id);
            DbSet.Remove(Entity);
        }

        public T Get(int Id)
        {
            var DbSet = _DbContext.Set<T>();
            var Result = DbSet.Find(Id);
            return Result;
        }

        public void SaveChanges()
        {
            _DbContext.SaveChanges();
        }
    }
}
