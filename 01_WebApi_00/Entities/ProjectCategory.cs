using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{	
	[Table("project_categories")]
	public record ProjectCategory(int Id, string Name)
	{
		[Column("id")]
		public int Id { get; init; } = Id;
		[Column("name")]
		public string Name { get; init; } = Name;
	}
}