using AutoMapper;
using RestWithASPNET.API.models;
using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
        }
    }
}
