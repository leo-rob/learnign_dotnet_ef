
using Microsoft.EntityFrameworkCore;

namespace Core.Entites
{	
	public class PMSContext(string InConnString) : DbContext
	{
		private readonly string kConnectionString = InConnString;
		
		public DbSet<Role> Roles { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			
			optionsBuilder.UseMySql(kConnectionString, ServerVersion.AutoDetect(kConnectionString));
		}
	}
}