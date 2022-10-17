using Microsoft.AspNetCore.Mvc;
using NRules;
using NRules.Fluent;
using NRulesAPI.Entities;
using NRulesAPI.Rules;

namespace NRulesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RulesController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(StudentInterestsRules).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            //Load domain model
            var student = new Student(1, "Carlos");
            var order1 = new Matricula(3500, 13, FinanceTypes.Finance, student);
            var order2 = new Matricula(2000, 1, FinanceTypes.Cash, student);

            //Insert facts into rules engine's memory
            session.Insert(student);
            session.Insert(order1);
            session.Insert(order2);

            //Start match/resolve/act cycle
            session.Fire();

            return new OkObjectResult(new List<Matricula> { order1, order2 });
        }
    }
}