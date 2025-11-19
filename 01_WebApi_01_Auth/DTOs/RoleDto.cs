
namespace Core.Dtos
{
    public record RoleDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }

		public static RoleDto From(Entities.Role? role)
		{
			if (role == null) return null!;
			
			return new RoleDto
			{
				Id = role.Id,
				Name = role.Name
			};
		}
	}
}
