using MySqlConnector;

public class DBInteractions(string InServer, string InUser, string InPassword, string InDatabase)
{
	private readonly string kServer = InServer, kUser = InUser, kPassword = InPassword, kDatabase = InDatabase;

	public async Task<long> InsertData(string tableName, Dictionary<string, object> data)
	{
		long lastInsertedId = -1L;
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string insertQuery = $"INSERT INTO {tableName} (";
			insertQuery += string.Join(", ", data.Keys);
			insertQuery += ") VALUES (";
			insertQuery += string.Join(", ", data.Keys.Select(k => "@" + k));
			insertQuery += ");";

			using (var command = new MySqlCommand(insertQuery, connection))
			{
				foreach (var kvp in data)
				{
					command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
				}
				
				await connection.OpenAsync();
				await command.ExecuteNonQueryAsync();
				lastInsertedId = command.LastInsertedId;
			}
			
			/*
			await connection.OpenAsync();
			
			var command = connection.CreateCommand();
			command.CommandText = 
				"INSERT INTO my_table (column1, column2) " +
				"VALUES (@value1, @value2);";
			
			command.Parameters.AddWithValue("@value1", data["column1"]);
			command.Parameters.AddWithValue("@value2", data["column2"]);
			
			await command.ExecuteNonQueryAsync();
			lastInsertedId = command.LastInsertedId;
			*/
		}
		return lastInsertedId;
	}
	
	public async Task<int> UpdateData(string tableName, Dictionary<string, object> data, string whereClause, Dictionary<string, object> whereParams)
	{
		int rowsAffected = 0;
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string updateQuery = $"UPDATE {tableName} SET ";
			updateQuery += string.Join(", ", data.Keys.Select(k => $"{k} = @{k}"));
			updateQuery += " WHERE " + whereClause + ";";

			using (var command = new MySqlCommand(updateQuery, connection))
			{
				foreach (var kvp in data)
				{
					command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
				}
				foreach (var kvp in whereParams)
				{
					command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
				}
				
				await connection.OpenAsync();
				rowsAffected = await command.ExecuteNonQueryAsync();
			}
		}
		return rowsAffected;
	}
	
	public async Task<int> DeleteData(string tableName, string whereClause, Dictionary<string, object> whereParams)
	{
		int rowsAffected = 0;
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string deleteQuery = $"DELETE FROM {tableName} WHERE " + whereClause + ";";

			using (var command = new MySqlCommand(deleteQuery, connection))
			{
				foreach (var kvp in whereParams)
				{
					command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
				}
				
				await connection.OpenAsync();
				rowsAffected = await command.ExecuteNonQueryAsync();
			}
		}
		return rowsAffected;
	}
	
	public async Task<List<Dictionary<string, object>>> QueryAllRows(string tableName)
	{
		var results = new List<Dictionary<string, object>>();
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string selectQuery = $"SELECT * FROM {tableName};";

			using (var command = new MySqlCommand(selectQuery, connection))
			{
				await connection.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						var row = new Dictionary<string, object>();
						for (int i = 0; i < reader.FieldCount; i++)
						{
							row[reader.GetName(i)] = reader.GetValue(i);
						}
						results.Add(row);
					}
				}
			}
		}
		return results;
	}
	
	public async Task<List<Dictionary<string, object>>> QueryData(string tableName, List<string> columns, string? whereClause = null, Dictionary<string, object>? whereParams = null)
	{
		var results = new List<Dictionary<string, object>>();
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string selectQuery = $"SELECT {string.Join(", ", columns)} FROM {tableName}";
			if (!string.IsNullOrEmpty(whereClause))
			{
				selectQuery += " WHERE " + whereClause;
			}
			selectQuery += ";";

			using (var command = new MySqlCommand(selectQuery, connection))
			{
				if (whereParams != null)
				{
					foreach (var kvp in whereParams)
					{
						command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
					}
				}
				
				await connection.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						var row = new Dictionary<string, object>();
						foreach (var col in columns)
						{
							row[col] = reader[col];
						}
						results.Add(row);
					}
				}
			}
		}
		return results;
	}
	
	public async Task<int> DeleteAllData(string tableName)
	{
		int rowsAffected = 0;
		
		var connString = $"server={kServer};user={kUser};password={kPassword};database={kDatabase};";
		using (var connection = new MySqlConnection(connString))
		{
			string deleteQuery = $"DELETE FROM {tableName};";

			using (var command = new MySqlCommand(deleteQuery, connection))
			{
				await connection.OpenAsync();
				rowsAffected = await command.ExecuteNonQueryAsync();
			}
		}
		return rowsAffected;
	}
}