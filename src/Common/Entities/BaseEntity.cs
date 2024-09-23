using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;

namespace Common.Entities
{
    public class BaseEntity
	{
        [ExplicitKey, Required]
        [Column("id")]
        public string Id { get; set; }
    }
}

