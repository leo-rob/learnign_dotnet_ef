using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{	
	[Table("roles")]
	public class Role : IdentityRole<Guid>
	{
		
	}
}