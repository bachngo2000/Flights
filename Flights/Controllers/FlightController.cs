using Flights.ReadModels;
using Flights.Domain.Entities;
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


        static private Flight[] flights = new Flight[]
        {

        new (   Guid.NewGuid(),
                "American Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlace("Los Angeles",DateTime.Now.AddHours(random.Next(1, 3))),
                new TimePlace("Istanbul",DateTime.Now.AddHours(random.Next(4, 10))),
                    2),
        new (   Guid.NewGuid(),
                "Emirates",
                random.Next(90, 5000).ToString(),
                new TimePlace("Dubai",DateTime.Now.AddHours(random.Next(1, 10))),
                new TimePlace("Mumbai",DateTime.Now.AddHours(random.Next(4, 15))),
                random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "British Airways",
                random.Next(90, 5000).ToString(),
                new TimePlace("London",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlace("Rome",DateTime.Now.AddHours(random.Next(4, 18))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Singapore Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlace("Amsterdam",DateTime.Now.AddHours(random.Next(1, 21))),
                new TimePlace("Glasgow, Scotland",DateTime.Now.AddHours(random.Next(4, 21))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "All Nippon Airways",
                random.Next(90, 5000).ToString(),
                new TimePlace("Zurich",DateTime.Now.AddHours(random.Next(1, 23))),
                new TimePlace("Tokyo",DateTime.Now.AddHours(random.Next(4, 25))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Korean Air",
                random.Next(90, 5000).ToString(),
                new TimePlace("Seoul",DateTime.Now.AddHours(random.Next(1, 15))),
                new TimePlace("Madrid",DateTime.Now.AddHours(random.Next(4, 19))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Turkish Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlace("Chicago",DateTime.Now.AddHours(random.Next(1, 55))),
                new TimePlace("Paris",DateTime.Now.AddHours(random.Next(4, 58))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Air France",
                random.Next(90, 5000).ToString(),
                new TimePlace("Moscow",DateTime.Now.AddHours(random.Next(1, 58))),
                new TimePlace("New York City",DateTime.Now.AddHours(random.Next(4, 60))),
                    random.Next(1, 853))

    };

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<FlightRm>), 200)]
        public IEnumerable<FlightRm> Search()
        {
            var flightRmList = flights.Select(flight => new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingNumberOfSeats
                ));//.toArray();

            return flightRmList;
        }


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

            var readModel = new FlightRm(
                 flight.Id,
                 flight.Airline,
                 flight.Price,
                 new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                 new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                 flight.RemainingNumberOfSeats
                 );

            return Ok(readModel);

        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200)]
        public IActionResult Book(BookDto dto)
        {
            
            System.Diagnostics.Debug.WriteLine($"Booking a new flight {dto.FlightId}");
           
            var flight = flights.SingleOrDefault(f => f.Id == dto.FlightId);

            if (flight == null)
                return NotFound();

            if(flight.RemainingNumberOfSeats < dto.NumberOfSeats)
            {
                return Conflict(new { message = "The number of requested seats exceeds the number of remaining seats" });
            }

            flight.Bookings.Add(
                new Booking(
                    dto.PassengerEmail,
                    dto.NumberOfSeats)
                );

            flight.RemainingNumberOfSeats -= dto.NumberOfSeats;

            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });
        }

    }
}