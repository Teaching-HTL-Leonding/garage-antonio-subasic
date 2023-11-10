using System.Text;
using Newtonsoft.Json;

namespace Garage.Logic;

public class Garage
{
    public ParkingSpot?[] ParkingSpots { get; } = new ParkingSpot[50];

    private static string CenterString(string? str, int length)
    {
        var padding = Math.Max(length - (str?.Length ?? 0), 0) / 2d;

        var before = new string(' ', (int)Math.Ceiling(padding));
        var after = new string(' ', (int)padding);

        return $"{before}{str?.Trim()}{after}";
    }

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
        var maxSpotLength = Math.Max(ParkingSpots.Length.ToString().Length, "Spot".Length);
        var maxLicensePlateLength = Math.Max(ParkingSpots.Max(parkingSpot => parkingSpot?.LicensePlate.Length ?? 0), "License Plate".Length);

        var stringBuilder = new StringBuilder($"| {CenterString("Spot", maxSpotLength)} | {CenterString("License Plate", maxLicensePlateLength)} |");
        stringBuilder.AppendLine($"\n| {new string('-', maxSpotLength)} | {new string('-', maxLicensePlateLength)} |");

        for (var i = 0; i < ParkingSpots.Length; i++)
        {
            stringBuilder.AppendLine($"| {CenterString((i + 1).ToString(), maxSpotLength)} | {CenterString(ParkingSpots[i]?.LicensePlate, maxLicensePlateLength)} |");
        }

        return stringBuilder.ToString();
    }

    public void Save(string filename)
    {
        var parkingSpots = (ParkingSpotJsonRepresentation[])this;
        var json = JsonConvert.SerializeObject(parkingSpots, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    public static Garage Load(string filename = "")
    {
        try
        {
            var json = File.ReadAllText(filename);
            var parkingSpots = JsonConvert.DeserializeObject<ParkingSpotJsonRepresentation[]>(json);
            return (Garage)parkingSpots;
        }
        catch (FileNotFoundException) { return new Garage(); }
    }

    public override string ToString() => GenerateReport();

    public static explicit operator ParkingSpotJsonRepresentation[](Garage garage)
    {
        var garageJsonRepresentation = new List<ParkingSpotJsonRepresentation>();

        for (var i = 0; i < garage.ParkingSpots.Length; i++)
        {
            if (garage.ParkingSpots[i] is not null)
            {
                garageJsonRepresentation.Add(new ParkingSpotJsonRepresentation
                {
                    Number = i + 1,
                    LicensePlate = garage.ParkingSpots[i]!.LicensePlate,
                    EntryDate = garage.ParkingSpots[i]!.EntryDate
                });
            }
        }

        return garageJsonRepresentation.ToArray();
    }

    public static explicit operator Garage(ParkingSpotJsonRepresentation[]? parkingSpots)
    {
        var garage = new Garage();

        if (parkingSpots is not null)
        {
            foreach (var parkingSpot in parkingSpots)
            {
                garage.Occupy(parkingSpot.Number, parkingSpot.LicensePlate, parkingSpot.EntryDate.DateTime);
            }
        }

        return garage;
    }
}
