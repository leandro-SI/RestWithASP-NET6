using RestWithASPNET.API.Data;
using RestWithASPNET.API.models;
using RestWithASPNET.API.Repositories.Interfaces;

namespace RestWithASPNET.API.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ProjetoContext _context;

        public PersonRepository(ProjetoContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            _context.Add(person);
            _context.SaveChanges();

            return person;
        }

        public List<Person> FindAll()
        {
            return _context.Persons.ToList();
        }

        public Person FindById(int id)
        {
            return _context.Persons.FirstOrDefault(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var person = _context.Persons.FirstOrDefault(x => x.Id == id);

            _context.Persons.Remove(person);
            _context.SaveChanges();
        }

        public Person Update(Person person)
        {
            _context.Persons.Update(person);

            return person;
        }
    }
}
