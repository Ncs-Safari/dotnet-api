using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;

namespace UserManagement.Domain.Entities
{
    [Table("ref_tables", Schema = "sdi")]
    public class RefTables : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("info", TypeName = "jsonb")]
        public List<RefTablesJsonData> Info { get; set; }
    }
}