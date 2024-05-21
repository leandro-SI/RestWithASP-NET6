using Microsoft.AspNetCore.Mvc;

namespace RestWithASPNET.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {

        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("sum/{firstNumber}/{secondNumber}")]
        public IActionResult Get(string firstNumber, string secondNumber)
        {

            if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
            {
                var sum = Convert.ToDecimal(firstNumber) + Convert.ToDecimal(secondNumber);
                return Ok(sum);
            }

            return BadRequest("Invalid Input.");
        }

        private bool IsNumeric(string firstNumber)
        {
            double number;
            bool isNumber = double.TryParse(firstNumber, out number);
            return isNumber;
        }
    }
}
