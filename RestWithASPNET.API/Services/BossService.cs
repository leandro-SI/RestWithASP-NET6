using RestWithASPNET.API.models;
using RestWithASPNET.API.Repositories.Generic;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Services
{
    public class BossService : IBossService
    {

        private readonly IRepository<Boss> _bossRepository;

        public BossService(IRepository<Boss> bossRepository)
        {
            _bossRepository = bossRepository;
        }

        public List<Boss> GetAll()
        {
            return _bossRepository.FindAll(); ;
        }

        public Boss Insert(Boss boss)
        {
            return _bossRepository.Create(boss);
        }
    }
}
