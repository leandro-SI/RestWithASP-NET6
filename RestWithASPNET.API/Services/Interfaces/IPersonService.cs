using RestWithASPNET.API.Hypermedia.Utils;
using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Services.Interfaces
{
    public interface IPersonService
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindById(int id);
        List<PersonDTO> FindAll();
        PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        PersonDTO Update(PersonDTO person);
        PersonDTO Disable(int id);
        List<PersonDTO> FindByName(string name);
        void Delete(int id);
    }
}
