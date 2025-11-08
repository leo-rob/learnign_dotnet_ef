using Microsoft.AspNetCore.Mvc;
using Core.Dtos;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProjectsController(Services.IProjectsService projectService) : ControllerBase
{
	private readonly Services.IProjectsService _projectService = projectService;
	
	[HttpGet]
	public Task<ProjectResponseFullDto[]> GetProjects(
		[FromQuery] bool resolveCategory = false,
		[FromQuery] bool resolveManager = false)
	{
		return _projectService.GetProjects(resolveCategory, resolveManager);
	}
	
	[HttpGet("by-ids")]
	public async Task<ActionResult<ProjectResponseFullDto[]>> GetProjectsByIds(
		[FromQuery] int[] ids,
		[FromQuery] bool resolveCategory = false,
		[FromQuery] bool resolveManager = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		return await _projectService.GetProjectsByIds(ids, resolveCategory, resolveManager);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> GetProject(int id,
		[FromQuery] bool resolveCategory = false,
		[FromQuery] bool resolveManager = false)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var projectDto = await _projectService.GetProjectById(id, resolveCategory, resolveManager);
		return projectDto != null ? Ok(projectDto) : NotFound();
	}
	
	[HttpPost]
	public async Task<IActionResult> UpsertProject([FromBody] ProjectRequestDto reqProject)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var (isNewProject, project) = await _projectService.UpsertProject(reqProject);
		
		if (project == null)
		{
			return NotFound("Trying to update a project that does not exist.");
		}
		
		return isNewProject ? CreatedAtAction(nameof(GetProject), new { id = project.Id }, project) : Ok(project);
	}
	
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProject(int id)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		
		var deleted = await _projectService.DeleteProject(id);
		return deleted ? Ok() : NotFound();
	}
}
