using System.Globalization;

const int CAR_ENTRY = 1;
const int CAR_EXIT = 2;
const int REPORT = 3;
const int EXIT = 4;

var garage = new Garage.Logic.Garage();

Console.WriteLine($@"What do you want to do?
{CAR_ENTRY}) Enter a car entry
{CAR_EXIT}) Enter a car exit
{REPORT}) Generate report
{EXIT}) Exit");

while (true)
{
    var selection = GetInputFromUser<int>("\nYour selection");

    var parkingSpotNumber = 0;
    var exit = selection == EXIT;

    if (selection is CAR_ENTRY or CAR_EXIT)
    {
        parkingSpotNumber = GetInputFromUser<int>("\nEnter parking spot number");
        exit = exit || (garage.IsOccupied(parkingSpotNumber) ^ (selection != CAR_ENTRY));
    }

    switch (selection)
    {
        case CAR_ENTRY:
            if (exit) { Console.WriteLine("Parking spot is occupied"); }
            else
            {
                Console.Write($"Enter license plate: ");
                var licensePlate = Console.ReadLine()!;
                var entryDateTime = GetInputFromUser<DateTime>("Enter entry date/time");
                garage.Occupy(parkingSpotNumber, licensePlate, entryDateTime);
            }

            break;

        case CAR_EXIT:
            if (exit) { Console.WriteLine("Parking spot is not occupied"); }
            else
            {
                var exitDateTime = GetInputFromUser<DateTime>("Enter exit date/time");
                Console.WriteLine($"Costs are {garage.Exit(parkingSpotNumber, exitDateTime)}€");
            }

            break;

        case REPORT:
            Console.WriteLine(garage);
            break;

        case EXIT:
            Console.WriteLine("Good bye!");
            return;
    }
}

static T GetInputFromUser<T>(string message) where T : IParsable<T>
{
    Console.Write($"{message}: ");
    var input = Console.ReadLine()!;
    return T.Parse(input, CultureInfo.InvariantCulture);
}
