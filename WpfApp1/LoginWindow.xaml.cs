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
using WpfApp1.DataBase;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }

        static public string Role;
        static public string FIO;

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            if (String.IsNullOrWhiteSpace(TbUser.Text)) message += "Введите логин!" + Environment.NewLine;
            if (String.IsNullOrEmpty(TbPass.Password)) message += "Введите пароль!" + Environment.NewLine;

            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                Users user = db.Users.FirstOrDefault(d => d.Login == TbUser.Text);
                if(user == null)
                {
                    MessageBox.Show("Логин указан неверно!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    if(user.Password == TbPass.Password)
                    {
                        FIO = user.Surname + " " + user.Name+ " " + user.Patronymic;
                        MainWindow main = new MainWindow();
                        this.Hide();
                        main.Owner = this;
                        main.Show();
                    }
                    else
                    {
                        MessageBox.Show("Не верный пароль!", "Ошибка", MessageBoxButton.OK);
                        return;
                    }
                }

                
            }
            


        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            var f = new RegWindow();
            f.ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
