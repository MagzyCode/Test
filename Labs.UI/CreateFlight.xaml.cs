using Labs.DataAccess.Models;
using Labs.DataAccess.Repositories;
using System.Linq;
using System.Windows;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for CreateFlight.xaml
    /// </summary>
    public partial class CreateFlight : Window
    {
        //private RouteRepository _routeRepository = new();
        //private AircraftTypeRepository _aircraftTypeRepository = new();
        //private FlightRepository _flightRepository = new();

        public CreateFlight()
        {
            InitializeComponent();

            RoutesNumbersList.ItemsSource = RepositoryContainer.RouteRepository.GetAll()
                .Select(x => x.RouteNumber)
                .ToList();
            AircraftTypeList.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll()
                .Select(x => x.AircraftTypeName)
                .ToList();
        }

        private void CreateFlightClick(object sender, RoutedEventArgs e)
        {
            if (RoutesNumbersList.SelectedIndex == -1
                || AircraftTypeList.SelectedIndex == -1
                || DepartureDatePicker.SelectedDate == null
                || ArrivalDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Check that all fields are filled");
            }
            else if (!DepartureDatePicker.SelectedDate.HasValue
                || !ArrivalDatePicker.SelectedDate.HasValue
                || DepartureDatePicker.SelectedDate > ArrivalDatePicker.SelectedDate)
            {
                MessageBox.Show("Departure date cannot be less then arrival date");
            }
            else
            {
                var creationFlight = new Flights()
                {
                    DepartureDate = DepartureDatePicker.SelectedDate.Value,
                    ArrivalDate = ArrivalDatePicker.SelectedDate.Value,
                    AircraftType = AircraftTypeList.SelectedItem.ToString(),
                    RouteNumber = RoutesNumbersList.SelectedItem.ToString()
                };

                var result = RepositoryContainer.FlightRepository.Create(creationFlight);

                if (!result.created)
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
