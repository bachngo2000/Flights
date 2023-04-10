using System.ComponentModel;

namespace Flights.Dtos
{
    public record FlightSearchParameters
    (

        [DefaultValue("04/10/2023 10:30:00 AM")]
        DateTime? FromDate,

        [DefaultValue("04/11/2023 10:30:00 AM")]
        DateTime? ToDate,

        [DefaultValue("Los Angeles")]
        string? From,

        [DefaultValue("Washington, DC.")]
        string? Destination,

        [DefaultValue(1)]
        int? NumberOfPassengers

     );

}