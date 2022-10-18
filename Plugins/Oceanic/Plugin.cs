using TrainingServer;
using TrainingServer.Extensibility;

namespace Oceanic;

public class Plugin : IServerPlugin
{
    public string FriendlyName => "Oceanic Traffic Loader";
    public string Maintainer => "Aldo (190881)";

    public bool CheckIntercept(string sender, string message) =>
        message.Trim().ToUpperInvariant().Equals("OCC");

    public string? MessageReceived(IServer server, string sender, string message)
    {
        string route = "LIMRI 5320N 5330N 5340N ELSIR";

        Flightplan fpl =
            new(
                'I', 'S', "1/B77W/H-SDE3FHIM3RW/LB1", "M084", "EGLL", new DateTime(2022, 10, 17, 22, 15, 00), new DateTime(2022, 10, 17, 22, 15, 00),
                "F350", "KJFK", 7, 0, 9, 0, "KEWR", "SEL/ABCD CS/CHEESEANDSANDWHICH RMK/NO CHARTS NO BRAINS", route
            );
        Coordinate last = new() { Latitude = 52, Longitude = -14 };

        var ac = server.SpawnAircraft("AAL123", fpl, last, 270, 450, 35000);

        if (ac is null)
            return "Callsign in use";

        double distance(Coordinate a, Coordinate b) => 
            Math.Abs(a.Latitude - b.Latitude) + 
            Math.Abs(a.Longitude - b.Longitude);

        foreach (string fix in route.Split())
        {
            if (fix[^1..].ToUpperInvariant() == "N" && fix[..^1].All(char.IsDigit))
                last = new() { Latitude = int.Parse(fix[..2]), Longitude = -int.Parse(fix[2..]) };
            else
                last =
                    IvaoApi.GetFixes(fix)!.items
                    .Select(f => new Coordinate() { Latitude = f.latitude, Longitude = f.longitude })
                    .MinBy(c => distance(c, last));

            ac.FlyDirect(last);
            Console.WriteLine($" {last.Latitude:00.###}/{last.Longitude:#00.###}");
        }

        return null;
    }
}