namespace TrigonosEnergyWebAPI.DTO
{
    public class BillingWindowsDto
    {
        public string Natural_key { get; set; }

        public int billing_type { get; set; }
        public string periods { get; set; }
        public string created_ts { get; set; }
        public string updated_ts { get; set; }
        public DateTime period { get; set; }
    }
}
