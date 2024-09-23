using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("ref_tables", Schema = "sdi")]
    public class RefTablesTr
    {
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
        [Column("id")]
        public string Id { get; set; }
        [Dapper.Contrib.Extensions.ExplicitKey, Required]
        [Column("language_id")]
        public string LanguageId { get; set; }
        [Column("info", TypeName = "jsonb")]
        public List<RefTablesJsonData> Info { get; set; }

        public TbLanguage Language { get; set; }

    }
}