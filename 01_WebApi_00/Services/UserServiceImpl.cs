using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Services;

public sealed class UserServiceImpl(PMSContext context) : IUserService
{
    private readonly PMSContext _context = context;
	
	public Task<UserResponseFullDto[]> GetUsersByIds(
		int[] ids = null!,
		bool resolveRole = false,
		bool includeProjects = false,
		bool includeTasks = false
	)
	{
		IQueryable<User> baseQuery = _context.Users.AsSplitQuery();
		
		if (ids != null)
		{
			baseQuery = baseQuery.Where(u => ids.Contains(u.Id));
		}

		if (resolveRole)
		{
			baseQuery = baseQuery.Include(static u => u.Role);
		}

		if (includeProjects)
		{
			baseQuery = baseQuery.Include(static u => u.Projects!).ThenInclude(static p => p.Category);
		}

		if (includeTasks)
		{
			baseQuery = baseQuery.Include(static u => u.Tasks);
		}

		var dtoQuery = baseQuery.Select(u => UserResponseFullDto.From(u));

		return dtoQuery.ToArrayAsync();
	}
	
	public Task<UserResponseFullDto[]> GetUsers(
		bool resolveRole = false,
		bool includeProjects = false,
		bool includeTasks = false
	)
	{
		return GetUsersByIds(
			ids: null!,
			resolveRole,
			includeProjects,
			includeTasks
		);
	}

	public async Task<UserResponseFullDto?> GetUserById(
		int id,
		bool resolveRole,
		bool includeProjects,
		bool includeTasks
	)
	{
		var dbUser = await _context.Users.FindAsync(id);
		if (dbUser == null)
		{
			return null;
		}
		
		var entry = _context.Entry(dbUser);
		{
			if (resolveRole)
			{
				await entry.Reference(static u => u.Role).LoadAsync();
			}
			
			if (includeProjects)
			{
				await entry.Collection(static u => u.Projects!).LoadAsync();
				
				foreach (var project in dbUser.Projects!)
				{
					await _context.Entry(project).Reference(static p => p.Category).LoadAsync();
				}
			}
			if (includeTasks)
			{
				await entry.Collection(static u => u.Tasks!).LoadAsync();
			}
		}
		
		return UserResponseFullDto.From(dbUser);
	}
	
	public async Task<(bool, UserResponseFullDto?)> UpsertUser(
		UserRequestDto reqUser
	)
	{
		bool isNewUser = false;
		User? user = null;
		
		if (reqUser.Id.HasValue)
		{
			user = await _context.Users.FindAsync(reqUser.Id.Value);
			
			if (user == null)
			{
				return (false, null);
			}
			
			// update using valid properties of reqUser
			user.Username = reqUser.Username ?? user.Username;
			user.Password = reqUser.Password ?? user.Password;
			user.Email = reqUser.Email ?? user.Email;
			user.FirstName = reqUser.FirstName ?? user.FirstName;
			user.LastName = reqUser.LastName ?? user.LastName;
			user.RoleId = reqUser.RoleId ?? user.RoleId;
			_context.Users.Update(user);
		}
		else
		{
			isNewUser = true;
			user = new User
			{
				Username = reqUser.Username ?? string.Empty,
				Password = reqUser.Password ?? string.Empty,
				Email = reqUser.Email ?? string.Empty,
				FirstName = reqUser.FirstName ?? string.Empty,
				LastName = reqUser.LastName ?? string.Empty,
				RoleId = reqUser.RoleId ?? -1,
			};
			_context.Users.Add(user);
		}
		
		await _context.SaveChangesAsync();
		
		return (isNewUser, UserResponseFullDto.From(user));
	}
	
	public async Task<bool> DeleteUser(int id)
	{
		var user = await _context.Users.FindAsync(id);
		if (user == null)
		{
			return false;
		}
		
		_context.Users.Remove(user);
		await _context.SaveChangesAsync();
		
		return true;
	}
	
	
}