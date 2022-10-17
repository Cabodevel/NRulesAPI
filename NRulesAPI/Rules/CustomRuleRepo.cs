using NRules.RuleModel;
using NRules.RuleModel.Builders;
using System.Linq.Expressions;

namespace NRulesAPI.Rules
{
    public class CustomRuleRepo : IRuleRepository
    {
        private readonly IRuleSet _ruleSet = new RuleSet("RuleSet");

        public IEnumerable<IRuleSet> GetRuleSets()
        {
            return new[] { _ruleSet };
        }

        public void LoadRules<T, U>(Expression<Func<T, bool>> leftCondition,
            Expression<Func<U, bool>> rigthCondition,
            Expression<Func<U, T, bool>> joinCondition,
            Expression<Action<IContext, T, U>> action)
        {
            //Assuming there is only one rule in this example
            var rule = BuildRule(leftCondition, rigthCondition, joinCondition, action);
            _ruleSet.Add(new[] { rule });
        }

        private IRuleDefinition BuildRule<T,U>(Expression<Func<T, bool>> leftCondition,
            Expression<Func<U, bool>> rigthCondition, 
            Expression<Func<U, T, bool>> joinCondition,
            Expression<Action<IContext, T, U>> action)
        {
            //Create rule builder
            var builder = new RuleBuilder();
            builder.Name("TestRule");

            //Build conditions
            PatternBuilder customerPattern = builder.LeftHandSide().Pattern(typeof(T), nameof(T));
            customerPattern.Condition(leftCondition);

            PatternBuilder orderPattern = builder.LeftHandSide().Pattern(typeof(U), nameof(U));
           
            orderPattern.Condition(joinCondition);
            orderPattern.Condition(rigthCondition);

            builder.RightHandSide().Action(action);

            //Build rule model
            return builder.Build();
        }

    }
}
