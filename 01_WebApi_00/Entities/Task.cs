using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{	
	[Table("tasks")]
	public record Task
	{
		[Key, Column("id")]
		public int Id { get; set; }
		[Column("title")]
		public required string Title { get; set; }
		[Column("description")]
		public required string Description { get; set; }
		[Column("status")]
		public required string Status { get; set; }
		[Column("priority")]
		public required string Priority { get; set; }
		[Column("due_date")]
		public DateTime? DueDate { get; set; }
		
		[Column("project_id")] public int ProjectId { get; set; }
		[ForeignKey(nameof(ProjectId))] public Project? Project { get; set; }
		
		[Column("assigned_to_user_id")] public int AssignedToUserId { get; init; }
		[ForeignKey(nameof(AssignedToUserId))] public User? User { get; init; }
	}
}