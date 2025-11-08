using Core.Dtos;

namespace Services;

public interface IProjectsService
{
	public Task<ProjectResponseFullDto[]> GetProjectsByIds(
		int[] ids,
		bool resolveCategory = false,
		bool resolveManager = false);
	
	public Task<ProjectResponseFullDto[]> GetProjects(
		bool resolveCategory = false,
		bool resolveManager = false);
	
	public Task<ProjectResponseFullDto?> GetProjectById(
		int id,
		bool resolveCategory = false,
		bool resolveManager = false);
		
	public Task<(bool, ProjectResponseFullDto?)> UpsertProject(
		ProjectRequestDto reqProject
	);
	
	public Task<bool> DeleteProject(int id);
}