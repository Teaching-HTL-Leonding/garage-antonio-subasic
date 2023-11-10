using System.Text;

namespace Garage.Logic;

public record ParkingSpot(string LicensePlate, DateTime EntryDate);

public class Garage
{
    public ParkingSpot?[] ParkingSpots { get; } = new ParkingSpot[50];


    public bool IsOccupied(int parkingSpotNumber) => ParkingSpots[parkingSpotNumber - 1] != null;

    public void Occupy(int parkingSpotNumber, string licensePlate, DateTime entryTime)
    {
        ParkingSpots[parkingSpotNumber - 1] = new(licensePlate, entryTime);
    }

    public decimal Exit(int parkingSpotNumber, DateTime exitTime)
    {
        var minutes = (exitTime - ParkingSpots[parkingSpotNumber - 1]!.EntryDate).TotalMinutes;
        ParkingSpots[parkingSpotNumber - 1] = null;
        return minutes < 15 ? 0 : (decimal)Math.Ceiling(minutes / 30) * 3;
    }

    public string GenerateReport()
    {
        var maxSpotLength = ParkingSpots.Length.ToString().Length;
        var maxLicensePlateLength = ParkingSpots.Max(parkingSpot => parkingSpot?.LicensePlate.Length ?? 0);

        var stringBuilder = new StringBuilder($"| {CenterString("Spot", maxSpotLength)} | {CenterString("License Plate", maxLicensePlateLength)} |");
        stringBuilder.AppendLine($"\n| {new string('-', maxSpotLength)} | {new string('-', maxLicensePlateLength)} |");

        for (var i = 0; i < ParkingSpots.Length; i++)
        {
            stringBuilder.AppendLine($"| {CenterString((i + 1).ToString(), maxSpotLength)} | {CenterString(ParkingSpots[i]?.LicensePlate, maxLicensePlateLength)} |");
        }

        return stringBuilder.ToString();
    }

    public override string ToString() => GenerateReport();

    private static string CenterString(string? str, int length)
    {
        var padding = Math.Max(length - (str?.Length ?? 0), 0) / 2d;

        var before = new string(' ', (int)Math.Ceiling(padding));
        var after = new string(' ', (int)padding);

        return $"{before}{str?.Trim()}{after}";
    }
}
