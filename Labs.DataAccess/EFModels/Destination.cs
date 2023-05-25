namespace Labs.DataAccess.EFModels;

/// <summary>
/// Представляет таблицу Destination в БД
/// </summary>
public partial class Destination
{
    public int Id { get; set; }

    public string DestinationName { get; set; }

    public virtual ICollection<Route> RouteArrivalDestinations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteDepartureDestinations { get; set; } = new List<Route>();
}
