using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Base;

namespace RestWithASPNET.API.Repositories.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T person);
        T FindById(int id);
        List<T> FindAll();
        T Update(T person);
        void Delete(int id);
    }
}
