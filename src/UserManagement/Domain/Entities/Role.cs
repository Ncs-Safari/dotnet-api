using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("role", Schema = "usr")]
	public class Role : BaseEntity{
		[Column("code")]
		public string Code { get; set; }
		[Column("name")]
		public string Name { get; set; }
	}
}