using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Services.Interfaces
{
    public interface IPersonService
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindById(int id);
        List<PersonDTO> FindAll();
        PersonDTO Update(PersonDTO person);
        void Delete(int id);
    }
}
