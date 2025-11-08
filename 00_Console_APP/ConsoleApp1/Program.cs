using MySqlConnector;

namespace ConsoleApp1
{
	class Program
	{
		private static readonly string connectionString = 
			"server=localhost;"
		+	"user=root;"
		+	"password=root;"
		+	"port=3306;"
		+	"database=pms;"
		;	
			
		private static async Task Main(string[] args)
		{
			DBInteractions db = new DBInteractions(
				"localhost",
				"root",
				"root",
				"pms"
			);
			
			long newProjectId = await db.InsertData("projects", new Dictionary<string, object>(4)
			{
				{"name", "Project X"},
				{"description", "Description X"},
				{"start_date", DateTime.Now},
				{"end_date", DateTime.Now.AddDays(15)}
			}),
			tmpId = await db.InsertData("projects", new Dictionary<string, object>(4)
			{
				{"name", "Project Y"},
				{"description", "Description Y"},
				{"start_date", DateTime.Now},
				{"end_date", DateTime.Now.AddDays(26)}
			})
			;
			
			var allProjects = await db.QueryAllRows("projects");
			foreach (var project in allProjects)
			{
				Console.WriteLine($"Project ID: {project["id"]}, Name: {project["name"]}, Description: {project["description"]}, Start Date: {project["start_date"]}, End Date: {project["end_date"]}");
			}
			
			await db.DeleteData("projects", "id = @id", new Dictionary<string, object>(1)
			{
				{"id", tmpId}
			});
			
			allProjects = await db.QueryAllRows("projects");
			foreach (var project in allProjects)
			{
				Console.WriteLine($"Project ID: {project["id"]}, Name: {project["name"]}, Description: {project["description"]}, Start Date: {project["start_date"]}, End Date: {project["end_date"]}");
			}
			
			await db.UpdateData("projects", new Dictionary<string, object>(4)
			{
				{"name", "Project Z"},
				{"description", "Description Z"},
				{"start_date", DateTime.Now},
				{"end_date", DateTime.Now.AddDays(20)}
			}, "id = @id", new Dictionary<string, object>()
			{
				{"id", newProjectId}
			});
			
			allProjects = await db.QueryAllRows("projects");
			foreach (var project in allProjects)
			{
				Console.WriteLine($"Project ID: {project["id"]}, Name: {project["name"]}, Description: {project["description"]}, Start Date: {project["start_date"]}, End Date: {project["end_date"]}");
			}
			
			await db.DeleteData("projects", "id = @id", new Dictionary<string, object>()
			{
				{"id", newProjectId}
			});
		//	using (var connection = new MySqlConnection(connectionString))
		//	{
		//		{
		//			await connection.OpenAsync();
		//			Console.WriteLine("Connection to database established successfully.");
		//		}
		//		
		//		long newProjectId;
		//		
		//		{
		//			newProjectId = await InsertProject(connection, 
		//				"Project 000", 
		//				"Description 000", 
		//				DateTime.Now,
		//				DateTime.Now.AddDays(30)
		//			);
		//			
		//			long tmpId = await InsertProject(connection, 
		//				"Project 002", 
		//				"Description 002", 
		//				DateTime.Now,
		//				DateTime.Now.AddDays(60)
		//			);
		//			await ListProjects(connection);
		//			await DeleteProject(connection, tmpId);
		//		}
		//		
		//		await ListProjects(connection);
		
		//		{
		//			await UpdateProject(connection, 
		//				newProjectId,
		//				"Project 001", 
		//				"Description 001", 
		//				DateTime.Now,
		//				DateTime.Now.AddDays(25)
		//			);
		//		}
		//		
		//		await ListProjects(connection);
		//		
		//		await DeleteAllProjects(connection);
		//	}
		}
		
	//	private static async Task ListProjects(MySqlConnection connection)
	//	{
	//		const string query = "select * from projects;";
	//		
	//		using (var command = new MySqlCommand(query, connection))
	//		{
	//			using (var reader = await command.ExecuteReaderAsync()) // Execute the query
	//			{
	//				while (await reader.ReadAsync()) // Read each row
	//				{
	//					// reader id is a string, so we convert it to string
	//					
	//					var id = Convert.ToString(reader["id"]);
	//					var name = Convert.ToString(reader["name"]);
	//					var description = Convert.ToString(reader["description"]);
	//					var startDate = Convert.ToDateTime(reader["start_date"]);
	//					var endDate = Convert.ToDateTime(reader["end_date"]);
	//					
	//					Console.WriteLine($"Project ID: {id}, Name: {name}, Description: {description}, Start Date: {startDate}, End Date: {endDate}");
	//				}
	//			}
	//		}
	//	}
	//	
	//	private static async Task DeleteAllProjects(MySqlConnection connection)
	//	{
	//		const string query = "delete from projects;";
	//		
	//		using (var command = new MySqlCommand(query, connection))
	//		{
	//			var rowsAffected = await command.ExecuteNonQueryAsync();
	//			Console.WriteLine($"{rowsAffected} row(s) deleted.");
	//		}
	//	}
	//	
	//	
	//	private const string insertProjectQuery = 
	//		"insert into projects (name, description, start_date, end_date)"
	//	+	"values (@name, @description, @start_date, @end_date)"
	//	;
	//
	//	private static async Task<long> InsertProject(MySqlConnection connection, string name, string desc, DateTime startDate, DateTime endDate)
	//	{
	//		long newProjectId = -1L;
	//		using (var command = new MySqlCommand(insertProjectQuery, connection))
	//		{
	//			var p = command.Parameters;
	//			p.AddWithValue("@name", name);
	//			p.AddWithValue("@description", desc);
	//			p.AddWithValue("@start_date", startDate);
	//			p.AddWithValue("@end_date", endDate);
	//			
	//			var rowsAffected = await command.ExecuteNonQueryAsync();
	//			var lastInsertedId = command.LastInsertedId;
	//			Console.WriteLine($"{rowsAffected} row(s) inserted. Last inserted ID: {lastInsertedId}");
	//			newProjectId = (int)lastInsertedId;
	//		}
	//		return newProjectId;
	//	}
	//
	//	private static readonly string updateProjectQuery = string.Join(", ", new string[] {
	//		"name = @name",
	//		"description = @description",
	//		"start_date = @start_date",
	//		"end_date = @end_date",
	//	});
	//
	//	private static async Task UpdateProject(MySqlConnection connection, long projectId, string? newName, string? newDescription, DateTime? newStartDate, DateTime? newEndDate)
	//	{
	//		using (var command = new MySqlCommand("update projects set " + updateProjectQuery + " where id = @id", connection))
	//		{
	//			command.Parameters.AddWithValue("@name", newName);
	//			command.Parameters.AddWithValue("@description", newDescription);
	//			command.Parameters.AddWithValue("@start_date", newStartDate);
	//			command.Parameters.AddWithValue("@end_date", newEndDate);
	//			command.Parameters.AddWithValue("@id", projectId);
	//			
	//			var rowsAffected = await command.ExecuteNonQueryAsync();
	//			Console.WriteLine($"{rowsAffected} row(s) updated.");
	//		}
	//	}
	//	
	//	private static async Task DeleteProject(MySqlConnection connection, long projectId)
	//	{
	//		const string deleteProjectQuery = "delete from projects where id = @id";
	//		
	//		using (var command = new MySqlCommand(deleteProjectQuery, connection))
	//		{
	//			command.Parameters.AddWithValue("@id", projectId);
	//			
	//			var rowsAffected = await command.ExecuteNonQueryAsync();
	//			Console.WriteLine($"{rowsAffected} row(s) deleted.");
	//		}
	//	}
	}
}
