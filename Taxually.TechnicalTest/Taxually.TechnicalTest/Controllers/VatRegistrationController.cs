using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Application.Interfaces.VatRegistration;
using Taxually.TechnicalTest.Application.VatRegistration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatRegistrationStrategyContext _strategyContext;

        public VatRegistrationController(IVatRegistrationStrategyContext strategyContext)
        {
            _strategyContext = strategyContext;
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            await _strategyContext.ExecuteStrategyAsync(request);
            return Ok();
        }
    }
}
