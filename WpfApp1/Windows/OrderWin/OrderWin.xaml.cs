using Microsoft.EntityFrameworkCore;
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
    /// Логика взаимодействия для OrderWin.xaml
    /// </summary>
    public partial class OrderWin : Window
    {
        private ExampleDbContext _context;

        public OrderWin(ExampleDbContext context)
        {
            InitializeComponent();
            _context = context;

            List<Order> orders = _context.Order
                .Include(q => q.PickupPoint)
                .Include(q => q.OrderStatus)
                .ToList();
            BoxOrder.ItemsSource = orders;

            if (Cookies.LoggedUser.Role.RoleName == "Администратор")
            {
                BoxOrder.MouseDoubleClick += BoxProduct_MouseDoubleClick; ;
                PanelBottomButton.Visibility = Visibility.Visible;
            }
            else
            {
                PanelBottomButton.Visibility = Visibility.Collapsed;
            }
        }

        private void BoxProduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Order order = BoxOrder.SelectedItem as Order;
            if (order != null)
            {
                EditOrder edit = new EditOrder(order, _context);

                if (edit.ShowDialog() == true)
                {
                    BoxOrder.ItemsSource = _context.Order
                        .Include(q => q.PickupPoint)
                        .Include(q => q.OrderStatus)
                        .ToList();
                }
            }
        }

        private void Button_add_reques(object sender, RoutedEventArgs e)
        {
            AddOrder add = new AddOrder(_context);
            if (add.ShowDialog() == true)
            {
                BoxOrder.ItemsSource = _context.Order.ToList();
            }
        }
        private void Buutton_delite_reques(object sender, RoutedEventArgs e)
        {
            Order order = BoxOrder.SelectedItem as Order;
            if (order != null)
            {
                var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.Order == order) ?? null;

                var itemsOfOrder = _context.OrderDetails.Where(od => od.Order == order);
                if (itemsOfOrder != null)
                {
                    _context.OrderDetails.RemoveRange(itemsOfOrder);
                }

                _context.Order.Remove(order);
                _context.SaveChanges();
                BoxOrder.ItemsSource = _context.Order.ToList();
            }
            else
            {
                MessageBox.Show("Выберете заказ для удаления", "Ошибка удаления заказа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            return;
        }
    }
}
