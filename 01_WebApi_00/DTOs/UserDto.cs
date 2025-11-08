
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Dtos
{
	public record UserRequestDto
	{
		public int? Id { get; init; }
		public string? Username { get; init; }
		public string? Password { get; init; }
		[EmailAddress]
		public string? Email { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		public int? RoleId { get; init; }
	}
	
	public record UserResponseBaseDto
	{
		public int Id { get; init; }
		public string? Username { get; init; }
		public string? Email { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? RoleId { get; init; }

		public static UserResponseBaseDto From(Entities.User? user)
		{
			if (user == null) return null!;

			return new UserResponseBaseDto
			{
				Id = user.Id,
				RoleId = user.RoleId,
				Username = user.Username,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}
	}
	
    public sealed record UserResponseFullDto : UserResponseBaseDto
    {
        public RoleDto? Role { get; init; }
        public IEnumerable<ProjectResponseBaseDto>? Projects { get; init; }
		public IEnumerable<TaskResponseFullDto>? Tasks { get; init; }
		
		
		public static new UserResponseFullDto From(Entities.User? user)
		{
			if (user == null) return null!;
			
			return new UserResponseFullDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				
				RoleId = user.Role != null ? null : user.RoleId,
				Role = RoleDto.From(user.Role),
				Projects = user.Projects?.Select(p => ProjectResponseFullDto.From(p)),
				Tasks = user.Tasks?.Select(t => TaskResponseFullDto.From(t))
			};
		}
	}
}
