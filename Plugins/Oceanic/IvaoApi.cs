using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Oceanic;

internal class IvaoApi
{
    public static Navaids? GetFixes(string fixName) =>
        new HttpClient().GetFromJsonAsync<Navaids>("https://api.ivao.aero/v2/navaids/FIX?page=1&apiKey=wBugpaXUjbXgzQ2BZvrturHTrTfEzLAp&name=" + fixName).Result;
}

public class Navaids
{
    public Navaid[] items { get; set; } = Array.Empty<Navaid>();
    public int totalItems { get; set; }
    public int perPage { get; set; }
    public int page { get; set; }
    public int pages { get; set; }
}

public class Navaid
{
    public int id { get; set; }
    public string name { get; set; } = "";
    public string icao { get; set; } = "";
    public string type { get; set; } = "";
    public float frequency { get; set; }
    public float longitude { get; set; }
    public float latitude { get; set; }
}
