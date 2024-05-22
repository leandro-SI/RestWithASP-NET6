using RestWithASPNET.API.models;

namespace RestWithASPNET.API.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Person Create(Person person);
        Person FindById(int id);
        List<Person> FindAll();
        Person Update(Person person);
        void Delete(int id);
    }
}
