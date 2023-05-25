using Labs.DataAccess.Models;
using Labs.DataAccess.Repositories;
using System.Linq;
using System.Windows;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for UpdateAircraftType.xaml
    /// </summary>
    public partial class UpdateAircraftType : Window
    {
        //private AircraftTypeRepository _aircraftTypeRepository = new();
        private AircraftTypes _aircraftTypes;

        public UpdateAircraftType(AircraftTypes aircraftTypes)
        {
            _aircraftTypes = aircraftTypes;
            InitializeComponent();

            AircraftTypeBox.Text = _aircraftTypes.AircraftTypeName;
        }

        private void UpdateAircraftTypeClick(object sender, RoutedEventArgs e)
        {
            var types = RepositoryContainer.AircraftTypeRepository.GetAll()
                .Select(x => x.AircraftTypeName)
                .ToList();

            if (string.IsNullOrWhiteSpace(AircraftTypeBox.Text))
            {
                MessageBox.Show("Type cannot be null or empty.");
            }
            else if (types.Contains(AircraftTypeBox.Text) && AircraftTypeBox.Text != _aircraftTypes.AircraftTypeName)
            {
                MessageBox.Show("Choose another type name, because given one used by another type.");
            }
            else
            {
                var updatedType = new AircraftTypes()
                {
                    Id = _aircraftTypes.Id,
                    AircraftTypeName = AircraftTypeBox.Text
                };

                var result = RepositoryContainer.AircraftTypeRepository.Update(updatedType);

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
