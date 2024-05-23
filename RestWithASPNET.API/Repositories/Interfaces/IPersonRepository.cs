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
        Person Disable(int id);
        List<Person> FindByName(string name);
        List<Person> FindWithPagedSearch(string query);
        int GetCount(string query);

    }
}
