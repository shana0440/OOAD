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

namespace WPF_Windows_Spotlight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Adapter _adapter;
        public MainWindow()
        {
            _adapter = new Adapter();
            InitializeComponent();
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            _adapter.Search(Input.Text);
        }
    }
}
