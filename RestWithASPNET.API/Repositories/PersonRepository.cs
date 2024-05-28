using Microsoft.EntityFrameworkCore;
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
            _context.SaveChanges();

            return person;
        }

        public Person Disable(int id)
        {
            if (!_context.Persons.Any(p => p.Id.Equals(id)))
                return null;

            var user = _context.Persons.SingleOrDefault(p =>  p.Id == id);

            if (user != null)
            {
                user.Enabled = false;

                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return user;
        }

        public List<Person> FindByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return _context.Persons.Where(p => p.Name.Contains(name)).ToList();
            }
            return null;
            
        }

        public List<Person> FindWithPagedSearch(string query)
        {
            return _context.Persons.FromSqlRaw(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }

            return int.Parse(result);
        }
    }
}
