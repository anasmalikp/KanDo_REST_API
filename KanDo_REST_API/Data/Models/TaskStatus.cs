namespace KanDo_REST_API.Data.Models
{
    public class TaskStatus
    {
        public string? id { get; set; }
        public string? statusname { get; set; }
        public string? boardid { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
