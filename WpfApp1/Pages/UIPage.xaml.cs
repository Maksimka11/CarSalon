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
    /// Логика взаимодействия для UIPage.xaml
    /// </summary>
    public partial class UIPage : Page
    {
        public UIPage()
        {
            InitializeComponent();
        }

        private void itemCars_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CarsPage());
        }

        private void itemHome_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new StartPage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.Owner.Close();
        }

        private void itemSalons_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SalonsPage());
        }

        private void itemCompany_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new MarksPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new StartPage());
        }

        private void itemLogout_Selected(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
            mainWindow.Close();
            mainWindow.Owner.Show();
        }

        private void itemCarsInSalon_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CarsInSalonPage());
        }
    }
}
