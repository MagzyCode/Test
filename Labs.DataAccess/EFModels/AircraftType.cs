using System;
using System.Collections.Generic;

namespace Labs.DataAccess.EFModels;

/// <summary>
/// Представляет таблицу AircraftType в БД
/// </summary>
public partial class AircraftType
{
    public int Id { get; set; }

    public string AircraftTypeName { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
