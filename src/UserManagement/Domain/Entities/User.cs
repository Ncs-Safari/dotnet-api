using System.ComponentModel.DataAnnotations.Schema;
using Common.Entities;
namespace UserManagement.Domain.Entities
{
    [Table("user", Schema = "usr")]
	public class User : BaseEntity{
		[Column("username")]
		public string Username { get; set; }
		[Column("password")]
		public string Password { get; set; }
		[Column("email")]
		public string Email { get; set; }
		[Column("first_name")]
		public string FirstName { get; set; }
		[Column("middle_name")]
		public string? MiddleName { get; set; }
		[Column("last_name")]
		public string? LastName { get; set; }
		[Column("address")]
		public string Address { get; set; }
		[Column("phone_number")]
		public string PhoneNumber { get; set; }
		[Column("mobile_number")]
		public string MobileNumber { get; set; }
		[Column("orgid")]
		public string Orgid { get; set; }
		[Column("status")]
		public int Status { get; set; }
		[Column("last_login")]
		public DateTime? LastLogin { get; set; }
		[Column("created_by")]
		public string CreatedBy { get; set; }
		[Column("created_date")]
		public DateTime CreatedDate { get; set; }
		[Column("updated_by")]
		public string UpdatedBy { get; set; }
		[Column("updated_date")]
		public DateTime? UpdatedDate { get; set; }

		[NotMapped]
        public string FullName
        {
            get
            {
                string fullName = FirstName;
                if (!string.IsNullOrEmpty(MiddleName))
                    fullName += " " + MiddleName;
                if (!string.IsNullOrEmpty(LastName))
                    fullName += " " + LastName;
                return fullName.Trim();
            }
        }
        public IEnumerable<Role>? Roles { get; set; }
        public IEnumerable<UserFile>? Files { get; set; }
        public void SaveHistory(System.Data.IDbConnection dbConnection, string userid, string endpointName, string note = "-", string attachFile = "-")
        {
            TbHistory<User> tbHistory = new()
            {
                CreatedBy = userid,
                EndpointName = endpointName,
                Note = note,
                AttachFile = attachFile,
                HistoryData = System.Text.Json.JsonSerializer.Serialize(this)
            };
            tbHistory.Save(dbConnection);
        }		
	}
}