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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labs.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // На основе выпадающего списка выбираем репозитории для какой лабораторной мы хотем выполнить (чистый ADO или ADO + LINQ)
            switch (LabNumberSelector.Text)
            {
                case "ADO.NET":
                    {
                        RepositoryContainer.Selector = DataAccess.Enums.Selector.Lab67;
                        break;
                    }
                case "LINQ":
                    {
                        RepositoryContainer.Selector = DataAccess.Enums.Selector.Lab8;
                        break;
                    }
            }

            // Первая таблица, которая отображается нам - таблицы полётов, поэтому заполняем данные для этой таблицы из БД
            FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll();
        }

        private void TableName_DropDownClosed(object sender, EventArgs e)
        {
            // На основе выбранной таблицы из выпадающего списка скрываем все таблицы кроме одной и эту таблицу заполняем данными
            // и обновляем текст кнопки для фильтрации
            switch (TableName.Text)
            {
                case "Flights":
                    {
                        FlightsTable.Visibility = Visibility.Visible;
                        RoutesTable.Visibility = Visibility.Hidden;
                        AircraftTypesTable.Visibility = Visibility.Hidden;
                        DestinationsTable.Visibility = Visibility.Hidden;

                        FilterButton.Content = "Фильтровать по дате отправления";

                        FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll();

                        break;
                    }
                case "Routes":
                    {
                        FlightsTable.Visibility = Visibility.Hidden;
                        RoutesTable.Visibility = Visibility.Visible;
                        AircraftTypesTable.Visibility = Visibility.Hidden;
                        DestinationsTable.Visibility = Visibility.Hidden;

                        FilterButton.Content = "Фильтровать по номеру рейса";

                        RoutesTable.ItemsSource = RepositoryContainer.RouteRepository.GetAll();
                        break;
                    }
                case "AircraftTypes":
                    {
                        FlightsTable.Visibility = Visibility.Hidden;
                        RoutesTable.Visibility = Visibility.Hidden;
                        AircraftTypesTable.Visibility = Visibility.Visible;
                        DestinationsTable.Visibility = Visibility.Hidden;

                        FilterButton.Content = "Фильтровать по типу самолёта";

                        AircraftTypesTable.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll();
                        break;
                    }
                case "Destinations":
                    {
                        FlightsTable.Visibility = Visibility.Hidden;
                        RoutesTable.Visibility = Visibility.Hidden;
                        AircraftTypesTable.Visibility = Visibility.Hidden;
                        DestinationsTable.Visibility = Visibility.Visible;

                        FilterButton.Visibility = Visibility.Visible;
                        FilterButton.Content = "Фильтровать по пунте назначения";

                        DestinationsTable.ItemsSource = RepositoryContainer.DestinationRepository.GetAll();
                        break;
                    }
            }
        }

        private void CreateTableItemClick(object sender, RoutedEventArgs e)
        {
            // В зависимости таблицы, в которую хотим добавить запись, открывается окно для добавления записи
            switch (TableName.Text)
            {
                case "Flights":
                    {
                        var window = new CreateFlight();
                        window.ShowDialog();
                        FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll();
                        break;
                    }
                case "Routes":
                    {
                        var window = new CreateRoute();
                        window.ShowDialog();
                        RoutesTable.ItemsSource = RepositoryContainer.RouteRepository.GetAll();
                        break;
                    }
                case "AircraftTypes":
                    {
                        var window = new CreateAircraftType();
                        window.ShowDialog();
                        AircraftTypesTable.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll();
                        break;
                    }
                case "Destinations":
                    {
                        var window = new CreateDistination();
                        window.ShowDialog();
                        DestinationsTable.ItemsSource = RepositoryContainer.DestinationRepository.GetAll();
                        break;
                    }
            }
        }

        private void UpdateTableItem(object sender, RoutedEventArgs e)
        {
            // В зависимости таблицы, в которую хотим обновить, открывается окно для обновления записи
            switch (TableName.Text)
            {
                case "Flights":
                    {
                        if (FlightsTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = FlightsTable.SelectedItem as Flights;

                        if (selected == null) 
                        {
                            MessageBox.Show("No flight selected. Select one.");
                            break;
                        }

                        var window = new UpdateFlight(selected);
                        window.ShowDialog();
                        FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll();
                        break;
                    }
                case "Routes":
                    {
                        if (RoutesTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = RoutesTable.SelectedItem as Routes;

                        if (selected == null)
                        {
                            MessageBox.Show("No route selected. Select one.");
                            break;
                        }

                        var window = new UpdateRoute(selected);
                        window.ShowDialog();
                        RoutesTable.ItemsSource = RepositoryContainer.RouteRepository.GetAll();
                        break;
                    }
                case "AircraftTypes":
                    {
                        if (AircraftTypesTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = AircraftTypesTable.SelectedItem as AircraftTypes;

                        if (selected == null)
                        {
                            MessageBox.Show("No type selected. Select one.");
                            break;
                        }

                        var window = new UpdateAircraftType(selected);
                        window.ShowDialog();
                        AircraftTypesTable.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll();
                        break;
                    }
                case "Destinations":
                    {
                        if (DestinationsTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = DestinationsTable.SelectedItem as Destinations;

                        if (selected == null)
                        {
                            MessageBox.Show("No type selected. Select one.");
                            break;
                        }

                        var window = new UpdateDestination(selected);
                        window.ShowDialog();
                        DestinationsTable.ItemsSource = RepositoryContainer.DestinationRepository.GetAll();
                        break;
                    }
            }
        }

        private void DeleteTableItemClick(object sender, RoutedEventArgs e)
        {
            switch (TableName.Text)
            {
                case "Flights":
                    {
                        if (FlightsTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = FlightsTable.SelectedItem as Flights;

                        if (selected == null)
                        {
                            MessageBox.Show("No flight selected. Select one.");
                            break;
                        }

                        var result = RepositoryContainer.FlightRepository.Delete(selected.Id);

                        if (!result.deleted)
                        {
                            MessageBox.Show(result.errorMessage);
                        }

                        FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll();
                        break;
                    }
                case "Routes":
                    {
                        if (RoutesTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = RoutesTable.SelectedItem as Routes;

                        if (selected == null)
                        {
                            MessageBox.Show("No route selected. Select one.");
                            break;
                        }

                        var result = RepositoryContainer.RouteRepository.Delete(selected.Id);

                        if (!result.deleted)
                        {
                            MessageBox.Show(result.errorMessage + "\nCheck that selected route not used by flights table.");
                        }

                        RoutesTable.ItemsSource = RepositoryContainer.RouteRepository.GetAll();
                        break;
                    }
                case "AircraftTypes":
                    {
                        if (AircraftTypesTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = AircraftTypesTable.SelectedItem as AircraftTypes;

                        if (selected == null)
                        {
                            MessageBox.Show("No type selected. Select one.");
                            break;
                        }

                        var result = RepositoryContainer.AircraftTypeRepository.Delete(selected.Id);

                        if (!result.deleted)
                        {
                            MessageBox.Show(result.errorMessage + "\nCheck that selected type not used by flights table.");
                        }

                        AircraftTypesTable.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll();
                        break;
                    }
                case "Destinations":
                    {
                        if (DestinationsTable.SelectedItems.Count > 1)
                        {
                            MessageBox.Show("Select only one row.");
                            break;
                        }

                        var selected = DestinationsTable.SelectedItem as Destinations;

                        if (selected == null)
                        {
                            MessageBox.Show("No type selected. Select one.");
                            break;
                        }

                        var result = RepositoryContainer.DestinationRepository.Delete(selected.Id);

                        if (!result.deleted)
                        {
                            MessageBox.Show(result.errorMessage + "\nCheck that selected destination not used by flights table.");
                        }

                        DestinationsTable.ItemsSource = RepositoryContainer.DestinationRepository.GetAll();
                        break;
                    }
            }
        }

        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {
            // Фильтруем данные
            switch (TableName.Text)
            {
                case "Flights":
                    {
                        FlightsTable.ItemsSource = RepositoryContainer.FlightRepository.GetAll()
                            .OrderBy(x => x.DepartureDate)
                            .ToList();

                        break;
                    }
                case "Destinations":
                    {
                        DestinationsTable.ItemsSource = RepositoryContainer.DestinationRepository.GetAll()
                            .OrderBy(x => x.DestinationName)
                            .ToList();

                        break;
                    }
                case "AircraftTypes":
                    {
                        AircraftTypesTable.ItemsSource = RepositoryContainer.AircraftTypeRepository.GetAll()
                            .OrderBy(x => x.AircraftTypeName)
                            .ToList();

                        break;
                    }
                case "Routes":
                    {
                        RoutesTable.ItemsSource = RepositoryContainer.RouteRepository.GetAll()
                            .OrderBy(x => x.RouteNumber)
                            .ToList();  

                        break;
                    }
            }
        }
    }
}
