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
        private ExampleDbContext _context;
        public AddOrder(ExampleDbContext context)
        {
            InitializeComponent();
            _context = context;
            BoxStatus.ItemsSource = _context.OrderStatus.ToList();
        }
        private void Button_add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text.Trim()) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text.Trim()))
            {
                try
                {
                    
                    Order order = new Order()
                    {
                        OrderDate = DateOnly.Parse(BoxDateOrder.Text.Trim()),
                        DeliveryDate = DateOnly.Parse(BoxDateDelivery.Text.Trim()),
                        Code = BoxArc.Text.Trim(),
                        PickupPoint = _context.PickupPoint.FirstOrDefault(q => q.Address == BoxDelivary.Text.Trim()),
                        OrderStatus = BoxStatus.SelectedItem as OrderStatus
                    };
                    _context.Order.Add(order);
                    _context.SaveChanges();
                    DialogResult = true;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка добавления заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
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
