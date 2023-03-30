// =============================================================================
// Sale.cs
// =============================================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCuritiba.WebApiSales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "Sale")]
    public class SaleController : ControllerBase
    {
        [HttpGet("saleuser")]
        public IActionResult SalesUser()
        {
            return Ok("Access granted to Sale User.");
        }
        
        [HttpGet("saleadmin")]
        //[Authorize(Roles = "admin")]
        public IActionResult SalesAdmin()
        {
            return Ok("Access granted to Sale Admin.");
        }
    }
}