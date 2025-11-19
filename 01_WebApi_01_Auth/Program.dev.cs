
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Models;

public static class Development
{
	public static void Builder_Apply(WebApplicationBuilder builder)
	{
		var services = builder.Services;
		{
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			services.AddOpenApi();
			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "oauth2"
							}
						},
						new List<string>()
					}
				});
				options.SwaggerDoc("v1", new()
				{
					Title = "My API",
					Version = "v1"
				});
			});
		}
	}
	
	public static async Task App_Apply(WebApplication app)
	{
		app.MapOpenApi();
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	internal static void Builder_Apply(DbContextOptionsBuilder opt)
	{
		// control the output information about db queries
		// opt.EnableSensitiveDataLogging();
		// opt.EnableDetailedErrors();
		opt.AddInterceptors(new DbCommandInterceptorLog());
	}
	
	private static void WriteLine(string message, DbParameterCollection parameters)
	{
		Console.WriteLine("[EF] Command:");
		Console.WriteLine('\t' + message.Replace("\n", "\n\t"));
		if (parameters.Count > 0)
		{
			Console.WriteLine("\t\tParameters:");
			foreach (DbParameter param in parameters)
			{
				Console.WriteLine($"\t\t  - {param.ParameterName} = {param.Value} (DbType={param.DbType})");
			}
		}
	}
	
	
	
	private sealed class DbCommandInterceptorLog : DbCommandInterceptor
	{
		// Synchronous methods
		
		public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.ReaderExecuting(command, eventData, result);
		}
		public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.NonQueryExecuting(command, eventData, result);
		}
		public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.ScalarExecuting(command, eventData, result);
		}
		
		// Asynchronous methods
		
		public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
		}
		public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
		}
		public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
		{
			WriteLine(command.CommandText, command.Parameters);
			return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
		}
	}
}