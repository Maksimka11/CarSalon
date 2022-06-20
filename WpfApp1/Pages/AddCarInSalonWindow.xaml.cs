using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.DataBase;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddCarInSalonWindow.xaml
    /// </summary>
    public partial class AddCarInSalonWindow : Window
    {
        public AddCarInSalonWindow()
        {
            InitializeComponent();
            LoadCb();
        }

        CarsInSalon car = null;

        public AddCarInSalonWindow(long id)
        {
            InitializeComponent();
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                car = db.CarsInSalon.FirstOrDefault(x => x.CIS_Id == id);
            }
            LoadCb();
            LoadData();
        }

        private void LoadData()
        {
            CbCar.SelectedValue = car.Car;
            CbSalon.SelectedValue = car.Salon;
            TbPrice.Text = car.Price.ToString();
            IudCount.Value = car.Count;
        }

        private void LoadCb()
        {
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                db.Cars.Load();
                ICollectionView view = new CollectionView(db.Cars.Include(x => x.Marks).ToList());
                CbCar.ItemsSource = view;
                db.Salons.Load();
                CbSalon.ItemsSource = db.Salons.Local;
            }
        }

        private void TbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!(CheckFields() == ""))
            {
                MessageBox.Show(CheckFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (car == null) Add();
            else Update();
        }

        private void Add()
        {
            car = new CarsInSalon();
            using (SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                car.Car = (int)CbCar.SelectedValue;
                car.Salon = (int)CbSalon.SelectedValue;
                car.Price = Convert.ToInt32(TbPrice.Text);
                car.Count = (byte)IudCount.Value;
                db.CarsInSalon.Add(car);
                db.SaveChanges();
            };
            DialogResult = true;
        }

        private void Update()
        {

            using (SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                CarsInSalon car1 = db.CarsInSalon.FirstOrDefault(x => x.CIS_Id == car.CIS_Id);
                db.CarsInSalon.Attach(car1);
                car1.Car = (int)CbCar.SelectedValue;
                car1.Salon = (int)CbSalon.SelectedValue;
                car1.Price = Convert.ToInt32(TbPrice.Text);
                car1.Count = (byte)IudCount.Value;
                db.SaveChanges();
            };
            DialogResult = true;
        }

        private string CheckFields()
        {
            string message = "";
            if (CbCar.SelectedIndex == -1)
            {
                message += "Выберите автомобиль!" + Environment.NewLine;
            }

            if (CbSalon.SelectedIndex == -1)
            {
                message += "Выберите салон!" + Environment.NewLine;

            }
            if (String.IsNullOrWhiteSpace(TbPrice.Text))
            {
                message += "Введите стоимость автомобиля!" + Environment.NewLine;
            }
            if (IudCount.Value == null)
            {
                message += "Введите количество автомобилей!" + Environment.NewLine;
            }

            return message;

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
