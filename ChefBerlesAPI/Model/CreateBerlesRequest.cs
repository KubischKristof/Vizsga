namespace ChefBerlesAPI.Model
{
    public class CreateBerlesRequest
    {
        public int Uid { get; set; }
        public int ChefId { get; set; }
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public int DailyRate { get; set; }
        public int BaseFee { get; set; }
    }
}
