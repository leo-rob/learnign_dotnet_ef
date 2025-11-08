using Core.Dtos;

namespace Services;

public interface IProjectsService
{
	Task<ProjectResponseDto[]> GetProjectsByIds(int[] ids, bool includeCategory = false);
	Task<ProjectResponseDto[]> GetProjects(bool includeCategory = false);
	Task<ProjectResponseDto?> GetProjectById(int id, bool includeCategory = false);
	Task<(bool, ProjectResponseDto?)> UpsertProject(ProjectRequestDto reqProject);
	Task<bool> DeleteProject(int id);
}