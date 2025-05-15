using CRM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRM.IdentityServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(string username, string password)
		{
			var user = new ApplicationUser
			{
				UserName = username,
				Email = username
			};

			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
				return Ok("User created successfully");

			return BadRequest(result.Errors);
		}
	}
}
