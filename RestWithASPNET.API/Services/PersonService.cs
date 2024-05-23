using AutoMapper;
using RestWithASPNET.API.Data;
using RestWithASPNET.API.Hypermedia.Utils;
using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Repositories.Interfaces;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Services
{
    public class PersonService : IPersonService
    {

        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository, IMapper mapper)
        {
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public PersonDTO Create(PersonDTO personDto)
        {
            var personEntity = _mapper.Map<Person>(personDto);

            var result = _personRepository.Create(personEntity);

            return _mapper.Map<PersonDTO>(result);
        }

        public List<PersonDTO> FindAll()
        {
            var persons = _personRepository.FindAll();

            return _mapper.Map<List<PersonDTO>>(persons);
        }

        public PersonDTO FindById(int id)
        {
            var person = _personRepository.FindById(id);

            return _mapper.Map<PersonDTO>(person);
        }

        public void Delete(int id)
        {
            _personRepository.Delete(id);
        }

        public PersonDTO Update(PersonDTO personDto)
        {
            var personEntity = _mapper.Map<Person>(personDto);

            var result = _personRepository.Update(personEntity);

            return _mapper.Map<PersonDTO>(result);
        }

        public PersonDTO Disable(int id)
        {
            var personEntity = _personRepository.Disable(id);

            return _mapper.Map<PersonDTO>(personEntity);
        }

        public List<PersonDTO> FindByName(string name)
        {
            var personsList = _personRepository.FindByName(name);

            return _mapper.Map<List<PersonDTO>>(personsList);
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var offset = page > 0 ? (page - 1) * pageSize : 0;
            var sorte = (!string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            var size = (pageSize < 1) ? 1 : pageSize;

            //string query = @"SELECT * FROM Persons p WHERE 1 = 1 AND p.Name LIKE '%LE%' 
            //                    ORDER BY p.Name ASC LIMIT 10 offset 1";

            string query = @"SELECT * FROM Persons p WHERE 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(name))
            {
                query += $"AND p.Name LIKE '%{name}%' ";
            }

            query += $"ORDER BY p.Name {sorte} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY";

            string countQuery = @"SELECT COUNT(*) FROM Persons p WHERE 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(name))
            {
                countQuery += $"AND p.Name LIKE '%{name}%' ";
            }

            var persons = _personRepository.FindWithPagedSearch(query);
            int totalResults = _personRepository.GetCount(countQuery);


            return new PagedSearchDTO<PersonDTO> 
            {
                CurrentPage = offset,
                List = _mapper.Map<List<PersonDTO>>(persons),
                PageSize = size,
                SortDirections = sorte,
                TotalResults = totalResults
            };
        }
    }
}
