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
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Windows.OrderWin
{
    /// <summary>
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private PaulDbBorkAsContext _context;
        public AddOrder(PaulDbBorkAsContext context)
        {
            InitializeComponent();
            _context = context;
            BoxStatus.ItemsSource = _context.OrderStatuses.ToList();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxRentalQuantity.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text.Trim()))
            {
                try
                {
                    
                    Order order = new Order()
                    {

                        RentalStartDate = DateTime.Parse(BoxDateOrder.Text.Trim()),
                        RentalQuantity = int.Parse(BoxRentalQuantity.Text.Trim()),
                        ReceiptCode = decimal.Parse(BoxArc.Text.Trim()),
                        PickupPoint = _context.PickupPoints.FirstOrDefault(q => q.Address == BoxDelivary.Text.Trim()),
                        Status = BoxStatus.SelectedItem as OrderStatus
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    DialogResult = true;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
