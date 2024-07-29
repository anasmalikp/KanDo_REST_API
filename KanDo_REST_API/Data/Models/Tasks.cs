namespace KanDo_REST_API.Data.Models
{
    public class Tasks
    {
        public string? id { get; set; }
        public DateTime? updatedAt { get; set; }
        public string? boardid { get; set; }
        public string? statusid { get; set; }
        public string? tasktitle { get; set; }
        public string? description { get; set; }
    }
}
