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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class SalonsPage : Page
    {
        SalonDirectoryEntities db;

        public SalonsPage()
        {
            InitializeComponent();
            LoadData();
            
        }

        private void LoadData()
        {
            db = new SalonDirectoryEntities();
            db.Salons.Load();
            DgSalons.ItemsSource = db.Salons.Local;
        }


        private void BtnAddSalon_Click(object sender, RoutedEventArgs e)
        {
            AddSalonWindow addSalon = new AddSalonWindow();

            if(addSalon.ShowDialog() == true)
            {
                MessageBox.Show("Запись добавлена!");
                LoadData();
            }
        }

        private void BtnDelSalon_Click(object sender, RoutedEventArgs e)
        {
            if (DgSalons.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgSalons.SelectedItems.Count; i++)
                        {
                            Salons note = DgSalons.SelectedItems[i] as Salons;
                            Salons note1 = db.Salons.FirstOrDefault(x => x.Salon_Id == note.Salon_Id);
                            db.Salons.Remove(note1);
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgSalons.SelectedItem == null) return;

            Salons salon = DgSalons.SelectedItem as Salons;

            AddSalonWindow window = new AddSalonWindow(salon.Salon_Id);

            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись обновлена!");
                LoadData();
            }
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\Salons" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Автосалоны";
                int row = 2;
                int i = 0;

                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Id";
                xlSheet.Cells[1, 3] = "Фото";
                xlSheet.Cells[1, 4] = "Название";
                xlSheet.Cells[1, 5] = "Адрес";
                xlSheet.Cells[1, 6] = "Владелец";
                xlSheet.Cells[1, 7] = "Телефон";





                if (DgSalons.Items.Count > 0)
                {
                    for (i = 0; i < DgSalons.Items.Count; i++)
                    {

                        Salons salon = DgSalons.Items[i] as Salons;

                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = salon.Salon_Id;
                        xlSheet.Cells[row, 3] = salon.Photo;
                        xlSheet.Cells[row, 4] = salon.Name;
                        xlSheet.Cells[row, 5] = salon.Address;
                        xlSheet.Cells[row, 6] = salon.Owner;
                        xlSheet.Cells[row, 7] = salon.Phone;

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
