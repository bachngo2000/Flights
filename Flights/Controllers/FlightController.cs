using Flights.ReadModels;
using Microsoft.AspNetCore.Mvc;
using Flights.Dtos;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {

        private readonly ILogger<FlightController> _logger;

        static Random random = new Random();


        static private FlightRm[] flights = new FlightRm[]
        {

        new (   Guid.NewGuid(),
                "American Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Los Angeles",DateTime.Now.AddHours(random.Next(1, 3))),
                new TimePlaceRm("Istanbul",DateTime.Now.AddHours(random.Next(4, 10))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Emirates",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Dubai",DateTime.Now.AddHours(random.Next(1, 10))),
                new TimePlaceRm("Mumbai",DateTime.Now.AddHours(random.Next(4, 15))),
                random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "British Airways",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("London",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlaceRm("Rome",DateTime.Now.AddHours(random.Next(4, 18))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Singapore Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Amsterdam",DateTime.Now.AddHours(random.Next(1, 21))),
                new TimePlaceRm("Glasgow, Scotland",DateTime.Now.AddHours(random.Next(4, 21))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "All Nippon Airways",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Zurich",DateTime.Now.AddHours(random.Next(1, 23))),
                new TimePlaceRm("Tokyo",DateTime.Now.AddHours(random.Next(4, 25))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Korean Air",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Seoul",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlaceRm("Madrid",DateTime.Now.AddHours(random.Next(4, 19))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Turkish Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Chicago",DateTime.Now.AddHours(random.Next(1, 55))),
                new TimePlaceRm("Paris",DateTime.Now.AddHours(random.Next(4, 58))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Air France",
                random.Next(90, 5000).ToString(),
                new TimePlaceRm("Moscow",DateTime.Now.AddHours(random.Next(1, 58))),
                new TimePlaceRm("New York City",DateTime.Now.AddHours(random.Next(4, 60))),
                    random.Next(1, 853))

    };

        static private IList<BookDto> Bookings = new List<BookDto>();
        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
        public IEnumerable<FlightRm> Search()
        => flights;

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm), 200)]
        [HttpGet("{id}")]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = flights.SingleOrDefault(f => f.Id == id);

            if (flight == null)
                return NotFound();

            return Ok(flight);

        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200)]
        public IActionResult Book(BookDto dto)
        {
            
            System.Diagnostics.Debug.WriteLine($"Booking a new flight {dto.FlightId}");
           
            var flightFound = flights.Any(f => f.Id == dto.FlightId);

            if (flightFound == false)
                return NotFound();

            Bookings.Add(dto);

            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });
        }

    }
}