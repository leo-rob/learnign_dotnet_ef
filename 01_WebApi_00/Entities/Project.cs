
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{	
	[Table("projects")]
	public record Project
	{
		[Column("id"), Required]
		public int Id { get; set; }
		[Column("name"), Required]
		public required string Name { get; set; }
		
		[Column("category_id")]
		public required int CategoryId { get; set; }
		
		[ForeignKey(nameof(CategoryId))]
		public ProjectCategory? Category { get; set; }
		
		[Column("manager_id")]
		public required int ManagerId { get; set; }
		
		[ForeignKey(nameof(ManagerId))]
		public User? User { get; set; }
		
		
		[Column("start_date")]
		public DateTime? StartDate { get; set; }
		[Column("end_date")]
		public DateTime? EndDate { get; set; }
	}
}