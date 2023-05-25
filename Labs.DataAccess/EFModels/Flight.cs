using System;
using System.Collections.Generic;

namespace Labs.DataAccess.EFModels;

/// <summary>
/// Представляет таблицу Flight в БД
/// </summary>
public partial class Flight
{
    public int Id { get; set; }

    public int? RouteId { get; set; }

    public DateTime? DepartureDate { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public int? AircraftTypeId { get; set; }

    public virtual AircraftType AircraftType { get; set; }

    public virtual Route Route { get; set; }
}
