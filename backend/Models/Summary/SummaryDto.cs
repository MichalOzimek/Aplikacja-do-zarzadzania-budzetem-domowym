namespace ProjectSoftwareWorkshop.Models.Summary
{
    public class SummaryDto
    {
        public decimal Incomes { get; set; }
        public decimal Expenses { get; set; }

        public decimal Balance { get; set; }
    }

    public class BalancePointDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }       
        public DateTime Date { get; set; }

        public decimal Incomes { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }
}
