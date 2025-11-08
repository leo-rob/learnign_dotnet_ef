
namespace Core.Dtos
{
	public record ProjectRequestDto
	{
		public int? Id { get; init; }
		public string? Name { get; init; }
		public int? CategoryId { get; init; }
		public int? ManagerId { get; init; }
		public DateTime? StartDate { get; init; }
		public DateTime? EndDate { get; init; }
	}
	
    public record ProjectResponseDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        // public int? CategoryId { get; init; }
        public ProjectCategoryDto? Category { get; init; }
        public int? ManagerId { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
		
		public static ProjectResponseDto From(Entities.Project? project)
		{
			if (project == null) return null!;
			
			return new ProjectResponseDto
			{
				Id = project.Id,
				Name = project.Name,
				// CategoryId = project.CategoryId,
				Category = ProjectCategoryDto.From(project.Category),
				ManagerId = project.ManagerId,
				StartDate = project.StartDate,
				EndDate = project.EndDate
			};
		}
    }
}
