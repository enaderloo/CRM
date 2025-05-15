using CRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.API.Controllers
{
	[Authorize(Policy = "ApiScope")]
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public CustomerController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet("customers")]
		public async Task<IActionResult> GetCustomers()
		{
			var customers = await _context.Customers.ToListAsync();
			return Ok(customers);
		}

		[HttpGet]
		public IActionResult Get() => Ok("Secure Data");
	}
}
