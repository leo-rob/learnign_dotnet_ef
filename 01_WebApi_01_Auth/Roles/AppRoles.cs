namespace Roles;

public static class AppRoles
{
	public const string SuperAdmin = "SuperAdmin";
	public const string Admin = "Admin";
	public const string User = "User";
	public const string Manager = "Manager";
	
	private static readonly Dictionary<string, int> _roles = new()
	{
		{ SuperAdmin, 0 },
		{ Admin, 1 },
		{ User, 2 },
		{ Manager, 3 }
	};
	
	public static IReadOnlyDictionary<string, int> Roles => _roles;
	
	public static bool IsValidRole(string? role)
	{
		return !string.IsNullOrEmpty(role) && _roles.ContainsKey(role);
	}
}