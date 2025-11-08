using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{
	public class PMSContext : DbContext
	{
		private readonly string kConnectionString = string.Empty;
		
		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectCategory> ProjectCategories { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<TaskAttachment> TaskAttachments { get; set; }
		public DbSet<User> Users { get; set; }
		
		
		////////////////////////////////////////////////
		public PMSContext() {}
		
		////////////////////////////////////////////////
		public PMSContext(string InConnString)
		{
			kConnectionString = InConnString;
		}
		
		////////////////////////////////////////////////
		public PMSContext(DbContextOptions<PMSContext> options)
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
	}
}