
namespace Core.Dtos
{
	public record TaskDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public string Priority { get; set; } = string.Empty;
		public DateTime? DueDate { get; set; } = null;
		
		public ProjectResponseDto? Project { get; set; }
		public UserResponseDto? User { get; set; }
		
		
		public static TaskDto From(Entities.Task? task)
		{
			if (task == null) return null!;
			
			return new TaskDto
			{
				Id = task.Id,
				Title = task.Title,
				Description = task.Description,
				Status = task.Status,
				Priority = task.Priority,
				DueDate = task.DueDate,
				Project = ProjectResponseDto.From(task.Project),
				User = UserResponseDto.From(task.User)
			};
		}
	}
}