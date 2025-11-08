using Core.Dtos;

namespace Services;

public interface IUserService
{
    public Task<UserResponseFullDto[]> GetUsersByIds(
		int[] ids,
		bool resolveRole = false,
		bool includeProjects = false,
		bool includeTasks = false
	);
    
	public Task<UserResponseFullDto[]> GetUsers(
		bool resolveRole = false,
		bool includeProjects = false,
		bool includeTasks = false
	);
	
	public Task<UserResponseFullDto?> GetUserById(
		int id,
		bool resolveRole = false,
		bool includeProjects = false,
		bool includeTasks = false
	);

	public Task<(bool, UserResponseFullDto?)> UpsertUser(
		UserRequestDto userDto);
	
	public Task<bool> DeleteUser(int id);
}