using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MarksPage.xaml
    /// </summary>
    public partial class MarksPage : Page
    {
        public MarksPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                db.Marks.Load();
                DgMarks.ItemsSource = db.Marks.Local;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddMarkWindow window = new AddMarkWindow();
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись добавлена");
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgMarks.SelectedItem == null) return;
            Marks mark = DgMarks.SelectedItem as Marks;
            AddMarkWindow window = new AddMarkWindow(mark.Mark_id);
            if (window.ShowDialog() == true)
            {
                MessageBox.Show("Запись обновлена");
                LoadData();
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgMarks.SelectedItems.Count < 1) return;
            else if (MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    try
                    {
                        for (int i = 0; i < DgMarks.SelectedItems.Count; i++)
                        {
                            Marks note = DgMarks.SelectedItems[i] as Marks;
                            Marks note1 = db.Marks.FirstOrDefault(x => x.Mark_id == note.Mark_id);
                            db.Marks.Remove(note1);
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
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + @"\Отчеты\Marks" + ".xls";
            if (!File.Exists(fileName)) File.Create(fileName);
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                xlApp.Workbooks.Open(fileName);
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                xlSheet.Name = "Марки";
                int row = 2;
                int i = 0;

                xlSheet.Cells[1, 1] = "№";
                xlSheet.Cells[1, 2] = "Id";
                xlSheet.Cells[1, 3] = "Логотип";
                xlSheet.Cells[1, 4] = "Название";
                xlSheet.Cells[1, 5] = "Дата основания";
                xlSheet.Cells[1, 6] = "Владелец";





                if (DgMarks.Items.Count > 0)
                {
                    for (i = 0; i < DgMarks.Items.Count; i++)
                    {

                        Marks mark = DgMarks.Items[i] as Marks;

                        xlSheet.Cells[row, 1] = i + 1;
                        xlSheet.Cells[row, 2] = mark.Mark_id;
                        xlSheet.Cells[row, 3] = mark.Logo;
                        xlSheet.Cells[row, 4] = mark.Name;
                        xlSheet.Cells[row, 5] = mark.FoundationDate;
                        xlSheet.Cells[row, 6] = mark.Owner;
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
