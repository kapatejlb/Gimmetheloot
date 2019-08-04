using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public interface IManageEntities<T>
    {
        void Add(T Entity);

        void Delete(int Id);

        T Get(int Id);

        void SaveChanges();
    }
}
