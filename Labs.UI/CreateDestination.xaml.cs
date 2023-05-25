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
    /// Interaction logic for CreateDistination.xaml
    /// </summary>
    public partial class CreateDistination : Window
    {
        //private DestinationRepository _destinationRepository = new();

        public CreateDistination()
        {
            InitializeComponent();
        }

        private void CreateDestinationClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DestinationBox.Text))
            {
                MessageBox.Show("Destination name cannot be null or empty");
                return;
            }
            else
            {
                var isDestinationExists = RepositoryContainer.DestinationRepository.GetAll()
                    .Select(x => x.DestinationName)
                    .Contains(DestinationBox.Text);

                if (isDestinationExists )
                {
                    MessageBox.Show("Destination with given name already exists. Enter anothee name.");
                }
                else
                {
                    var destination = new Destinations()
                    {
                        DestinationName = DestinationBox.Text,
                    };

                    var result = RepositoryContainer.DestinationRepository.Create(destination);

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
}
