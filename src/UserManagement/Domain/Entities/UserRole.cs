
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
	[Table("user_role", Schema = "usr")]
	public class UserRole {
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("user_id")]
		public string Id { get; set; }
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("role_id")]
		public string RoleId { get; set; }
		public void SaveHistory(System.Data.IDbConnection dbConnection, string userid, string endpointName, string note = "-", string attachFile = "-")
		{
			TbHistory<UserRole> tbHistory = new()
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
