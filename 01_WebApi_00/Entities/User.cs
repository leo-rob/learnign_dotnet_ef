using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{	
	[Table("users")]
	public record User
	{
		[Key, Column("id")]
		public int Id { get; init; }
		[Column("username")]
		public required string Username { get; set; }
		[Column("password")]
		public required string Password { get; set; }
		[Column("email")]
		public required string Email { get; set; }
		[Column("first_name")]
		public string? FirstName { get; set; }
		[Column("last_name")]
		public string? LastName { get; set; }
		
		[Column("role_id"), Required]
		public required int RoleId { get; set; }
		
		[ForeignKey(nameof(RoleId))]
		public Role? Role { get; set; }
		
		public List<Project>? Projects { get; set; }
		public List<Task>? Tasks { get; set; }
	}
}