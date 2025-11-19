
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
	public readonly struct UserRequestDto
	{
		public Guid? Id { get; init; }
		public string? Username { get; init; }
		[EmailAddress]
		public string? Email { get; init; }
		public string? Password { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		public string? Role { get; init; }
	}
	
	public record UserResponseBaseDto
	{
		public Guid Id { get; init; }
		public string? Username { get; init; }
		public string? Email { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }

		public static UserResponseBaseDto From(Entities.User? user)
		{
			if (user == null) return null!;

			return new UserResponseBaseDto
			{
				Id = user.Id,
				Username = user.UserName,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}
	}
	
    public sealed record UserResponseFullDto : UserResponseBaseDto
    {
        public IEnumerable<ProjectResponseBaseDto>? Projects { get; init; }
		public IEnumerable<TaskResponseFullDto>? Tasks { get; init; }
		
		
		public static new UserResponseFullDto From(Entities.User? user)
		{
			if (user == null) return null!;
			
			return new UserResponseFullDto
			{
				Id = user.Id,
				Username = user.UserName,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				
				Projects = user.Projects?.Select(p => ProjectResponseFullDto.From(p)),
				Tasks = user.Tasks?.Select(t => TaskResponseFullDto.From(t))
			};
		}
	}
}
