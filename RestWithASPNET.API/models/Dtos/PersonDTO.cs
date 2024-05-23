using RestWithASPNET.API.Hypermedia;
using RestWithASPNET.API.Hypermedia.Abstract;

namespace RestWithASPNET.API.models.Dtos
{
    public class PersonDTO : ISupportsHyperMedia
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public bool Enabled { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
