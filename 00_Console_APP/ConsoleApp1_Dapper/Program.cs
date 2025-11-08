
using System.Data;
using Dapper;
using MySqlConnector;

namespace ConsoleApp1
{
	class Program
	{
		private const string connectionString = 
			"server=localhost;" +
			"user=root;" +
			"password=root;" +
			"database=pms";
			
		static void Main(string[] args)
		{
			DBInteractions dB = new DBInteractions(
				"localhost",
				"root",
				"root",
				"pms"
			);
			
			// create 10 projects using DBInteractions 
			for (int i = 1; i <= 10; i++)
			{
				dB.InsertData("projects", new Dictionary<string, object>
				{
					{"name", $"Project {i}"},
					{"description", $"Description for Project {i}"},
					{"start_date", DateTime.Now},
					{"end_date", DateTime.Now.AddDays(30)}
				}).Wait();
			}

			foreach (var project in GetProjects(5))
			{
				Console.WriteLine($"Project: {project.Name}, Description: {project.Description}");
			}
			
			dB.DeleteAllData("projects").Wait();
		}
		
		public static List<Project> GetProjects(int limit)
		{
			// const string Query = "SELECT * FROM Projects";
			const string Query = """
				SELECT id, name, description, start_date, end_date, manager_id, category_id
				FROM Projects
			""";

			using (IDbConnection db = new MySqlConnection(connectionString))
			{
				var projects = db.Query<Project>(Query, new { Limit = limit }).ToList();
				return projects;
			}
		}
		
		public static void InsertProject(string name, string description, DateTime startDate, DateTime endDate)
		{
			const string InsertQuery = """
				INSERT INTO Projects (name, description, start_date, end_date)
				VALUES (@Name, @Description, @StartDate, @EndDate)
			""";

			using (IDbConnection db = new MySqlConnection(connectionString))
			{
				db.Execute(InsertQuery, new
				{
					Name = name,
					Description = description,
					StartDate = startDate,
					EndDate = endDate
				});
			}
		}
	}
}
