using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class Identity
{
	public static void Builder_Apply(WebApplicationBuilder builder)
	{
		var services = builder.Services;
		{
			services.AddAuthorization();
			services.AddIdentityApiEndpoints<User>(options =>
			{
				// Password settings.
				options.Password.RequiredLength = 8;
				
				// User settings.
				options.User.RequireUniqueEmail = true;
				
				// Lockout settings.
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				
				// SignIn settings.
				options.SignIn.RequireConfirmedEmail = true;
				
				// Token settings.
				options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
			})
				// Register role support so the underlying EF stores implement IUserRoleStore<TUser>
				.AddRoles<Role>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager()
				.AddRoleManager<RoleManager<Role>>()
				.AddApiEndpoints()
				.AddDefaultTokenProviders()
			;
		}
	}
	
	public static async System.Threading.Tasks.Task App_Apply(WebApplication app)
	{
		using (var scope = app.Services.CreateScope())
		{
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			
			// Add Roles
			{
				var roles = await roleManager.Roles.ToArrayAsync();
			//	foreach (var item in roles)
			//	{
			//		Console.WriteLine($"[Role] {item.Name}");
			//	}
				
				foreach (string roleName in Roles.AppRoles.Roles.Keys)
				{
					if (!roles.Any(r => r.Name == roleName))
					{
						// Create using the custom Role entity
						var newRole = new Role {
							Name = roleName,
							NormalizedName = roleName.ToUpper()
						};
						await roleManager.CreateAsync(newRole);
					}
				}
			}
			
			// Ensure Admin User
			{
				const string admMail = "adm_min@mail.adm", admPass = "Admin@123";
				var u = await userManager.FindByEmailAsync(admMail);
				if (u == null)
				{
					u = new User
					{
						UserName = "adm",
						Email = admMail,
						FirstName = string.Empty,
						LastName = string.Empty
					};
					var res = await userManager.CreateAsync(u, admPass);
					if (res.Succeeded)
					{
						await userManager.AddToRoleAsync(u, Roles.AppRoles.Admin);
					}
					else
					{
						// Handle creation failure (e.g., log the errors)
						var errors = string.Join(", ", res.Errors.Select(e => e.Description));
						throw new Exception($"Failed to create admin user: {errors}");
					}
				}
			}
		}
		
		app.UseAuthentication();
		app.UseAuthorization();

		// Identity minimal API endpoints
		app.MapIdentityApi<User>();
	}
}