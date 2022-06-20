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
    /// Логика взаимодействия для CarsInSalon.xaml
    /// </summary>
    public partial class CarsInSalonPage : Page
    {
        public CarsInSalonPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                db.CarsInSalon.Load();
                ICollectionView view = new CollectionView(db.CarsInSalon.Include(x => x.Cars.Marks).Include(x => x.Salons).ToList());
                DgCarsInSalon.ItemsSource = view;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCarInSalonWindow window = new AddCarInSalonWindow();
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись добавлена");
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgCarsInSalon.SelectedItem == null) return;
            CarsInSalon car = DgCarsInSalon.SelectedItem as CarsInSalon;
            AddCarInSalonWindow window = new AddCarInSalonWindow(car.CIS_Id);
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись обновлена");
                LoadData();
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgCarsInSalon.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgCarsInSalon.SelectedItems.Count; i++)
                        {
                            CarsInSalon note = DgCarsInSalon.SelectedItems[i] as CarsInSalon;
                            CarsInSalon note1 = db.CarsInSalon.FirstOrDefault(x => x.CIS_Id == note.CIS_Id);
                            db.CarsInSalon.Remove(note1);
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

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\CarsInSalon" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Машины в салонах";
                int row = 2;
                int i = 0;

                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Id";
                xlSheet.Cells[1, 3] = "Машина";
                xlSheet.Cells[1, 4] = "Салон";
                xlSheet.Cells[1, 5] = "Стоимость";
                xlSheet.Cells[1, 6] = "Количество";





                if (DgCarsInSalon.Items.Count > 0)
                {
                    for (i = 0; i < DgCarsInSalon.Items.Count; i++)
                    {

                        CarsInSalon car = DgCarsInSalon.Items[i] as CarsInSalon;

                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = car.CIS_Id;
                        xlSheet.Cells[row, 3] = car.CarName;
                        xlSheet.Cells[row, 4] = car.Salons.Name;
                        xlSheet.Cells[row, 5] = car.Price;
                        xlSheet.Cells[row, 6] = car.Count;
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
