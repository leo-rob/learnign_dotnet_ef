using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{
	public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
	{
		private readonly string kConnectionString = string.Empty;
		
		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectCategory> ProjectCategories { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<TaskAttachment> TaskAttachments { get; set; }
		
		
		////////////////////////////////////////////////
		public ApplicationDbContext() {}
		
		////////////////////////////////////////////////
		public ApplicationDbContext(string InConnString)
		{
			kConnectionString = InConnString;
		}
		
		////////////////////////////////////////////////
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }
		
		////////////////////////////////////////////////
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			
			if (kConnectionString != string.Empty)
			{
				optionsBuilder.UseMySql(kConnectionString, ServerVersion.AutoDetect(kConnectionString));
			}
		}
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			
			builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
			builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
		}
	}
}