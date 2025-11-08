using Core.Dtos;

namespace Services;

public interface IUserService
{
    public Task<UserResponseDto[]> GetUsersByIds(
		int[] ids,
		bool includeRoles = false,
		bool includeProjects = false,
		bool includeTasks = false);
    
	public Task<UserResponseDto[]> GetUsers(
		bool includeRoles = false,
		bool includeProjects = false,
		bool includeTasks = false);	
		
	public Task<UserResponseDto?> GetUserById(
		int id,
		bool includeRoles = false,
		bool includeProjects = false,
		bool includeTasks = false);

	public Task<(bool, UserResponseDto?)> UpsertUser(
		UserRequestDto userDto);
	
	public Task<bool> DeleteUser(int id);
}