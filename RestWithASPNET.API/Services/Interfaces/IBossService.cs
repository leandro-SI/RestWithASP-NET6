using RestWithASPNET.API.models;

namespace RestWithASPNET.API.Services.Interfaces
{
    public interface IBossService
    {
        List<Boss> GetAll();
        Boss Insert(Boss boss);
    }
}
