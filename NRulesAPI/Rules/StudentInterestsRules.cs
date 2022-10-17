using NRules.Fluent.Dsl;
using NRulesAPI.Entities;

namespace NRulesAPI.Rules
{
    public class StudentInterestsRules : Rule
    {
        public override void Define()
        {
            Student student = default;
            IEnumerable<Matricula> orders = default;

            When()
                .Match<Student>(() => student)
                .Query(() => orders, x => x
                    .Match<Matricula>(
                        o => o.Student == student,
                        m => m.FinanceType > FinanceTypes.Cash,
                        o => o.Payments > 12,
                        o => o.Interests == 0.0M)
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => ApplyDiscount(orders))
                .Do(ctx => ctx.UpdateAll(orders));
        }

        private static void ApplyDiscount(IEnumerable<Matricula> matriculas)
        {
            foreach (var matricula in matriculas)
            {
                matricula.Interests = decimal.Round(matricula.Price / matricula.Payments, 2);
            }
        }
    }
}
