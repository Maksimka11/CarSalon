using System;
using System.Linq;
using System.Windows;
using WpfApp1.DataBase;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

            string message = "";
            if (String.IsNullOrWhiteSpace(TbName.Text)) message += "Введите имя!" + Environment.NewLine;
            if(String.IsNullOrWhiteSpace(TbSurname.Text)) message += "Введите фамилию!" + Environment.NewLine;
            if (String.IsNullOrWhiteSpace(TbPatron.Text)) message += "Введите отчество!" + Environment.NewLine;
            if (String.IsNullOrWhiteSpace(TbLogin.Text)) message += "Введите логин!" + Environment.NewLine;
            if (String.IsNullOrEmpty(TbPass.Password)) message += "Введите пароль!" + Environment.NewLine;

            using(SalonDirectoryEntities db = new SalonDirectoryEntities())
            {
                Users user = db.Users.FirstOrDefault(d => d.Login == TbLogin.Text);

                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже зарегистрирован!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }

            if(message == "")
            {
                using(SalonDirectoryEntities db = new SalonDirectoryEntities())
                {
                    Users user = new Users
                    {
                        Name = TbName.Text,
                        Surname = TbSurname.Text,
                        Patronymic = TbPatron.Text,
                        Login = TbLogin.Text,
                        Password = TbPass.Password,
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                }

                DialogResult = true;
            }
            else
            {
                MessageBox.Show(message,"Ошибка",MessageBoxButton.OK);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
