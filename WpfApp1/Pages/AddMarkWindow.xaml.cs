using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp1.DataBase;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddMarkWindow.xaml
    /// </summary>
    public partial class AddMarkWindow : Window
    {
        public AddMarkWindow()
        {
            InitializeComponent();
        }

        Marks mark = null;

        public AddMarkWindow(int id)
        {
            InitializeComponent();
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                mark = db.Marks.FirstOrDefault(x => x.Mark_id == id);
            }
            LoadData();
        }


        private void LoadData()
        {
            if (!string.IsNullOrWhiteSpace(mark.Logo))
            {
                MarkLogo.Source = new BitmapImage(new Uri(mark.GetLogo));
            }
            TbName.Text = mark.Name;
            TbOwner.Text = mark.Owner;
            DpDate.SelectedDate = mark.FoundationDate;
        }

        string imgName = "";
        string imgPath = "";

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
                MarkLogo.Source = new BitmapImage(new Uri(fileDialog.FileName));
                CheckPhotoName();
            }
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

        private string CheckData()
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(TbName.Text)) message += "Введите название!" + Environment.NewLine;
            if (DpDate.SelectedDate == null) message += "Введите дату основания!" + Environment.NewLine;
            if (string.IsNullOrWhiteSpace(TbOwner.Text)) message += "Введите владельца компании!" + Environment.NewLine;
            return message;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CheckData()))
            {
                MessageBox.Show(CheckData(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (mark == null) Add();
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
                    mark = new Marks()
                    {
                        Logo = imgName,
                        Name = TbName.Text,
                        Owner = TbOwner.Text,
                        FoundationDate = DpDate.SelectedDate,
                    };
                    db.Marks.Add(mark);
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
                    Marks mark1 = db.Marks.FirstOrDefault(x => x.Mark_id == mark.Mark_id);
                    db.Marks.Attach(mark1);
                    if (!string.IsNullOrWhiteSpace(imgName)) mark1.Logo = imgName;
                    mark1.FoundationDate = DpDate.SelectedDate;
                    mark1.Name = TbName.Text;
                    mark1.Owner = TbOwner.Text;
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
    }
}
