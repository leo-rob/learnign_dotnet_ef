
namespace Core.Dtos
{
    public record RoleDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }

		internal static RoleDto From(Entities.Role role)
		{
			if (role == null) return null!;
			
			return new RoleDto
			{
				Id = role.RoleId,
				Name = role.RoleName
			};
		}
	}
}
