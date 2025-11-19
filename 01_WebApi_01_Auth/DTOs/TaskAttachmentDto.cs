
using System.Text.Json.Serialization;

namespace Core.Dtos
{
	public readonly struct TaskAttachmentRequestDto
	{
		public int? Id { get; init; }
		public required string FileData { get; init; }
		public int TaskId { get; init; }
		public string? FileName { get; init; }
	}
	
	public record TaskAttachmentBaseResponseDto
	{
		public int Id { get; init; }
		public string? FileName { get; init; }
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? TaskId { get; init; }
		
		public static TaskAttachmentBaseResponseDto From(Entities.TaskAttachment? attachment)
		{
			if (attachment == null) return null!;
			
			return new TaskAttachmentBaseResponseDto
			{
				Id = attachment.Id,
				TaskId = attachment.TaskId,
				FileName = attachment.FileName
			};
		}
	}
	
	public sealed record TaskAttachmentResponseFullDto : TaskAttachmentBaseResponseDto
	{
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string? FileData { get; init; }
		// [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public TaskResponseFullDto? Task { get; init; }
		
		
		public static new TaskAttachmentResponseFullDto From(Entities.TaskAttachment? attachment)
		{
			if (attachment == null) return null!;
			
			return new TaskAttachmentResponseFullDto
			{
				Id = attachment.Id,
				FileName = attachment.FileName,
				
				FileData = attachment.FileData,
				
				TaskId = attachment.Task != null ? null : attachment.TaskId,
				Task = TaskResponseFullDto.From(attachment.Task)
			};
		}
	}
}