using System.Globalization;

const int CAR_ENTRY = 1;
const int CAR_EXIT = 2;
const int REPORT = 3;
const int EXIT = 4;

var garage = Garage.Logic.Garage.Load("garage.json");

Console.WriteLine($@"What do you want to do?
{CAR_ENTRY}) Enter a car entry
{CAR_EXIT}) Enter a car exit
{REPORT}) Generate report
{EXIT}) Exit");

while (true)
{
    var selection = GetInputFromUser<int>("\nYour selection");
    var invalidInput = selection == EXIT;
    var parkingSpot = 0;

    if (selection is CAR_ENTRY or CAR_EXIT)
    {
        parkingSpot = GetInputFromUser<int>("\nEnter parking spot number");
        invalidInput = invalidInput || (garage.IsOccupied(parkingSpot) ^ (selection != CAR_ENTRY));
    }

    switch (selection)
    {
        case CAR_ENTRY:
            if (invalidInput) { Console.WriteLine("Parking spot is occupied"); }
            else
            {
                Console.Write($"Enter license plate: ");
                var licensePlate = Console.ReadLine()!;
                var entryDateTime = GetInputFromUser<DateTime>("Enter entry date/time");

                garage.Occupy(parkingSpot, licensePlate, entryDateTime);
            }
            break;

        case CAR_EXIT:
            if (invalidInput) { Console.WriteLine("Parking spot is not occupied"); }
            else
            {
                var exitDateTime = GetInputFromUser<DateTime>("Enter exit date/time");
                Console.WriteLine($"Costs are {garage.Exit(parkingSpot, exitDateTime)}€");
            }
            break;

        case REPORT:
            Console.WriteLine($"\n{garage}");
            break;

        case EXIT:
            Console.WriteLine("\nGood bye!");
            garage.Save("garage.json");
            return;
    }
}

static T GetInputFromUser<T>(string message) where T : IParsable<T>
{
    Console.Write($"{message}: ");
    var input = Console.ReadLine()!;
    return T.Parse(input, CultureInfo.InvariantCulture);
}
