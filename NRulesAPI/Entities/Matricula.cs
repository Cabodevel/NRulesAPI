namespace NRulesAPI.Entities
{
    public class Matricula
    {
        public decimal Price { get; set; }
        public decimal Interests { get; set; }
        public int Payments { get; set; }
        public FinanceTypes FinanceType { get; set; }
        public Student Student { get; set; }

        public Matricula(decimal price, int payments, FinanceTypes financeType, Student student)
        {
            Price = price;
            Payments = payments;
            FinanceType = financeType;
            Student = student;
        }
    }
}
