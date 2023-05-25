using Labs.DataAccess.Models;
using Labs.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for CreateRoute.xaml
    /// </summary>
    public partial class CreateRoute : Window
    {
        //private DestinationRepository _destinationRepository = new();
        //private RouteRepository _routeRepository = new();

        public CreateRoute()
        {
            InitializeComponent();

            var destinations = RepositoryContainer.DestinationRepository.GetAll()
                .Select(x => x.DestinationName)
                .ToList();

            ArrivalDestinationList.ItemsSource = destinations;
            DepartureDestinationList.ItemsSource = destinations;
        }

        private void CreateRouteClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RouteNumberBox.Text))
            {
                MessageBox.Show("Route number cannot be null or empty");
                return;
            }
            else
            {
                var existingRoutesNames = RepositoryContainer.RouteRepository.GetAll()
                    .Select(x => x.RouteNumber)
                    .ToList();

                if (existingRoutesNames.Contains(RouteNumberBox.Text))
                {
                    MessageBox.Show("Route number with such number already exists. Change number");
                    return;
                }
            }
            
            if (ArrivalDestinationList.SelectedIndex == DepartureDestinationList.SelectedIndex
                || ArrivalDestinationList.SelectedIndex == -1
                || DepartureDestinationList.SelectedIndex == -1)
            {
                MessageBox.Show("Destination and arrival cannot be the same or empty");
            }
            else
            {
                var creationRoute = new Routes()
                {
                    RouteNumber = RouteNumberBox.Text,
                    ArrivalDestination = ArrivalDestinationList.SelectedItem.ToString(),
                    DepartureDestination = DepartureDestinationList.SelectedItem.ToString(),
                };

                var result = RepositoryContainer.RouteRepository.Create(creationRoute);

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
