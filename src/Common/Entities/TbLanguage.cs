using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Common.Entities
{
	[Table("tb_language", Schema = "public")]
	public class TbLanguage
	{
		[Key, Required]
		[Column("id")]
		public string Id { get; set; }
		[Column("name")]
		public string Name { get; set; }
		[Column("description")]
		public string Description { get; set; }
	}
}