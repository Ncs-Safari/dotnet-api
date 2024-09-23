
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserManagement.Domain.Entities
{
	[Table("appl_extra", Schema = "usr")]
	public class ApplExtra {
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("appl_id")]
		public string Id { get; set; }
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("key")]
		public string Key { get; set; }
		[Column("type")]
		public string Type { get; set; }
		[Column("value")]
		public string Value { get; set; }
	}
}