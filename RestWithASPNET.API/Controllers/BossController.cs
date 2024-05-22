using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.API.models;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BossController : ControllerBase
    {
        private readonly IBossService _bossService;

        public BossController(IBossService bossService)
        {
            _bossService = bossService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            var bosses = _bossService.GetAll();

            if (bosses == null)
                return NotFound();

            return Ok(bosses);
        }

        [HttpPost]
        public IActionResult Create(Boss boss)
        {
            var result = _bossService.Insert(boss);

            return Ok(result);
        }
    }
}
