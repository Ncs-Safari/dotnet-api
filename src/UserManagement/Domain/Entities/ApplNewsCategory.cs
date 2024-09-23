using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_news_category", Schema = "usr")]
	public class ApplNewsCategory : BaseEntity{
		[Column("name")]
		public string Name { get; set; }
		[Column("description")]
		public string Description { get; set; }
		[Column("file_logo")]
		public string FileLogo { get; set; }
	}
}