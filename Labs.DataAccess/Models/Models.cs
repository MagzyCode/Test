namespace Labs.DataAccess.Models;

/// <summary>
/// Класс для отображения пунтов назначения в таблице UI
/// </summary>
public class Destinations
{
    public int Id { get; set; }
    public string DestinationName { get; set; }
}

/// <summary>
/// Класс для отображения типов самолётов в таблице UI
/// </summary>
public class AircraftTypes 
{
    public int Id { get; set; }
    public string AircraftTypeName { get; set; }
}

/// <summary>
/// Класс для отображения рейсов в таблице UI
/// </summary>
public class Routes
{
    public int Id { get; set; }
    public string RouteNumber { get; set; }
    public string DepartureDestination { get; set; }
    public string ArrivalDestination { get; set; }
}

/// <summary>
/// Класс для отображения полётов в таблице UI
/// </summary>
public class Flights
{
    public int Id { get; set; }
    public string RouteNumber { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string AircraftType { get; set; }
}
