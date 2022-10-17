using NRules.Fluent.Dsl;
using NRulesAPI.Entities;

namespace NRulesAPI.Rules
{
    [Repeatability(NRules.RuleModel.RuleRepeatability.NonRepeatable)]
    public class StudentInterestsRules : Rule
    {
        public override void Define()
        {
            Student student = default;
            IEnumerable<Matricula> matriculas = default;

            When()
                .Match<Student>(() => student)
                .Query(() => matriculas, x => x
                    .Match<Matricula>(
                        m => m.Student == student,
                        m => m.FinanceType > FinanceTypes.Cash,
                        m => m.Payments > 12,
                        m => m.Interests == 0.0M)
                    .Collect()
                    .Where(c => c.Any()));

            Then()
                .Do(ctx => ApplyInterests(matriculas))
                .Do(ctx => ctx.UpdateAll(matriculas));
        }

        private static void ApplyInterests(IEnumerable<Matricula> matriculas)
        {
            foreach (var matricula in matriculas)
            {
                matricula.Interests = decimal.Round(matricula.Price / matricula.Payments, 2);
            }
        }
    }
}
