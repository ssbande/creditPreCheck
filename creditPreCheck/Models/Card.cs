namespace creditPreCheck.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public decimal APR { get; set; }
        public int MinAge { get; set; }
        public int IncomeThreshold { get; set; }
    }
}
