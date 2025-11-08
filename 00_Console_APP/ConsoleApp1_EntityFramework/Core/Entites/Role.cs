
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entites
{	
	[Table("roles")]
	public record Role
	{
		[Column("id")]
		[Key]
		public int RoleId { get; set; }
		
		[Column("name")]
		public required string RoleName { get; set; }
	}
}