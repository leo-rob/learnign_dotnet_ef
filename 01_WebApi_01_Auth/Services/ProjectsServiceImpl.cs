using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Services;

public class ProjectsServiceImpl(ApplicationDbContext context) : IProjectsService
{
    private readonly ApplicationDbContext _context = context;
	
	public Task<ProjectResponseFullDto[]> GetProjectsByIds(
		int[]? ids,
		bool resolveCategory = false,
		bool resolveManager = false
	)
	{
		// Using AsSplitQuery to optimize performance when including related data
		IQueryable<Project> baseQuery = _context.Projects.AsSplitQuery();
		
		if (ids != null)
		{
			baseQuery = baseQuery.Where(p => ids.Contains(p.Id));
		}
		
		if (resolveCategory)
		{
			baseQuery = baseQuery.Include(static p => p.Category);
		}
		
		if (resolveManager)
		{
			baseQuery = baseQuery.Include(static p => p.User);
		}
		
		return baseQuery.Select(p => ProjectResponseFullDto.From(p)).ToArrayAsync();
	}
	
	public Task<ProjectResponseFullDto[]> GetProjects(
		bool resolveCategory = false,
		bool resolveManager = false
	)
	{
		return GetProjectsByIds(
			ids: null,
			resolveCategory,
			resolveManager
		);
	}
	
	public async Task<ProjectResponseFullDto?> GetProjectById(
		int id,
		bool resolveCategory = false,
		bool resolveManager = false
	)
	{
		var dbProject = await _context.Projects.FindAsync(id);
		if (dbProject == null)
		{
			return null;
		}
		
		var entry = _context.Entry(dbProject);
		{
			if (resolveCategory)
			{
				await entry.Reference(static p => p.Category).LoadAsync();
			}
			if (resolveManager)
			{
				await entry.Reference(static p => p.User).LoadAsync();
			}
		}
		
		return ProjectResponseFullDto.From(dbProject);
	}
	
	public async Task<(bool, ProjectResponseFullDto?)> UpsertProject(
		ProjectRequestDto reqProject
	)
	{
		Project? project;
		bool isNewProject = false;
		
		if (reqProject.Id.HasValue)
		{
			project = await _context.Projects.FindAsync(reqProject.Id.Value);
			
			if (project == null)
			{
				// Trying to update a project that does not exist
				return (false, null);
			}
			
			// Update existing project
			_context.Projects.Update(Project.FromUpsert(reqProject, project));
		}
		else
		{
			isNewProject = true;
			await _context.Projects.AddAsync(project = Project.FromUpsert(reqProject));
		}
		
		await _context.SaveChangesAsync();
		return (isNewProject, ProjectResponseFullDto.From(project));
	}
	
	public async Task<bool> DeleteProject(int id)
	{
		var project = await _context.Projects.FindAsync(id);
		if (project == null)
		{
			return false;
		}
		
		_context.Projects.Remove(project);
		await _context.SaveChangesAsync();
		
		return true;
	}
}