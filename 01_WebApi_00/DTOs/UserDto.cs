
using System.ComponentModel.DataAnnotations;

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
	
    public record UserResponseDto
    {
        public int Id { get; init; }
        public string? Username { get; init; }
        public string? Password { get; init; }
        public string? Email { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public RoleDto? Role { get; init; }
        public IEnumerable<ProjectResponseDto>? Projects { get; init; }
		public IEnumerable<TaskDto>? Tasks { get; init; }
		
		
		public static UserResponseDto From(Entities.User? user)
		{
			if (user == null) return null!;
			
			return new UserResponseDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = RoleDto.From(user.Role!),
				Projects = user.Projects?.Select(ProjectResponseDto.From),
				Tasks = user.Tasks?.Select(TaskDto.From)
			};
		}
	}
}
