
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entites
{	
	[Table("projects")]
	public record Project
	{
		[Column("id"), Required]
		public required int Id { get; init; }
		[Column("name"), Required]
		public required string Name { get; init; }
		
		[Column("category_id"), Required]
		public required int CategoryId { get; init; }

		[ForeignKey(nameof(CategoryId)), Required]
		public required ProjectCategory Category { get; init; }

		[Column("manager_id"), Required]
		public required int ManagerId { get; init; }
		
		[ForeignKey(nameof(ManagerId)), Required]
		public required User User { get; init; }
		
		
		[Column("start_date")]
		public DateTime? StartDate { get; init; }
		[Column("end_date")]
		public DateTime? EndDate { get; init; }
	}
}