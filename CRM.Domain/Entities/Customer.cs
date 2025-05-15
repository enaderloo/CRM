
namespace CRM.Domain.Entities
{
	public class Customer
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string? Phone { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
