namespace VacancyProAPI.Models.ViewModels;


public class Location
{
    public string Name { get; set; }

    public Location(string name)
    {
        Name = name;
    }
}

public class Current
{
    public int Temperature { get; set; }
    public string[] WeatherIcons { get; set; }
    public string[] WeatherDescriptions { get; set; }
    public int WindSpeed { get; set; }
    public int Presure { get; set; }

    public Current(int temperature, string[] weatherIcons, string[] weatherDescriptions)
    {
        Temperature = temperature;
        WeatherIcons = weatherIcons;
        WeatherDescriptions = weatherDescriptions;
    }
}


public class MeteoViewModel
{
    public Location Location { get; set; }
    public Current Current { get; set; }


    public MeteoViewModel(Location location, Current current)
    {
        Location = location;
        Current = current;
    }
}

