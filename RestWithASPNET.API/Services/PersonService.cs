using RestWithASPNET.API.Data;
using RestWithASPNET.API.models;
using RestWithASPNET.API.Repositories.Interfaces;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Services
{
    public class PersonService : IPersonService
    {

        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Person Create(Person person)
        {
            var result = _personRepository.Create(person);

            return result;
        }

        public List<Person> FindAll()
        {
            return _personRepository.FindAll();
        }

        public Person FindById(int id)
        {
            return _personRepository.FindById(id);
        }

        public void Delete(int id)
        {
            _personRepository.Delete(id);
        }

        public Person Update(Person person)
        {
            var result = _personRepository.Update(person);

            return result;
        }
    }
}
