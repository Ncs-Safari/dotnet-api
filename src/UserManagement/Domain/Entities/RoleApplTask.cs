
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserManagement.Domain.Entities
{
	[Table("role_appl_task", Schema = "usr")]
	public class RoleApplTask {
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("role_id")]
		public string Id { get; set; }
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("appl_task_id")]
		public string ApplTaskId { get; set; }
	}
}