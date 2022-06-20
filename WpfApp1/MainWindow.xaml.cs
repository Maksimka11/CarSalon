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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Pages;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Frame xFrame
        {
            get;
            set;
        }
        public ListView listView
        {
            get;
            set;
        }

       static  public string FIO { get; set; }
        static public string Role { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            UIPage uIPage = new UIPage();
            MainFrame.NavigationService.Navigate(uIPage);
            xFrame = uIPage.ContentFrame;
            listView = uIPage.lstView;
        }


    }
}
