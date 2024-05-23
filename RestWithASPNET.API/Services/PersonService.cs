using AutoMapper;
using RestWithASPNET.API.Data;
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
    }
}
