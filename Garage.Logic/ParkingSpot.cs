using Newtonsoft.Json;

namespace Garage.Logic;

public record ParkingSpot(string LicensePlate, DateTime EntryDate);

public class ParkingSpotJsonRepresentation
{
    [JsonProperty("spot-number")]
    public int Number { get; set; }

    [JsonProperty("license-plate")]
    public string LicensePlate { get; set; } = "";

    [JsonProperty("entry-date")]
    public DateTimeOffset EntryDate { get; set; }
}
