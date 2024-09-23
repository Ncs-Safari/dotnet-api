
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
	[Table("user_file", Schema = "usr")]
	public class UserFile
	{
		[Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("user_id")]
		public string Id { get; set; }
		[Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("type")]
		public string Type { get; set; }
		[Column("category")]
		public string? Category { get; set; }
		[Column("file_name")]
		public string FileName { get; set; }
		[Column("file_thumbnail")]
		public string? FileThumbnail { get; set; }
		[Column("title")]
		public string Title { get; set; }
		[Column("description")]
		public string? Description { get; set; }

		public void SaveHistory(System.Data.IDbConnection dbConnection, string userid, string endpointName, string note = "-", string attachFile = "-")
		{
			TbHistory<UserFile> tbHistory = new()
			{
				CreatedBy = userid,
				EndpointName = endpointName,
				Note = note,
				AttachFile = attachFile,
				HistoryData = System.Text.Json.JsonSerializer.Serialize(this)
			};
			tbHistory.Save(dbConnection);
		}
	}
}