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
    /// Interaction logic for UpdateDestination.xaml
    /// </summary>
    public partial class UpdateDestination : Window
    {
        // private DestinationRepository _destinationRepository = new();
        private Destinations _destinations;

        public UpdateDestination(Destinations destinations)
        {
            _destinations = destinations;
            InitializeComponent();

            DestinationBox.Text = _destinations.DestinationName;
        }

        private void UpdateDestinationClick(object sender, RoutedEventArgs e)
        {
            var destinations = RepositoryContainer.DestinationRepository.GetAll()
                .Select(x => x.DestinationName)
                .ToList();

            if (string.IsNullOrWhiteSpace(DestinationBox.Text))
            {
                MessageBox.Show("Destination name cannot be null or empty.");
            }
            else if (destinations.Contains(DestinationBox.Text) && DestinationBox.Text != _destinations.DestinationName)
            {
                MessageBox.Show("Entered destination name cannot be used, because another destination already use it.");
            }
            else
            {
                var updatedDestination = new Destinations()
                {
                    Id = _destinations.Id,
                    DestinationName = DestinationBox.Text
                };

                var result = RepositoryContainer.DestinationRepository.Update(updatedDestination);

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
