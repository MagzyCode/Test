namespace Labs.DataAccess.EFModels;

/// <summary>
/// Представляет таблицу Route в БД
/// </summary>
public partial class Route
{
    public int Id { get; set; }

    public string RouteNumber { get; set; }

    public int? DepartureDestinationId { get; set; }

    public int? ArrivalDestinationId { get; set; }

    public virtual Destination ArrivalDestination { get; set; }

    public virtual Destination DepartureDestination { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
