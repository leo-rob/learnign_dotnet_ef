
namespace Core.Dtos
{
    public record ProjectCategoryDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
		
		public static ProjectCategoryDto From(Entities.ProjectCategory? category)
		{
			if (category == null) return null!;
			
			return new ProjectCategoryDto
			{
				Id = category.Id,
				Name = category.Name
			};
		}
    }
}
