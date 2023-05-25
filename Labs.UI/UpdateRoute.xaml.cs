using Labs.DataAccess.Models;
using Labs.DataAccess.Repositories;
using System.Linq;
using System.Windows;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for UpdateRoute.xaml
    /// </summary>
    public partial class UpdateRoute : Window
    {
        //private RouteRepository _routeRepository = new();
        //private DestinationRepository _destinationRepository = new();
        private Routes _routes; 

        public UpdateRoute(Routes routes)
        {
            _routes = routes;
            InitializeComponent();
            
            RouteNumberBox.Text = _routes.RouteNumber;
            
            var destinations = RepositoryContainer.DestinationRepository.GetAll()
                .Select(x => x.DestinationName)
                .ToList();
            var selectedDepartureDestinationIndex = destinations.IndexOf(_routes.DepartureDestination);
            var selectedArrivalDestinationIndex = destinations.IndexOf(_routes.ArrivalDestination);
            ArrivalDestinationList.ItemsSource = destinations;
            ArrivalDestinationList.SelectedIndex = selectedArrivalDestinationIndex;
            DepartureDestinationList.ItemsSource = destinations;
            DepartureDestinationList.SelectedIndex = selectedDepartureDestinationIndex;
        }

        private void UpdateRouteClick(object sender, RoutedEventArgs e)
        {
            var routes = RepositoryContainer.RouteRepository.GetAll()
                .Select(x => x.RouteNumber)
                .ToList();

            if (string.IsNullOrWhiteSpace(RouteNumberBox.Text))
            {
                MessageBox.Show("Route number cannot be null or empty.");
            }
            else if (routes.Contains(RouteNumberBox.Text) && RouteNumberBox.Text != _routes.RouteNumber)
            {
                MessageBox.Show("Cannot change route number to new value, because it's already used by another route.");
            }
            else if (DepartureDestinationList.SelectedItem.ToString() == ArrivalDestinationList.SelectedItem.ToString())
            {
                MessageBox.Show("Arrival cannot be the same as depature");
            }
            else
            {
                var updatedRoute = new Routes
                {
                    Id = _routes.Id,
                    RouteNumber = RouteNumberBox.Text,
                    DepartureDestination = DepartureDestinationList.SelectedItem.ToString(),
                    ArrivalDestination = ArrivalDestinationList.SelectedItem.ToString()
                };

                var result = RepositoryContainer.RouteRepository.Update(updatedRoute);

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
