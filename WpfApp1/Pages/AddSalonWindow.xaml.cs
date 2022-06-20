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
using System.Windows.Shapes;
using System.IO;
using WpfApp1.DataBase;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddSalonWindow.xaml
    /// </summary>
    public partial class AddSalonWindow : Window
    {
        public AddSalonWindow()
        {
            InitializeComponent();
        }

        Salons salon = null;

        public AddSalonWindow(int id)
        {
            InitializeComponent();
            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                salon = db.Salons.FirstOrDefault(x => x.Salon_Id == id);
            }
            LoadData();
        }

        private void LoadData()
        {
            if (!string.IsNullOrWhiteSpace(salon.Photo))
            {
                try
                {
                    SalonImage.Source = new BitmapImage(new Uri(salon.Photo));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            TbName.Text = salon.Name;
            TbOwner.Text = salon.Owner;
            TbAddress.Text = salon.Address;
            TbPhone.Text = salon.Phone;
        }

        string PhotoName;
        string Photo = "";

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!(CheckFields() == ""))
            {
                MessageBox.Show(CheckFields(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (salon == null) Add();
            else Update();
            
        }


        private void Add()
        {
            salon = new Salons();
            using (SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                salon.Name = TbName.Text;
                salon.Owner = TbOwner.Text;
                salon.Address = TbAddress.Text;
                salon.Phone = TbPhone.Text;
                string dest = AppDomain.CurrentDomain.BaseDirectory + @"Images\" + PhotoName;
                salon.Photo = dest;
                db.Salons.Add(salon);

                string filepath = Photo;
                if (Photo != "") File.Copy(filepath, dest);
                db.SaveChanges();
            };
            DialogResult = true;
        }

        private void Update()
        {
            
            using (SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                Salons salon1 = db.Salons.FirstOrDefault(x=>x.Salon_Id==salon.Salon_Id);
                db.Salons.Attach(salon1);
                salon1.Name = TbName.Text;
                salon1.Owner = TbOwner.Text;
                salon1.Address = TbAddress.Text;
                salon1.Phone = TbPhone.Text;
                if (!string.IsNullOrWhiteSpace(PhotoName))
                {
                    string dest = AppDomain.CurrentDomain.BaseDirectory + @"Images\" + PhotoName;
                    salon1.Photo = dest;
                    string filepath = Photo;
                    if (Photo != "") File.Copy(filepath, dest);
                }
                db.SaveChanges();
            };
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnLoadPhto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFile.Title = "Выберите фотографию";
            if(openFile.ShowDialog() == true)
            {
                string path = openFile.SafeFileName;
                SalonImage.Source = new BitmapImage(new Uri(openFile.FileName, UriKind.RelativeOrAbsolute));
                PhotoName = ChangePhotoName(path);
                Photo = openFile.FileName;
            }
        }

        string ChangePhotoName(string Name)
        {
            string x = AppDomain.CurrentDomain.BaseDirectory + "Images/" + Name;
            string photoname = Name;
            
            int i = 0;
            if (System.IO.File.Exists(x))
            {
                while (System.IO.File.Exists(x))
                {
                    i++;
                    x = AppDomain.CurrentDomain.BaseDirectory + "Images/" + i.ToString() + photoname;
                }
                photoname = i.ToString() + photoname;
            }
            return photoname;
            

        }

        private string CheckFields()
        {
            string message = "";
            if (String.IsNullOrWhiteSpace(TbName.Text))
            {
                message += "Поле название пустое!" + Environment.NewLine;
            }
            
            if (String.IsNullOrWhiteSpace(TbOwner.Text))
            {
                message += "Поле владельца пустое!" + Environment.NewLine;
            }
            if (String.IsNullOrWhiteSpace(TbAddress.Text))
            {
                message += "Поле адреса пустое!" + Environment.NewLine;
            }
            if (String.IsNullOrWhiteSpace(TbPhone.Text))
            {
                message += "Поле телефона пустое!" + Environment.NewLine;
            }

           return message;
           
        }

        private void TbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
