using Core.Entites;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
	class Program
	{
		private const string kConnectionString = "server=localhost;user=root;password=root;database=pms2";
		
		public static void Main(string[] args)
		{
			using (var instance = new PMSContext(kConnectionString))
			{
				instance.Database.EnsureCreated();
				
			//	foreach (var project in instance.Projects)
			//	{
			//		Console.WriteLine($"Project: {project.Id} {project.Name} ({project.StartDate} - {project.EndDate})");
			//	}
			//	foreach (var user in instance.Users)
			//	{
			//		Console.WriteLine($"User: {user.Id} {user.Username} ({user.Email})");
			//	}
			
				{
			//		var role = instance.Roles.First();
			//		var newuser = new User()
			//		{
			//			Username = "newuser",
			//			Password = "newpassword",
			//			Email = "newuser@example.com",
			//			Role = role
			//		};
			//		// new User(Username: "newuser", Password: "newpassword", Email: "newuser@example.com",
			//		//  null, null, role.Id);
			//		instance.Users.Add(newuser);
			//		instance.SaveChanges();
				}
				
				{
			//		var users = instance.Users
			//		// Lazy join for projects and roles
			//			.Include(u => u.Projects)
			//			.Include(u => u.Role)
			//		;
			//		Console.WriteLine(users.ToQueryString());
			//		/*
			//		 *	SELECT `u`.`id`, `u`.`email`, `u`.`first_name`, `u`.`last_name`, `u`.`password`, `u`.`username`, `u`.`role_id`, `r`.`id`, `p`.`id`, `p`.`category_id`, `p`.`end_date`, `p`.`manager_id`, `p`.`name`, `p`.`start_date`, `r`.`name`
			//		 *	FROM `users` AS `u`
			//		 *	INNER JOIN `roles` AS `r` ON `u`.`role_id` = `r`.`id`
			//		 *	LEFT JOIN `projects` AS `p` ON `u`.`id` = `p`.`manager_id`
			//		 *	ORDER BY `u`.`id`, `r`.`id`
			//		*/
			//		foreach (var user in users)
			//		{
			//			Console.WriteLine($"User: {user.Id} {user.Username} ({user.Email})");
			//			
			//			if (user.Projects != null)
			//			{
			//				foreach (var project in user.Projects)
			//				{
			//					Console.WriteLine($"\tProject: {project.Id} {project.Name} ({project.StartDate} - {project.EndDate})");
			//				}
			//			}
			//			if (user.Role != null)
			//			{
			//				Console.WriteLine($"\tRole: {user.Role.RoleId} {user.Role.RoleName}");
			//			}
			//		}
				}
				
				{
					
				}
			}
		}
	}
}
