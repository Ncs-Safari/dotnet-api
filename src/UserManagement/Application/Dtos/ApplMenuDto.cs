namespace UserManagement.Application.Dtos
{
    public class ApplMenuDto
    {
        public string number { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string to { get; set; }
        public string route { get; set; }
        public string customid { get; set; }
        public string tag { get; set; }
        public List<ApplMenuDto> childrens { get; set; }
    }
}