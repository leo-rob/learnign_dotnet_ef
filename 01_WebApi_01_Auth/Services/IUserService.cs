using Core.Dtos;

namespace Services;

public interface IUserService
{
    public Task<UserResponseFullDto[]> GetUsersByIds(
		Guid[] ids,
		bool includeProjects = false,
		bool includeTasks = false
	);
    
	public Task<UserResponseFullDto[]> GetUsers(
		bool includeProjects = false,
		bool includeTasks = false
	);
	
	public Task<UserResponseFullDto?> GetUserById(
		Guid id,
		bool includeProjects = false,
		bool includeTasks = false
	);

	public Task<(bool, UserResponseFullDto?, int/*StatusCodes*/?, string? errAdd)> UpsertUser(
		System.Security.Claims.ClaimsPrincipal caller, UserRequestDto userDto);
	
	public Task<bool> DeleteUser(Guid id);
}