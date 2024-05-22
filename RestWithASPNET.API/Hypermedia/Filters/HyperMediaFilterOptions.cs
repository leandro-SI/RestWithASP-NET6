using RestWithASPNET.API.Hypermedia.Abstract;

namespace RestWithASPNET.API.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentRespondeEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
