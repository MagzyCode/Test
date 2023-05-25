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
    /// Interaction logic for CreareAircraftType.xaml
    /// </summary>
    public partial class CreateAircraftType : Window
    {
        //private AircraftTypeRepository _aircraftTypeRepository = new();

        public CreateAircraftType()
        {
            InitializeComponent();
        }

        private void CreateAircraftTypeClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AircraftTypeBox.Text))
            {
                MessageBox.Show("Cannot create aircraft type with null or empty name.");
                return;
            }
            else
            {
                var types = RepositoryContainer.AircraftTypeRepository.GetAll()
                    .Select(x => x.AircraftTypeName)
                    .ToList();

                if (types.Contains(AircraftTypeBox.Text))
                {
                    MessageBox.Show("Aircraft type with such name already exists, enter another name.");
                    return;
                }
            }

            var creationType = new AircraftTypes()
            {
                AircraftTypeName = AircraftTypeBox.Text
            };

            var result = RepositoryContainer.AircraftTypeRepository.Create(creationType);

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
