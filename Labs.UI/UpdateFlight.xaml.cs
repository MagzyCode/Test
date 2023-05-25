using Labs.DataAccess.Models;
using Labs.DataAccess.Repositories;
using System.Linq;
using System.Windows;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for UpdateFlight.xaml
    /// </summary>
    public partial class UpdateFlight : Window
    {
        //private FlightRepository _flightRepository = new();
        //private AircraftTypeRepository _aircraftTypeRepository = new();
        //private RouteRepository _routeRepository = new();
        private Flights _flights; 

        public UpdateFlight(Flights flights)
        {
            _flights = flights;
            InitializeComponent();

            var routes = RepositoryContainer.RouteRepository.GetAll()
                .Select(x => x.RouteNumber)
                .ToList();
            var selectedRouteIndex = routes.IndexOf(flights.RouteNumber);
            RoutesNumbersList.ItemsSource = routes;
            RoutesNumbersList.SelectedIndex = selectedRouteIndex;

            var planeTypes = RepositoryContainer.AircraftTypeRepository.GetAll()
                .Select(x => x.AircraftTypeName)
                .ToList();
            var selectedTypeIndex = planeTypes.IndexOf(flights.AircraftType);
            AircraftTypeList.ItemsSource = planeTypes;
            AircraftTypeList.SelectedIndex = selectedTypeIndex;

            DepartureDatePicker.SelectedDate = flights.DepartureDate;
            ArrivalDatePicker.SelectedDate = flights.ArrivalDate;
        }

        private void UpdateFlightClick(object sender, RoutedEventArgs e)
        {
            if (!DepartureDatePicker.SelectedDate.HasValue
                || !ArrivalDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Arrival and departure dates cannot be null.");
            }
            else if (DepartureDatePicker.SelectedDate.Value > ArrivalDatePicker.SelectedDate.Value)
            {
                MessageBox.Show("Departure date cannot be more than arrival date.");
            }
            else
            {
                var updatedFlight = new Flights()
                {
                    Id = _flights.Id,
                    AircraftType = AircraftTypeList.SelectedItem.ToString(),
                    DepartureDate = DepartureDatePicker.SelectedDate.Value,
                    ArrivalDate = ArrivalDatePicker.SelectedDate.Value,
                    RouteNumber = RoutesNumbersList.SelectedItem.ToString()
                };

                var result = RepositoryContainer.FlightRepository.Update(updatedFlight);

                if (!result.updated)
                {
                    MessageBox.Show(result.errorMessage);
                }
                else
                {
                    Close();
                }
            }
        }
    }
}
