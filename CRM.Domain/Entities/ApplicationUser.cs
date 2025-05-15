using Microsoft.AspNetCore.Identity;

namespace CRM.Domain.Entities;


public class ApplicationUser : IdentityUser
{
	public string? FullName { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

