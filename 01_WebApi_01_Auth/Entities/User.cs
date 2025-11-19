using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{	
	[Table("users")]
	public class User : IdentityUser<Guid>
	{
		[Column("first_name")]
		public string? FirstName { get; set; }
		[Column("last_name")]
		public string? LastName { get; set; }
		
		
		public List<Project>? Projects { get; set; }
		public List<Task>? Tasks { get; set; }
	}
}