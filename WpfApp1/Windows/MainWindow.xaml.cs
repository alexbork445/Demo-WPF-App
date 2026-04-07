using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Data;
using WpfApp1.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PaulDbBorkAsContext _context = new();

        public MainWindow()
        {
            Authorization authorization = new Authorization(_context);
            if(authorization.ShowDialog() != true)
            {
                Application.Current.Shutdown();
            }
            InitializeComponent();
        }
    }
}