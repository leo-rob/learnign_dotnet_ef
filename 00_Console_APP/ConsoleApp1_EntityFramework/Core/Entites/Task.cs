using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entites
{	
	[Table("tasks")]
	public record Task
	{
		[Key, Column("id")]
		public int Id { get; init; }
		[Column("title")]
		public required string Title { get; init; }
		[Column("description")]
		public string Description { get; init; } = string.Empty;
		[Column("status")]
		public required string Status { get; init; } = string.Empty;
		[Column("priority")]
		public required string Priority { get; init; } = string.Empty;
		[Column("due_time")]
		public DateTime DueTime { get; init; }
		
		[Column("project_id")]
		public int ProjectId { get; init; }
		public Project? Project { get; init; }
		
		[Column("assigned_to_user_id")]
		public int AssignedToUser { get; init; }
		public User? User { get; init; }
	}
}