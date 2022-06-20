using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
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
using WpfApp1.DataBase;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для CarsPage.xaml
    /// </summary>
    public partial class CarsPage : Page
    {
        public CarsPage()
        {
            InitializeComponent();
            LoadData();
        }
        SalonDirectoryEntities db;
        private void BtnAddCar_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCar = new AddCarWindow();

            if (addCar.ShowDialog() == true)
            {
                MessageBox.Show("Запись добавлена!");
                LoadData();
            }
        }

        private void BtnDelCar_Click(object sender, RoutedEventArgs e)
        {
            if (DgCars.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgCars.SelectedItems.Count; i++)
                        {
                            Cars note = DgCars.SelectedItems[i] as Cars;
                            Cars note1 = db.Cars.FirstOrDefault(x => x.Car_Id == note.Car_Id);
                            db.Cars.Remove(note1);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Имеются связанные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        db.Dispose();
                    }
                    finally
                    {
                        LoadData();
                    }
                }
            }
        }

        private void LoadData()
        {
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                db.Cars.Load();
                ICollectionView view = new CollectionView(db.Cars.Include(x => x.Marks).ToList());
                DgCars.ItemsSource = view;
            }
            
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgCars.SelectedItem == null) return;

            Cars car = DgCars.SelectedItem as Cars;

            AddCarWindow window = new AddCarWindow(car.Car_Id);

            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись обновлена!");
                LoadData();
            }
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\Cars" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Автомобили";
                int row = 2;
                int i = 0;

                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Id";
                xlSheet.Cells[1, 3] = "Фото";
                xlSheet.Cells[1, 4] = "Марка";
                xlSheet.Cells[1, 5] = "Модель";
                xlSheet.Cells[1, 6] = "Год выпуска";





                if (DgCars.Items.Count > 0)
                {
                    for (i = 0; i < DgCars.Items.Count; i++)
                    {

                        Cars car = DgCars.Items[i] as Cars;

                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = car.Car_Id;
                        xlSheet.Cells[row, 3] = car.Photo;
                        xlSheet.Cells[row, 4] = car.Marks.Name;
                        xlSheet.Cells[row, 5] = car.Model;
                        xlSheet.Cells[row, 6] = car.ProductionYear;
                        row++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                xlApp.Visible = true;
                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;

            }
        }
    }
}
