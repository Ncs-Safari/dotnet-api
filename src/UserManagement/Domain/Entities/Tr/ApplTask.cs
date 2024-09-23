using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("appl_task", Schema = "usr_tr")]
	public class ApplTaskTr
	{
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("id")]
		public string Id { get; set; }
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
		[Column("language_id")]
		public string LanguageId { get; set; }

		[Column("task_name")]
		public string TaskName { get; set; }
		[Column("description")]
		public string Description { get; set; }
		public TbLanguage Language { get; set; }		
		
	}
}