using AsyncSocketLib;
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

namespace Milosevic_AsyncServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AsyncSocketServer mServer;

        public MainWindow()
        {
            InitializeComponent();

            mServer = new AsyncSocketServer();
        }

        private void btn_ascolto_Click(object sender, RoutedEventArgs e)
        {
            mServer.InAscolto();
        }

        private void btn_disconnettiti_Click(object sender, RoutedEventArgs e)
        {
            mServer.Disconnetti();
        }
    }
}
