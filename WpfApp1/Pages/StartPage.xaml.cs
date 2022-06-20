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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            LblWelcome.Content += " " + (LoginWindow.FIO + "!");
        }

        private void BtnCars_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.listView.SelectedIndex = -1;
            mainWindow.xFrame.Navigate(new CarsPage());
        }

        private void BtnSalons_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.listView.SelectedIndex = -1;
            mainWindow.xFrame.Navigate(new SalonsPage());
        }

        private void BtnMarks_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.listView.SelectedIndex = -1;
            mainWindow.xFrame.Navigate(new MarksPage());
        }

        private void BtnCarsInSalon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.listView.SelectedIndex = -1;
            mainWindow.xFrame.Navigate(new CarsInSalonPage());
        }
    }
}
