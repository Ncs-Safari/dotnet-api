using System.Data;
using System.Reflection;
using Dapper;
namespace Common.Entities
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tb_history", Schema = "public")]
    public class TbHistory<ClassTable>
    {

        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("table_name")]
        public string TableName
        {
            get
            {
                var tableAttribute = typeof(ClassTable).GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute)) as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
                return $"{tableAttribute.Schema}.{tableAttribute.Name}";
            }
            set { }
        }
        [System.ComponentModel.DataAnnotations.Schema.Column("endpoint_name")]
        public string EndpointName { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("created_by")]
        public string CreatedBy { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("created_date")]
        public DateTime CreatedDate
        {
            get
            {
                return DateTime.Now;
            }
            set { }
        }
        [System.ComponentModel.DataAnnotations.Schema.Column("note")]
        public string Note { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("attach_file")]
        public string AttachFile { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Column("history_data")]
        public string HistoryData { get; set; }
        public void Save(IDbConnection conn)
        {
            this.EndpointName = string.IsNullOrEmpty(this.EndpointName) ? "-" : this.EndpointName;
            this.Note = string.IsNullOrEmpty(this.Note) ? "-" : this.Note;
            //this.AttachFile = attachFile;
            //this.HistoryData = historyData;
            string sql = string.Empty;
            sql += "INSERT INTO public.tb_history(";
            sql += "table_name, created_by, created_date, history_data, note, attach_file, endpoint_name)";
            sql += "	VALUES (@tablename, @createdby, @createddate, @historydata::jsonb, @note, @attachfile, @EndpointName)";

            conn.ExecuteScalar(sql, this);
        }
    }
}