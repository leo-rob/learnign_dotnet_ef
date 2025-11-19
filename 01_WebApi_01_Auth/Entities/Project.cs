
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{	
	[Table("projects")]
	public record Project
	{
		[Column("id"), Required]
		public int Id { get; set; }
		[Column("name"), Required]
		public required string Name { get; set; }
		
		[Column("start_date")]
		public DateTime? StartDate { get; set; }
		[Column("end_date")]
		public DateTime? EndDate { get; set; }
		
		[Column("category_id")] public required int CategoryId { get; set; }
		
		[ForeignKey(nameof(CategoryId))] public ProjectCategory? Category { get; set; }
		
		[Column("manager_id")] public required Guid ManagerId { get; set; }
		
		[ForeignKey(nameof(ManagerId))] public User? User { get; set; }
		
		
		
		
		public static Project FromUpsert(Dtos.ProjectRequestDto upsrtProject, Project? existingProject = null)
		{
			if (existingProject != null)
			{
				existingProject.Name = upsrtProject.Name ?? existingProject.Name;
				existingProject.CategoryId = upsrtProject.CategoryId ?? existingProject.CategoryId;
				existingProject.ManagerId = upsrtProject.ManagerId ?? existingProject.ManagerId;
				existingProject.StartDate = upsrtProject.StartDate ?? existingProject.StartDate;
				existingProject.EndDate = upsrtProject.EndDate ?? existingProject.EndDate;
				return existingProject;
			}
			else
			{
				return new Project
				{
					Name = upsrtProject.Name ?? string.Empty,
					CategoryId = upsrtProject.CategoryId ?? -1,
					ManagerId = upsrtProject.ManagerId ?? Guid.Empty,
					StartDate = upsrtProject.StartDate,
					EndDate = upsrtProject.EndDate
				};
			}
		}
	}
}