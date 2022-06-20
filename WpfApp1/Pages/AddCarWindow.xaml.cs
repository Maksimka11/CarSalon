using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {
        public AddCarWindow()
        {
            InitializeComponent();
            LoadMarks();
        }

        Cars car = null;

        public AddCarWindow(int id)
        {
            InitializeComponent();
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                car = db.Cars.FirstOrDefault(x => x.Car_Id == id);
            }
            LoadMarks();
            LoadData();
        }

        string imgName = "";
        string imgPath = "";

        private void LoadMarks()
        {
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                db.Marks.Load();
                cbMark.ItemsSource = db.Marks.Local;

            }
        }

        private void LoadData()
        {
            TbModel.Text = car.Model;
            TbYear.Text = car.ProductionYear.ToString();
            cbMark.SelectedValue = car.Mark;
        }


        private void BtnOK_Click(object sender, RoutedEventArgs e)
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
            try
            {
                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\" + imgName;
                    File.Copy(imgPath, dest);
                }
                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    car = new Cars()
                    {
                        Photo = imgName,
                        Model = TbModel.Text,
                        ProductionYear = Convert.ToInt32(TbYear.Text),
                        Mark = (int)cbMark.SelectedValue,
                    };
                    db.Cars.Add(car);
                    db.SaveChanges();
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void Update()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(imgName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"\Images\" + imgName;
                    File.Copy(imgPath, dest);
                }

                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    Cars car1 = db.Cars.FirstOrDefault(x => x.Car_Id == car.Car_Id);
                    db.Cars.Attach(car1);
                    if (!string.IsNullOrWhiteSpace(imgName)) car1.Photo = imgName;
                        car1.Model = TbModel.Text;
                        car1.ProductionYear = Convert.ToInt32(TbYear.Text);
                        car1.Mark = (int)cbMark.SelectedValue;
                    db.SaveChanges();
                }
                DialogResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }

        private void CheckPhotoName()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\" + imgName))
            {
                for (int i = 1; i < Int32.MaxValue; i++)
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Images\" + imgName))
                    {
                        imgName = $"({i})" + imgName;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void BtnLoadPhto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "All Files (*.*)|*.*|JPEG, JPG (*.jpeg),(*.jpg)|*.jpeg;*.jpg|PNG (*.png)|*.png",
                Title = "Выберите фотографию!"
            };
            if (fileDialog.ShowDialog() == true)
            {
                imgName = fileDialog.SafeFileName;
                imgPath = fileDialog.FileName;
                CarImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
                CheckPhotoName();
            }
        }

        private string CheckFields()
        {
            string message = "";
            
            if (cbMark.SelectedIndex == -1)
            {
                message += "Выберите марку автомобиля!" + Environment.NewLine;
            }
            
            if (String.IsNullOrWhiteSpace(TbModel.Text))
            {
                message += "Поле модель пустое!" + Environment.NewLine;
            }

            if (String.IsNullOrWhiteSpace(TbYear.Text))
            {
                message += "Поле год выпуска пустое!" + Environment.NewLine;
            }

            return message;

        }

        private void TbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
