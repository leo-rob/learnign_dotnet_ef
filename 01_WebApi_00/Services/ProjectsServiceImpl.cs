using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Services;

public class ProjectsServiceImpl(PMSContext context) : IProjectsService
{
    private readonly PMSContext _context = context;
	
	public Task<ProjectResponseDto[]> GetProjectsByIds(
		int[] ids,
		bool includeCategory = false
	)
	{
		// Using AsSplitQuery to optimize performance when including related data
		IQueryable<Project> baseQuery = _context.Projects.AsSplitQuery();
		
		if (ids != null)
		{
			baseQuery = baseQuery.Where(p => ids.Contains(p.Id));
		}
		
		if (includeCategory)
		{
			baseQuery = baseQuery.Include(static p => p.Category);
		}
		
		var dtoQuery = baseQuery.Select(static p => ProjectResponseDto.From(p));

		return dtoQuery.ToArrayAsync();
	}
	
	public Task<ProjectResponseDto[]> GetProjects(
		bool includeCategory = false
	)
	{
		return GetProjectsByIds(
			ids: null!,
			includeCategory
		);
	}
	
	public async Task<ProjectResponseDto?> GetProjectById(
		int id,
		bool includeCategory
	)
	{
		var dbProject = await _context.Projects.FindAsync(id);
		if (dbProject == null)
		{
			return null;
		}
		
		var entry = _context.Entry(dbProject);
		{
			if (includeCategory)
			{
				await entry.Reference(static p => p.Category).LoadAsync();
			}
		}
		
		return ProjectResponseDto.From(dbProject);
	}
	
	public async Task<(bool, ProjectResponseDto?)> UpsertProject(
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
				return (false, null);
			}
			
			// Update existing project
			project.Name = reqProject.Name ?? project.Name;
			project.CategoryId = reqProject.CategoryId ?? project.CategoryId;
			project.ManagerId = reqProject.ManagerId ?? project.ManagerId;
			project.StartDate = reqProject.StartDate ?? project.StartDate;
			project.EndDate = reqProject.EndDate ?? project.EndDate;

			_context.Projects.Update(project);
		}
		else
		{
			isNewProject = true;
			project = new Project
			{
				Name = reqProject.Name ?? string.Empty,
				CategoryId = reqProject.CategoryId ?? -1,
				ManagerId = reqProject.ManagerId ?? -1,
				StartDate = reqProject.StartDate,
				EndDate = reqProject.EndDate
			};

			await _context.Projects.AddAsync(project);
		}

		await _context.SaveChangesAsync();
		return (isNewProject, ProjectResponseDto.From(project));
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