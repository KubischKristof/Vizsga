namespace ChefBerlesAPI.Model
{
    public class Berles
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public int ChefId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DailyRate { get; set; }
        public int BaseFee { get; set; }
        public int TotalPrice => BaseFee + ((EndDate.ToDateTime(TimeOnly.MinValue) - StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1) * DailyRate;
    }
}
