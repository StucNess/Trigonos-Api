namespace TrigonosEnergyWebAPI.DTO
{
    public class excelHistoryDto
    {
        public string excelName { get; set; }
        public string status { get; set; }
        public DateTime date { get; set; }
        public int? idParticipant { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }
    }
}
