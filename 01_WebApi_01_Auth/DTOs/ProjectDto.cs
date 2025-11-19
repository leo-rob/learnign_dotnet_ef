using System.Text.Json.Serialization;

namespace Core.Dtos
{
	public readonly struct ProjectRequestDto
	{
		public int? Id { get; init; }
		public string? Name { get; init; }
		public int? CategoryId { get; init; }
		public Guid? ManagerId { get; init; }
		public DateTime? StartDate { get; init; }
		public DateTime? EndDate { get; init; }
	}
	
	public record ProjectResponseBaseDto
	{
		public int Id { get; init; }
		public string? Name { get; init; }
		public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? CategoryId { get; init; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Guid? ManagerId { get; init; }
		
		public static ProjectResponseBaseDto From(Entities.Project? project)
		{
			if (project == null) return null!;
			
			return new ProjectResponseBaseDto
			{
				Id = project.Id,
				Name = project.Name,
				StartDate = project.StartDate,
				EndDate = project.EndDate,
				CategoryId = project.CategoryId,
				ManagerId = project.ManagerId
			};
		}
	}
	
    public sealed record ProjectResponseFullDto : ProjectResponseBaseDto
    {
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProjectCategoryDto? Category { get; init; }
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public UserResponseBaseDto? Manager { get; init; }
		
		public static new ProjectResponseFullDto From(Entities.Project? project)
		{
			if (project == null) return null!;
			
			return new ProjectResponseFullDto
			{
				Id = project.Id,
				Name = project.Name,
				StartDate = project.StartDate,
				EndDate = project.EndDate,

				CategoryId = project.Category != null ? null : project.CategoryId,
				Category = ProjectCategoryDto.From(project.Category),
				ManagerId = project.User != null ? null : project.ManagerId,
				Manager = UserResponseFullDto.From(project.User),
			};
		}
    }
}
