
using System.Text.Json.Serialization;

namespace Core.Dtos
{
	public readonly struct TaskRequestDto
	{
		public int? Id { get; init; }
		public string? Title { get; init; }
		public string? Description { get; init; }
		public string? Status { get; init; }
		public string? Priority { get; init; }
		public DateTime? DueDate { get; init; }
		
		public int? ProjectId { get; init; }
		public int? UserId { get; init; }
	}
	
	public record TaskResponseBaseDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string Priority { get; set; } = string.Empty;
		public DateTime? DueDate { get; set; } = null;
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? ProjectId { get; set; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Guid? UserId { get; set; }
		
		public static TaskResponseBaseDto From(Entities.Task? task)
		{
			if (task == null) return null!;
			
			return new TaskResponseBaseDto
			{
				Id = task.Id,
				Title = task.Title,
				Description = task.Description,
				Status = task.Status,
				Priority = task.Priority,
				DueDate = task.DueDate,
				ProjectId = task.ProjectId,
				UserId = task.AssignedToUserId
			};
		}
	}
	
	public sealed record TaskResponseFullDto : TaskResponseBaseDto
	{
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ProjectResponseFullDto? Project { get; set; }
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public UserResponseFullDto? User { get; set; }
		
		
		public static new TaskResponseFullDto From(Entities.Task? task)
		{
			if (task == null) return null!;
			
			return new TaskResponseFullDto
			{
				Id = task.Id,
				Title = task.Title,
				Description = task.Description,
				Status = task.Status,
				Priority = task.Priority,
				DueDate = task.DueDate,
				
				ProjectId = task.Project != null ? null : task.ProjectId,
				Project = ProjectResponseFullDto.From(task.Project),
				UserId = task.User != null ? null : task.AssignedToUserId,
				User = UserResponseFullDto.From(task.User)
			};
		}
	}
}