using System.Text.Json.Serialization;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

bool isDevelopment = !builder.Environment.IsProduction();

var services = builder.Services;
{
	var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
	services.AddDbContext<ApplicationDbContext>(opt =>
	{
		opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		
		if (isDevelopment)
		{
			Development.Builder_Apply(opt);
		}
	});
	
	// Identity
	Identity.Builder_Apply(builder);
	
	services.ConfigureApplicationCookie(options =>
	{
		// Cookie settings
		// options.Cookie.HttpOnly = true;
		options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
		
		options.SlidingExpiration = true;
	});
	
	// Add services to the container.
	services.AddControllers().AddJsonOptions(options =>
	{
		options.AllowInputFormatterExceptionMessages = false;
		options.JsonSerializerOptions.AllowTrailingCommas = true;
		options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	});
	services.AddAutoMapper(typeof(Program));
	
	if (isDevelopment)
	{
		Development.Builder_Apply(builder);
	}
	
	// Register application services
	services.AddScoped<Services.IUserPermissionService, Services.UserPermissionService>();
	services.AddScoped<Services.IUserService, Services.UserServiceImpl>();
	services.AddScoped<Services.IProjectsService, Services.ProjectsServiceImpl>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
	await Development.App_Apply(app);
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();

await Identity.App_Apply(app);

app.MapControllers();

app.Run();
