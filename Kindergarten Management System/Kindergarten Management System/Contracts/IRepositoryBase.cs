using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kindergarten_Management_System.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        ICollection<T> FindAll();
        T FindById(int id);

        bool isExists(int id);

        bool CreateT(T entity);

        bool UpdateT(T entity);

        bool DeleteT(T entity);

        bool Save();
    }
}
