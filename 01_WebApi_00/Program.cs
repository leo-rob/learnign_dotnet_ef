using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

bool isDevelopment = !builder.Environment.IsProduction();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

var services = builder.Services;
{
    // Add services to the container.
    services.AddControllers().AddJsonOptions(options =>
    {
        options.AllowInputFormatterExceptionMessages = false;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    services.AddAutoMapper(typeof(Program));
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    services.AddOpenApi();
    services.AddSwaggerGen();

    services.AddDbContext<PMSContext>(opt =>
    {
        opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    //    if(isDevelopment)
    //    {
    //        opt.LogTo(
    //            msg => Console.WriteLine(msg),
    //            new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuted }
    //        );
    //        opt.EnableSensitiveDataLogging();
    //    }
    });

    // Register application services
    services.AddScoped<Services.IUserService, Services.UserServiceImpl>();
    services.AddScoped<Services.IProjectsService, Services.ProjectsServiceImpl>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
