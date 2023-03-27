// =============================================================================
// StockController.cs
// =============================================================================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCuritiba.WebApiStock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "Stock")]
    public class StockController : ControllerBase
    {
        [HttpGet("stockuser")]
        public IActionResult StockUser()
        {
            return Ok("Access granted to Stock User.");
        }
        
        [HttpGet("stockadmin")]
        [Authorize(Roles = "admin")]
        public IActionResult StockAdmin()
        {
            return Ok("Access granted to Stock Admin.");
        }
        
        [HttpGet("stockadminpolicy")]
        [Authorize(Policy = "Admin")]
        public IActionResult StockAdminPolicy()
        {
            return Ok("Access granted to Stock Admin Policy.");
        }
    }
}