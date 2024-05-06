namespace VPTEventApi.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Location { get; set; }
        public int VisitorLimit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ChairsReserved { get; set; }
        public List<Part> Parts { get; set; }

    }
}
