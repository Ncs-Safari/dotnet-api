namespace Common.Dtos
{
    public class EmailMessageDto
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Overview { get; set; }
        public string Content { get; set; }
        public List<string> Attachments { get; set; }

    }
}