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
        private readonly List<Student> students;
        private readonly List<Matricula> matriculas;

        public RulesController()
        {
            students = new List<Student> {
                new Student(1, "Carlos") ,
                new Student(2, "Maca") ,
                new Student(3, "Juan") ,
            };
            matriculas = new List<Matricula>
            {
                new Matricula(3500, 13, FinanceTypes.Finance, students[0]),
                new Matricula(4500, 16, FinanceTypes.Refinance, students[1]),
                new Matricula(2000, 1, FinanceTypes.Cash, students[2])
            };
        }


        [HttpGet]
        public IActionResult Get()
        {
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(StudentInterestsRules).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            //Insert facts into rules engine's memory
            session.TryInsertAll(students);

            session.TryInsertAll(matriculas);


            //Start match/resolve/act cycle
            session.Fire();

            return new OkObjectResult(matriculas);
        }
    }
}