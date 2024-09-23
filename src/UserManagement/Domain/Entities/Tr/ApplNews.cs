using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_news", Schema = "usr_tr")]
	public class ApplNewsTr
	{
		[Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("id")]
		public string Id { get; set; }
		[Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("language_id")]
		public string LanguageId { get; set; }
		[Column("title")]
		public string Title { get; set; }
		[Column("description")]
		public string Description { get; set; }
		public TbLanguage Language { get; set; }

	}
}