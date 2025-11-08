using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entites
{	
	[Table("users")]
	public record User
	{
		[Key, Column("id")]
		public int Id { get; init; }
		[Column("username")]
		public required string Username { get; init; }
		[Column("password")]
		public required string Password { get; init; }
		[Column("email")]
		public required string Email { get; init; }
		[Column("first_name")]
		public string? FirstName { get; init; }
		[Column("last_name")]
		public string? LastName { get; init; }
		
		[Column("role_id")]
		[ForeignKey("role_id")]
		[Required]
		public Role? Role { get; init; }
		
		public List<Project>? Projects { get; init; }
	}
}