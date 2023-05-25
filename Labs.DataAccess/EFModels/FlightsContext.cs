using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Labs.DataAccess.EFModels;

/// <summary>
/// Представляет базу данных Flights
/// </summary>
public partial class FlightsContext : DbContext
{
    public FlightsContext()
    {
    }

    public FlightsContext(DbContextOptions<FlightsContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Представляет таблицу типов самолётов
    /// </summary>
    public virtual DbSet<AircraftType> AircraftTypes { get; set; }

    /// <summary>
    /// Представляет таблицу пунктов назначения
    /// </summary>
    public virtual DbSet<Destination> Destinations { get; set; }

    /// <summary>
    /// Представляет таблицу полётов
    /// </summary>
    public virtual DbSet<Flight> Flights { get; set; }

    /// <summary>
    /// Представляет таблицу рейсов
    /// </summary>
    public virtual DbSet<Route> Routes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Flights;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    /// <summary>
    /// Задаёт связи между классами моделей как у таблиц в БД
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AircraftType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aircraft__3214EC07798A01FC");

            entity.Property(e => e.AircraftTypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Destinat__3214EC079B0E2177");

            entity.Property(e => e.DestinationName).HasMaxLength(255);
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flights__3214EC071C1C4780");

            entity.Property(e => e.ArrivalDate).HasColumnType("date");
            entity.Property(e => e.DepartureDate).HasColumnType("date");

            entity.HasOne(d => d.AircraftType).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AircraftTypeId)
                .HasConstraintName("FK__Flights__Aircraf__2D27B809");

            entity.HasOne(d => d.Route).WithMany(p => p.Flights)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Flights__RouteId__2C3393D0");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Routes__3214EC07234CB29C");

            entity.Property(e => e.RouteNumber).HasMaxLength(255);

            entity.HasOne(d => d.ArrivalDestination).WithMany(p => p.RouteArrivalDestinations)
                .HasForeignKey(d => d.ArrivalDestinationId)
                .HasConstraintName("FK__Routes__ArrivalD__29572725");

            entity.HasOne(d => d.DepartureDestination).WithMany(p => p.RouteDepartureDestinations)
                .HasForeignKey(d => d.DepartureDestinationId)
                .HasConstraintName("FK__Routes__Departur__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
