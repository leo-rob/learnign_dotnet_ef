
using Core.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Roles;

namespace Services;

public interface IUserPermissionService
{
	Task<bool> CanUpsertUserAsync(User caller, UserRequestDto dto, User? target);
//	Task EnsureUpsertAllowedAsync(User caller, UserRequestDto dto, User? target);
}

public class UserPermissionService(UserManager<User> userManager) : IUserPermissionService
{
	private readonly UserManager<User> _userManager = userManager;

	public async Task<bool> CanUpsertUserAsync(User caller, UserRequestDto dto, User? target)
	{
		string? callerRole = (await _userManager.GetRolesAsync(caller)).FirstOrDefault();
		
		if (string.IsNullOrEmpty(callerRole))
		{
			return false;
		}
		
		int callerRank = GetBestRank(callerRole);

		if (callerRole == AppRoles.SuperAdmin)
		{
			return true;
		}
		
		if (string.IsNullOrEmpty(dto.Role))
		{
			return false;
		}
		if (!AppRoles.Roles.ContainsKey(dto.Role))
		{
			return false;
		}

		int requestedBestRank = GetBestRank(dto.Role);

		// Caller cannot assign same-or-higher power: requestedBestRank <= callerRank
		if (requestedBestRank <= callerRank)
		{
			return false;
		}

		if (target != null)
		{
			int targetBestRank = GetBestRank((await _userManager.GetRolesAsync(target)).FirstOrDefault());

			// No self-elevation
			if (target.Id == caller.Id)
			{
				// If requested roles give more power than current (smaller rank), deny
				if (requestedBestRank < targetBestRank)
				{
					return false;
				}
			}

			// Caller cannot touch targets with same-or-higher power: targetBestRank <= callerRank
			if (targetBestRank <= callerRank)
			{
				return false;
			}
		}

		return true;
	}

//	public async Task EnsureUpsertAllowedAsync(User caller, UserRequestDto dto, User? target)
//	{
//		bool ok = await CanUpsertUserAsync(caller, dto, target);
//		if (!ok)
//		{
//			throw new UnauthorizedAccessException("Insufficient permissions for requested user upsert.");
//		}
//	}

	private static int GetBestRank(string? role)
	{
		if (string.IsNullOrEmpty(role))
		{
			return int.MaxValue;
		}
		
		if (AppRoles.Roles.TryGetValue(role, out int r))
		{
			return r;
		}
		return int.MaxValue;
	}
}