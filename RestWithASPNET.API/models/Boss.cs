using RestWithASPNET.API.models.Base;

namespace RestWithASPNET.API.models
{
    public class Boss : BaseEntity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}
