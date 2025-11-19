using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;
using Microsoft.AspNetCore.Identity;
using Roles;
using System.Security.Claims;

namespace Services;

public sealed class UserServiceImpl(ApplicationDbContext context, UserManager<User> userManager, IUserPermissionService userPermissionService) : IUserService
{
    private readonly ApplicationDbContext _context = context;
	private readonly UserManager<User> _userManager = userManager;
	private readonly IUserPermissionService _userPermissionService = userPermissionService;
	
	public Task<UserResponseFullDto[]> GetUsersByIds(
		Guid[] ids = null!,
		bool includeProjects = false,
		bool includeTasks = false
	)
	{
		IQueryable<User> baseQuery = _context.Users.AsSplitQuery();
		
		if (ids != null)
		{
			baseQuery = baseQuery.Where(u => ids.Contains(u.Id));
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
		bool includeProjects = false,
		bool includeTasks = false
	)
	{
		return GetUsersByIds(
			ids: null!,
			includeProjects,
			includeTasks
		);
	}

	public async Task<UserResponseFullDto?> GetUserById(
		Guid id,
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
	
	public async Task<(bool, UserResponseFullDto?, int/*StatusCodes*/?, string? errAdd)> UpsertUser(
		ClaimsPrincipal callerPrincipal, UserRequestDto dto
	)
	{
		if (callerPrincipal == null || !callerPrincipal.Identity?.IsAuthenticated == true)
        {
			return (false, null, StatusCodes.Status401Unauthorized, "Caller is not authenticated.");
        }
		
		CallerContext caller = await ResolveCallerAsync(callerPrincipal);
		
		if (caller.User == null)
        {
			return (false, null, StatusCodes.Status401Unauthorized, "Caller user not found.");
        }
		
		User? target = null;
        if (dto.Id.HasValue)
        {
            target = await _userManager.FindByIdAsync(dto.Id.Value.ToString());
            if (target == null)
            {
                return (false, null, StatusCodes.Status404NotFound, $"Target user with Id '{dto.Id.Value}' not found.");
            }
        }
		
		bool bIsAllowedTo = await _userPermissionService.CanUpsertUserAsync(caller.User, dto, target);
		if (!bIsAllowedTo)
		{
			return (false, null, StatusCodes.Status403Forbidden, "Insufficient permissions for requested user upsert.");
		}
		
		bool isNewUser = false;
		
		if (target != null)
		{
			// update existing user name
			if (!string.IsNullOrEmpty(dto.Username))
			{
				target.UserName = dto.Username;
				await _userManager.SetUserNameAsync(target, dto.Username);
				await _userManager.UpdateNormalizedUserNameAsync(target);
			}
			
			// update existing user email
			if (!string.IsNullOrEmpty(dto.Email))
			{
				target.Email = dto.Email;
				await _userManager.SetEmailAsync(target, dto.Email);
				await _userManager.UpdateNormalizedEmailAsync(target);
			}
			
			// update existing user password
			if (!string.IsNullOrEmpty(dto.Password))
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(target);
				var result = await _userManager.ResetPasswordAsync(target, token, dto.Password);
				if (!result.Succeeded)
				{
					return (false, null, StatusCodes.Status500InternalServerError, GatherErrorInfos("Password update", result));
				}
			}
			
			// update existing user first name
			if (!string.IsNullOrEmpty(dto.FirstName))
			{
				target.FirstName = dto.FirstName;
				await _userManager.UpdateAsync(target);
			}
			
			// update existing user last name
			if (!string.IsNullOrEmpty(dto.LastName))
			{
				target.LastName = dto.LastName;
				await _userManager.UpdateAsync(target);
			}
		}
		else
		{
			if (string.IsNullOrEmpty(dto.Password))
			{
				return (false, null, StatusCodes.Status400BadRequest, "Password is required for new users.");
			}
			
			isNewUser = true;
			target = new User
			{
				UserName = dto.Username ?? string.Empty,
				Email = dto.Email ?? string.Empty,
				FirstName = dto.FirstName ?? string.Empty,
				LastName = dto.LastName ?? string.Empty,
			};
			var result = await _userManager.CreateAsync(target, dto.Password!);
			if (!result.Succeeded)
			{
				return (false, null, StatusCodes.Status500InternalServerError, GatherErrorInfos("User creation", result));
			}
		}
		
		// assign role if specified
		if (AppRoles.IsValidRole(dto.Role))
		{
			// get current roles
			var roles = await _userManager.GetRolesAsync(target);
			
			// remove from all previous roles
			await _userManager.RemoveFromRolesAsync(target, roles);
			
			// assign new role
			var result = await _userManager.AddToRoleAsync(target, dto.Role!);
			if (!result.Succeeded)
			{
				return (false, null, StatusCodes.Status500InternalServerError, GatherErrorInfos("Role assignment", result));
			}
		}

		return (isNewUser, UserResponseFullDto.From(target), null, null);
	}
	
	public async Task<bool> DeleteUser(Guid id)
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
	
	
	private static string GatherErrorInfos(string context, IdentityResult result)
	{
		return $"{context} failed: " + string.Join("; ", result.Errors.Select(e => e.Description));
	}
	
	private async Task<CallerContext> ResolveCallerAsync(ClaimsPrincipal principal)
	{
		// Try to resolve by UserManager first (works with cookie or bearer auth)
		User? user = await _userManager.GetUserAsync(principal);

		// Prefer roles from claims if present (saves a DB call in JWT scenarios),
		// otherwise fall back to UserManager.
		var rolesFromClaims = principal
			.Claims
			.Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
			.Select(c => c.Value)
			.Distinct(StringComparer.Ordinal)
			.ToList();

		IList<string> roles;
		if (rolesFromClaims.Count > 0)
		{
			roles = rolesFromClaims;
		}
		else if (user != null)
		{
			roles = await _userManager.GetRolesAsync(user);
		}
		else
		{
			roles = new List<string>();
		}

		return new CallerContext
		{
			User = user,
			Roles = roles
		};
	}
	
	public class CallerContext
	{
		public User? User { get; set; }
		public IList<string> Roles { get; set; } = new List<string>();
	}
}