using ChefBerlesAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ChefBerlesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BerlesekController : ControllerBase
    {
        private static readonly List<Berles> Berlesek = new();
        private static int _nextId = 1;

        private static void Seed()
        {
            if (Berlesek.Any()) return;
            Berlesek.AddRange(new[]
            {
            new Berles { Id = _nextId++, Uid = 101, ChefId = 1,  StartDate = new DateOnly(2025,  3, 15), EndDate = new DateOnly(2025,  3, 18), DailyRate = 250, BaseFee = 500 },
            new Berles { Id = _nextId++, Uid = 102, ChefId = 3,  StartDate = new DateOnly(2025,  4,  1), EndDate = new DateOnly(2025,  4, 10), DailyRate = 300, BaseFee = 600 },
            new Berles { Id = _nextId++, Uid = 103, ChefId = 2,  StartDate = new DateOnly(2025,  5, 20), EndDate = new DateOnly(2025,  5, 27), DailyRate = 275, BaseFee = 550 },
            new Berles { Id = _nextId++, Uid = 101, ChefId = 5,  StartDate = new DateOnly(2025,  6,  1), EndDate = new DateOnly(2025,  6,  5), DailyRate = 200, BaseFee = 400 },
            new Berles { Id = _nextId++, Uid = 104, ChefId = 1,  StartDate = new DateOnly(2025,  7, 10), EndDate = new DateOnly(2025,  7, 14), DailyRate = 250, BaseFee = 500 },
        });
        }

        [HttpGet]
        public ActionResult<IEnumerable<Berles>> GetAll()
        {
            Seed();
            return Ok(Berlesek);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Berles> GetById(int id)
        {
            Seed();
            var berles = Berlesek.FirstOrDefault(b => b.Id == id);
            return berles is null ? NotFound() : Ok(berles);
        }

        [HttpPost]
        public ActionResult<Berles> Post([FromBody] CreateBerlesRequest request)
        {
            Seed();

            if (!DateOnly.TryParseExact(request.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate))
                return BadRequest("A kezdő dátum formátuma érvénytelen. Használjon YYYY-MM-DD formátumot.");

            if (!DateOnly.TryParseExact(request.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate))
                return BadRequest("A záró dátum formátuma érvénytelen. Használjon YYYY-MM-DD formátumot.");

            var holnap = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            if (startDate < holnap)
                return BadRequest("A bérlés kezdő dátuma nem lehet korábbi, mint holnap.");

            var napok = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
            if (napok < 3)
                return BadRequest("A bérlés időtartamának legalább 3 napnak kell lennie.");
            if (napok > 14)
                return BadRequest("A bérlés időtartama legfeljebb 14 nap lehet.");

            var atfedes = Berlesek.Any(b =>
                b.ChefId == request.ChefId &&
                startDate <= b.EndDate &&
                endDate >= b.StartDate);

            if (atfedes)
                return BadRequest("Ugyanezt a séfet az adott időszakban már lefoglalták.");

            var uj = new Berles
            {
                Id = _nextId++,
                Uid = request.Uid,
                ChefId = request.ChefId,
                StartDate = startDate,
                EndDate = endDate,
                DailyRate = request.DailyRate,
                BaseFee = request.BaseFee
            };

            Berlesek.Add(uj);
            return CreatedAtAction(nameof(GetById), new { id = uj.Id }, uj);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Seed();
            var berles = Berlesek.FirstOrDefault(b => b.Id == id);
            if (berles is null) return NotFound();
            Berlesek.Remove(berles);
            return NoContent();
        }
    }
}
